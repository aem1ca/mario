using UnityEngine;

public class InvisibleBlock : Hittable
{
    protected override void Awake()
    {
        base.Awake();
        sr.enabled = false; // hide it at the start
    }

    protected override void ProcessHit()
    {
        sr.enabled = true; // show it when hit
    }
}