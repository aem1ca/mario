using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFalling : Monster
{
    internal override void CollisionAny(GameObject go)
    {
        base.CollisionAny(go);
        if (isAlive)
        {
            if (go.GetComponent<MonsterFollow>() == null)
            {
                ReplaceMonster();
            }
        }
    }
}
