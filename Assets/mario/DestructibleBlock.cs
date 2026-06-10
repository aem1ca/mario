using UnityEngine;

public class DestructibleBlock : Hittable
{
    protected override void ProcessHit()
    {
        Destroy(gameObject);
    }
}