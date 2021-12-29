using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [SerializeField]
    private StackPartController[] stackPartControllers = null;

    public void ShatterAllParts()
    {
        if(transform.parent != null)
        {
            transform.parent = null;
        }

        //一つづつ(床のパーツ４つ)吹き飛ばしていく
        foreach (StackPartController o in stackPartControllers)
        {
            o.Shatter();
        }

        StartCoroutine("RemoveParts");
    }

    IEnumerator RemoveParts()
    {   //１秒後に処理を開始する
        yield return new WaitForSeconds(1);

        Destroy(gameObject);
    }
}
