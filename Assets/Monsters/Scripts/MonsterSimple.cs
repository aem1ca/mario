using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSimple : Monster
{
    internal override void MoveAction()
    {
        if (canMove)
        {
            if (IsGrounded())
            {
                m_Rigidbody2D.linearVelocity = new Vector2(Speed * directionX, m_Rigidbody2D.linearVelocity.y);
            }
        }
    }

    internal void TryToFlipDirectionFromWall(GameObject other)
    {
        if (!other.CompareTag("Player"))
        {
            FlipDirection();
        }
    }
    
    internal override void CollisionLeft(GameObject other)
    {
        base.CollisionLeft(other);
        TryToFlipDirectionFromWall(other);
    }

    internal override void CollisionRight(GameObject other)
    {
        base.CollisionRight(other);
        TryToFlipDirectionFromWall(other);
    }

}