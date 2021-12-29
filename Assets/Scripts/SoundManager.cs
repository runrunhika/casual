using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //シングルトン化
    public static SoundManager instance;

    private AudioSource audioSource;

    private void Awake()
    {
        Singleton();

        audioSource = GetComponent<AudioSource>();
    }

    void Singleton()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        //ロードしてもSoundManagerは壊れない
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySE(AudioClip clip)
    {   
        //1回鳴らす
        audioSource.PlayOneShot(clip);
    }
}
