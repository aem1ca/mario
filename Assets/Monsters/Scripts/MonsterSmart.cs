using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSmart : MonsterSimpleShell
{
    [SerializeField] internal Transform m_NextGroundCheck;
    
    internal override void MoveAction()
    {
        base.MoveAction();
        if (IsGrounded())
        {
            bool groundedNext = false;
            Collider2D[] colliders =
                Physics2D.OverlapCircleAll(m_NextGroundCheck.position, k_GroundedRadius, props.WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject && colliders[i].gameObject != m_NextGroundCheck)
                {
                    groundedNext = true;
                }
            }
            
            if (!groundedNext)
            {
                FlipDirection();
            }
        }
    }
}
