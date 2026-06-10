using UnityEngine;

public class StarBlock : Hittable
{
    [SerializeField] GameObject starPrefab;

    protected override void ProcessHit()
    {
        Instantiate(starPrefab, transform.position + Vector3.up, Quaternion.identity);
    }
}