using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private bool touch;
    [SerializeField]
    private float maxSpeed;

    private float currentTime;
    private bool invincible;
    //炎エフェクト格納
    [SerializeField]
    private GameObject fireEffect;
    [SerializeField]
    private GameObject goalEffect;
    [SerializeField]
    private GameObject splashEffect;

    //SE
    [SerializeField]
    private AudioClip bounceClip, explosionClip, goalClip, shatterClip;

    [SerializeField]
    private GameObject invincbleObj;

    [SerializeField]
    private Image image;


    //列挙型
    public enum PlayerState
    {
        Prepre,
        Playing,
        Died,
        Finish
    }

    [HideInInspector]
    public PlayerState playerState = PlayerState.Prepre;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // currentTime = 0.7f;
    }

    // Update is called once per frame
    void Update()
    {
        TouchCheck();
        SpeedCheck();
        InvincibleCheck();
        FinishGameCheck();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!touch)
        {
            rb.velocity = new Vector3(0,5,0);

            if(collision.gameObject.tag != "Finish")
            {
                SplashEffect(collision);
            }
            SoundManager.instance.PlaySE(bounceClip);
        }
        else
        {
            if(invincible)
            {
                if(collision.gameObject.tag == "Normal" || collision.gameObject.tag == "Enemy")
                {
                    // Destroy(collision.transform.parent.gameObject);
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();

                    SoundManager.instance.PlaySE(shatterClip);
                }
            }
            else
            {   
                if(collision.gameObject.tag == "Normal")
                {
                    // Destroy(collision.transform.parent.gameObject);
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();

                    SoundManager.instance.PlaySE(shatterClip);
                }
                //GameOver
                else if(collision.gameObject.tag == "Enemy")
                {
                    playerState = PlayerState.Died;
                    //物理的影響を受けなくなる
                    rb.isKinematic = true;
                    //Inspectorのチェックが外れて、消える
                    gameObject.SetActive(false);

                    SoundManager.instance.PlaySE(explosionClip);
                }
            }
        }

        //ゴールする（PlayerがPlay中にゴールに触れた）
        if(collision.gameObject.tag == "Finish" && playerState == PlayerState.Playing)
        {
            playerState = PlayerState.Finish;
            GoalEffect();

            SoundManager.instance.PlaySE(goalClip);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(!touch)
        {
            rb.velocity = new Vector3(0,5,0);
        }
    }

    /// <summary>
    /// マウスが押されているときにプレイヤーを下に動かす
    /// </summary>
    void TouchCheck()
    {
        if(Input.GetMouseButton(0))
        {
            //Playerがゲーム開始したとき
            if(playerState == PlayerState.Prepre)
            {
                playerState = PlayerState.Playing;
            }

            touch = true;
            rb.velocity = new Vector3(0,-100 * Time.fixedDeltaTime * 7 ,0);
        }

        if(Input.GetMouseButtonUp(0))
        {
            touch = false;

        }
    }

    void SpeedCheck()
    {
        if(rb.velocity.y > maxSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxSpeed, rb.velocity.z);
        }
    }

    // 無敵仕様
    void InvincibleCheck()
    {
        if(invincible)
        {
            currentTime -= Time.deltaTime * 0.35f;

            //Hierarchyに表示されてないとき
            if(!fireEffect.activeInHierarchy)
            {
                fireEffect.SetActive(true);
            }
        }
        else
        {
            if(fireEffect.activeInHierarchy)
            {
                fireEffect.SetActive(false);
            }

            if(touch)
            {
                currentTime += Time.deltaTime * 0.8f;
            }
            else
            {
                currentTime -= Time.deltaTime * 0.5f;
            }
        }

        if(currentTime >= 0.15 || image.color == Color.red)
        {
            invincbleObj.SetActive(true);
        }
        else
        {
            invincbleObj.SetActive(false);
        }

        if(currentTime >= 1)
        {
            currentTime = 1;
            invincible = true;

            image.color = Color.red;
        }
        else if(currentTime <= 0)
        {
            currentTime = 0;
            invincible = false;

            image.color = Color.white;
        }

        //UI(Play)が表示されているか判定
        if(invincbleObj.activeInHierarchy)
        {
            image.fillAmount = currentTime / 1f;
        }
    }

    void FinishGameCheck()
    {
        if(playerState == PlayerState.Finish)
        {
            if(Input.GetMouseButtonDown(0))
            {
                FindObjectOfType<LevelSpawns>().NextLevel();
            }
        }
    }

    void GoalEffect()
    {   //生成
        GameObject goal = Instantiate(goalEffect);
        //MainCameraを親として設定し、位置取得
        goal.transform.SetParent(Camera.main.transform);
        //ゴールの位置取得
        goal.transform.localPosition = Vector3.up * 1.5f;
        //回転をなくす
        goal.transform.eulerAngles = Vector3.zero;
    }

    void SplashEffect(Collision target)
    {
        GameObject splash = Instantiate(splashEffect);
        splash.transform.SetParent(target.transform);
                                                    //Inspector > Transform > RotationX
        splash.transform.localEulerAngles = new Vector3(90, Random.Range(0, 359), 0);
        //Effect Size Random
        float randomScalse = Random.Range(0.0038f, 0.0055f);
        splash.transform.localScale = new Vector3(randomScalse, randomScalse, 1);
        splash.transform.position = new Vector3(transform.position.x, transform.position.y -0.22f, transform.position.z);
    }
}
