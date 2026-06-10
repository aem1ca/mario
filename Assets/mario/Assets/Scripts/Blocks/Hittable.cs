using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour
{
    [SerializeField] protected Sprite usedSprite;
    protected bool hasBeenHit = false;
    protected SpriteRenderer sr;

    protected virtual void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var normal = other.contacts[0].normal;
            if (normal.y < 0 && !hasBeenHit)
            {
                hasBeenHit = true;
                sr.sprite = usedSprite;
                ProcessHit();
            }
        }
    }

    protected virtual void ProcessHit()
    {
        
    }
}