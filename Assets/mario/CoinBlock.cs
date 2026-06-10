using UnityEngine;

public class CoinBlock : Hittable
{
    [SerializeField] GameObject coinPrefab;

    protected override void ProcessHit()
    {
        Instantiate(coinPrefab, transform.position + Vector3.up, Quaternion.identity);
    }
}