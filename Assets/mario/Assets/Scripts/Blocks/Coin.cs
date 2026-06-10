using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectable
{
    protected override void ProcessCollect()
    {
        GameGui.Instance.AddCoins(1);
        GameGui.Instance.AddScore(200);
    }
}
