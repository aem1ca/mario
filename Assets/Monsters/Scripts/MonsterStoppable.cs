using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStoppable : MonsterSimple
{
    public bool frozen = false;
    public int unfreezeTimeout = 3;
    private float frozenTime = 0;

    internal override void Start()
    {
        base.Start();
        if (frozen)
        {
            frozenTime = Time.time;
        }
    }

    internal override void MoveAction()
    {
        if (!frozen)
        {
            base.MoveAction();
        }
        else
        {
            if (canMove)
            {
                if (IsGrounded())
                {
                    m_Rigidbody2D.linearVelocity = new Vector2(0, m_Rigidbody2D.linearVelocity.y);

                    if (Time.time - frozenTime > unfreezeTimeout)
                    {
                        ReplaceMonster();
                    }
                }
            }
        }
    }

    internal override void CollisionLeft(GameObject go)
    {
        TryToFlipDirectionFromWall(go);
        if (go.CompareTag("Player"))
        {
            if (frozen)
            {
                frozen = !frozen;
                directionX = 1;
            }
            else
            {
                CharacterMovement.Instance.GetHit();
            }
        }
        else if (go.CompareTag("Enemy"))
        {
            if (!frozen)
            {
                go.GetComponent<Monster>().Hit();
            }
        }
    }

    internal override void CollisionRight(GameObject go)
    {
        TryToFlipDirectionFromWall(go);
        if (go.CompareTag("Player"))
        {
            if (frozen)
            {
                frozen = !frozen;
                directionX = -1;
            }
            else
            {
                CharacterMovement.Instance.GetHit();
            }
        }
        else if (go.CompareTag("Enemy"))
        {
            if (!frozen)
            {
                go.GetComponent<Monster>().Hit();
            }
        }
    }

    internal override void CollisionTop(GameObject go)
    {
        if (go.CompareTag("Player"))
        {
            CharacterMovement.Instance.MakeJump();
            frozen = !frozen;
            if (frozen)
            {
                frozenTime = Time.time;
            }
        }
    }
}