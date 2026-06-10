using System;
using UnityEngine;

public class LivesCoin : GenericCoin
{
    public int liveAmount = 1;
    
    public override void CollectMyself()
    {
        gameGui.AddGoldenCoins(liveAmount);
    }
}