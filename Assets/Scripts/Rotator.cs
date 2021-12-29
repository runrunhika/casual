using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private float speed = 100;
    
    // Update is called once per frame
    void Update()
    {
        //床を回転させる                Pcのスペック関係なく数値を出してくる
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }
}
