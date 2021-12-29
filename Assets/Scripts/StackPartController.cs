using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackPartController : MonoBehaviour
{
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private Collider cod;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        cod = GetComponent<Collider>();
    }

    //床オブジェクトの吹き飛ばし関数
    public void Shatter()
    {
        rb.isKinematic = false;
        //コンポーネント単位でオフにする
        cod.enabled = false;
                            //親の位置から、力を加える
        Vector3 forcePoint = transform.parent.position;
                            //親のｘ軸を取得
        float parentX = transform.parent.position.x;
                                //3次元空間の範囲.中心座標.ｘ
        float x = meshRenderer.bounds.center.x;
        //右か左のどちらに力が働いてるか判定し、力が働いている方向へ床オブジェクトを飛ばす
        Vector3 subDir = (parentX - x < 0) ? Vector3.right : Vector3.left;

        Vector3 dir = (Vector3.up * 1.5f + subDir).normalized;
        //与えられる力はランダムに決める
        float force = Random.Range(20, 35);
        float torque = Random.Range(110, 180);
        //力を与える（場所も指定）                       //力を一瞬で与えることのできるもの
        rb.AddForceAtPosition(force * dir, forcePoint, ForceMode.Impulse);
        //回転
        rb.AddTorque(Vector3.left * torque);
        //下に落ちる
        rb.velocity = Vector3.down;
    }
}
