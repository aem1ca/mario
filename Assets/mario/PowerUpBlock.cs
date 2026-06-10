using UnityEngine;

public class PowerUpBlock : Hittable
{
    [SerializeField] GameObject powerUpPrefab;

    protected override void ProcessHit()
    {
        Instantiate(powerUpPrefab, transform.position + Vector3.up, Quaternion.identity);
    }
}