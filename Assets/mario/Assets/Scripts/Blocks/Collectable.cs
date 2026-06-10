using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("Collected: "+name);
            ProcessCollect();
            Destroy(gameObject);
        }
    }

    protected virtual void ProcessCollect()
    {
    }
}
