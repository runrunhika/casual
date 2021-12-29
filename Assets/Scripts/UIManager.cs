using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject clearUI, GameOverUI;
    [SerializeField]
    private Text clearLevelText;
    private Player player;

    void Awake()
    {
        player = FindObjectOfType<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        //PlayerStateに合わせてUIの on off切り替える
        if(player.playerState == Player.PlayerState.Finish)
        {
            if(!clearUI.activeInHierarchy)
            {
                clearLevelText.text = "Level" + FindObjectOfType<LevelSpawns>().level;
            }

            clearUI.SetActive(true);
            GameOverUI.SetActive(false);
        }

        if(player.playerState == Player.PlayerState.Died)
        {
            clearUI.SetActive(false);
            GameOverUI.SetActive(true);

            if(Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
