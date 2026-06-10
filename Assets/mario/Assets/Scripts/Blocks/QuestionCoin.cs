using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionCoin : Hittable
{
    protected override void ProcessHit()
    {
        GameGui.Instance.AddCoins(1);
        GameGui.Instance.AddScore(200);
        //TODO replace sprite? disable?
    }
}