using System;
using UnityEngine;

public class GoldenCoin : GenericCoin
{
    public int coinAmount = 1;

    public override void CollectMyself()
    {
        gameGui.AddGoldenCoins(coinAmount);
    }
}
