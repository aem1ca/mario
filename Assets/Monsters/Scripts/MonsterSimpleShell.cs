using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSimpleShell : MonsterSimple
{
    internal override void CollisionTop(GameObject go)
    {
        if (go.CompareTag("Player"))
        {
            CharacterMovement.Instance.MakeJump();
            ReplaceMonster();
        }
    }
    

}
