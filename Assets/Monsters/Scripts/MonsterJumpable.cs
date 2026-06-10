using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterJumpable : MonsterSimpleShell
{

    [SerializeField] private float m_JumpForce = 100f;

    internal override void MoveAction()
    {
        base.MoveAction();
        if (IsGrounded())
        {
            m_Rigidbody2D.linearVelocity = new Vector2(m_Rigidbody2D.linearVelocity.x,0);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            SetIsGrounded(false);
        }
    }
}
