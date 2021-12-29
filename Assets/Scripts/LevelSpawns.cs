using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSpawns : MonoBehaviour
{
    //床
    [SerializeField]
    private GameObject[] model;
    //ゴール
    [SerializeField]
    private GameObject goal;
    //生成したオブジェクトを一時的に格納
    private GameObject temp1, temp2;
    //ステージのレベル ・ 生成するステージの長さ
    public int level = 1, addOn = 10;
    float i = 0;

    //ステージ生成コード
    private void Awake()
    {
        //LevelのKeyに保存されていないとき
        if(!PlayerPrefs.HasKey("Level"))
        {
            PlayerPrefs.SetInt("Level", 1);
        }

        level = PlayerPrefs.GetInt("Level");

        if(level > 9)
        {
            addOn = 0;
        }

        for(i = 0; i > -level - addOn; i -= .5f)
        {
            //床生成
            if(level <= 20)
            {       //Prefab貸したobjectを生成できる
                temp1 = Instantiate(model[Random.Range(0, 2)]);
            }
            if(level > 20 && level <= 50)
            {       
                temp1 = Instantiate(model[Random.Range(1, 3)]);
            }
            if(level > 50 && level <= 100)
            {       
                temp1 = Instantiate(model[Random.Range(2, 4)]);
            }
            if(level > 100)
            {      
                temp1 = Instantiate(model[Random.Range(3, 4)]);
            }

            //輪を作るごとに少しづつ下に下げる
            temp1.transform.position = new Vector3(0, i - 0.01f, 0);
            //少しづつ回転して生成する
            temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);
            //生成された床オブジェクトの親をRotatorにする => 親が回転 => 子も回転
            temp1.transform.parent = FindObjectOfType<Rotator>().transform;
        }

        temp2 = Instantiate(goal);
        temp2.transform.position = new Vector3(0, i -5f, 0);
    }

    public void NextLevel()
    {   
        //Save機能
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        SceneManager.LoadScene(0);
    }
}
