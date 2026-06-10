using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Monster : MonoBehaviour
{

    internal bool canMove = false;
    private float minDistanceToMove = 14f;


    public bool canBeKilledByFire = true;
    public bool canBeKilledJump = true;
    public bool damagePlayerWhenJump = false;


    [Header("Diriection - 1 = right, -1 = left")]
    public float directionX = 1;

    public float Speed = 3;


    public GameObject replacementMonster;
    
    
    [SerializeField] internal MonsterProps props;
    internal Rigidbody2D m_Rigidbody2D;
    internal Transform m_GroundCheck;
    internal const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded; // Whether or not the player is grounded.

    
    //for checking, if isnt too young to be replaced
    private float bornTime;
    internal bool isAlive = true;

    internal virtual void Start()
    {
        bornTime = Time.time;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        if (m_GroundCheck == null)
        {
            m_GroundCheck = transform;
        }

        OrientSprite();
    }

    internal void OrientSprite()
    {
        if (directionX > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (directionX < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    internal void FlipDirection()
    {
        //Debug.Log("Flipping direction for" + name);
        directionX = -directionX;
        OrientSprite();
    }


    void SetCanMove()
    {
        if (!canMove & Mathf.Abs(CharacterMovement.Instance.transform.position.x - transform.position.x) <= minDistanceToMove)
        {
            canMove = true;
        }
    }


    void Update()
    {
        SetCanMove();
    }

    internal virtual void MoveAction()
    {
    }
    void FixedUpdate()
    {
        CheckIfIsGrounded();
        MoveAction();
    }

    
    public bool IsGrounded()
    {
        return m_Grounded;
    }

    public void SetIsGrounded(bool newIsGrounded = false)
    {
        m_Grounded = newIsGrounded;
    }

    void CheckIfIsGrounded()
    {
        m_Grounded = false;
        {
            Collider2D[] colliders =
                Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, props.WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject && colliders[i].gameObject != m_GroundCheck)
                {
                    m_Grounded = true;
                }
            }
        }
    }

    internal virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(directionX, 0));
    }


    internal void ReplaceMonster()
    {
        if (Time.time - bornTime > 0.1f)
        {
            //Debug.Log("replacing monster "+name+" with "+replacementMonster.name);
            GameObject go = Instantiate(replacementMonster);
            go.transform.position = transform.position;
            go.GetComponent<Monster>().directionX = directionX;
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("NOT replacing monster " + name + " with " + replacementMonster.name);
        }
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        bool leftHit = false;
        bool rightHit = false;
        bool bottomHit = false;
        bool topHit = false;

        var hit = other.contacts[0];
        Vector2 normal = other.contacts[0].normal;

        float f = 0.5f;

        if (Mathf.Abs(hit.normal.y) > f)
        {
            if (hit.normal.y > 0f)
                bottomHit = true;
            else
                topHit = true;
        }
        else if (Mathf.Abs(hit.normal.x) > f)
        {
            if (hit.normal.x > 0f)
                leftHit = true;
            else
                rightHit = true;
        }
        
        CollisionAny(other.gameObject);

        if (leftHit)
        {
            CollisionLeft(other.gameObject);
        }

        if (rightHit)
        {
            CollisionRight(other.gameObject);
        }

        if (bottomHit)
        {
            CollisionBottom(other.gameObject);
        }

        if (topHit)
        {
            CollisionTop(other.gameObject);
        }
    }

    internal virtual void CollisionAny(GameObject go)
    {
        if (go.CompareTag("PlayerBullet"))
        {
            Destroy(go);
            if (canBeKilledByFire)
            {
                Hit();
            }
        }
    }

    internal virtual void CollisionLeft(GameObject go)
    {
        if (go.CompareTag("Player"))
        {
            CharacterMovement.Instance.GetHit();
        }
    }

    internal virtual void CollisionRight(GameObject go)
    {
        if (go.CompareTag("Player"))
        {
            CharacterMovement.Instance.GetHit();
        }
    }

    internal virtual void CollisionBottom(GameObject go)
    {
        if (go.CompareTag("Player"))
        {
            CharacterMovement.Instance.GetHit();
        }
    }

    internal virtual void CollisionTop(GameObject go)
    {
        if (go.CompareTag("Player"))
        {
            if (canBeKilledJump)
            {
                Hit();
            }

            if (damagePlayerWhenJump)
            {
                CharacterMovement.Instance.GetHit();
            }
        }
    }

    public virtual void Hit()
    {
        Destroy(gameObject, 10f);
        this.enabled = false;
        isAlive = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1) * 1, ForceMode2D.Impulse);
    }
}