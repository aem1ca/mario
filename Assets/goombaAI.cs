using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class GoombaAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float patrolDistance = 4f;   // max distance from spawn before turning

    [Header("Detection")]
    [SerializeField] private float groundCheckDistance = 0.15f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckLeft;   // assign in Inspector (optional)
    [SerializeField] private Transform groundCheckRight;  // assign in Inspector (optional)

    [Header("Stomp")]
    [SerializeField] private float stompWindowY = 0.4f;   // how far above goomba top counts as stomp
    [SerializeField] private float stompBounceForce = 8f;
    [SerializeField] private string playerTag = "Player";

    [Header("Player Kill")]
    [SerializeField] private string playerDeathMethod = "Die"; // method name to call on player

    [Header("Sprites (optional)")]
    [SerializeField] private Sprite walkSprite1;
    [SerializeField] private Sprite walkSprite2;
    [SerializeField] private Sprite stompedSprite;
    [SerializeField] private float walkAnimInterval = 0.2f;

    // ── internal ──────────────────────────────────────────────────────────────
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D col;

    private Vector2 spawnPos;
    private float facingDir = -1f;   // -1 = left, 1 = right
    private bool isDead = false;

    // walk animation
    private float animTimer;
    private bool animFrame;

    // ─────────────────────────────────────────────────────────────────────────

    private void Awake()
    {
        rb  = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr  = GetComponent<SpriteRenderer>();

        rb.freezeRotation = true;
        rb.gravityScale   = 3f;

        spawnPos = transform.position;
    }

    private void Update()
    {
        if (isDead) return;
        AnimateWalk();
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        Move();
        CheckEdgeOrWall();
    }

    // ── movement ──────────────────────────────────────────────────────────────

    private void Move()
    {
        rb.linearVelocity = new Vector2(facingDir * moveSpeed, rb.linearVelocity.y);

        // flip sprite to match direction
        if (sr != null)
            sr.flipX = facingDir > 0f;
    }

    private void CheckEdgeOrWall()
    {
        // --- patrol distance limit ---
        float distFromSpawn = transform.position.x - spawnPos.x;
        if ((facingDir > 0f && distFromSpawn > patrolDistance) ||
            (facingDir < 0f && distFromSpawn < -patrolDistance))
        {
            Flip();
            return;
        }

        // --- edge detection (no ground ahead) ---
        bool leftGrounded  = CheckGround(groundCheckLeft,  Vector2.left);
        bool rightGrounded = CheckGround(groundCheckRight, Vector2.right);

        if (facingDir < 0f && !leftGrounded)  { Flip(); return; }
        if (facingDir > 0f && !rightGrounded) { Flip(); return; }
    }

    // Returns true if ground exists below the chosen foot (or a fallback offset)
    private bool CheckGround(Transform checkPoint, Vector2 side)
    {
        Vector2 origin;

        if (checkPoint != null)
        {
            origin = checkPoint.position;
        }
        else
        {
            // Auto-compute from collider bounds
            Bounds b = col.bounds;
            float xOffset = side == Vector2.left ? -b.extents.x - 0.05f : b.extents.x + 0.05f;
            origin = new Vector2(b.center.x + xOffset, b.min.y + 0.05f);
        }

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }

    private void Flip()
    {
        facingDir *= -1f;
    }

    // ── collision ─────────────────────────────────────────────────────────────

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isDead) return;

        // Hitting a wall → turn around
        if (((1 << other.gameObject.layer) & groundLayer.value) != 0)
        {
            foreach (ContactPoint2D cp in other.contacts)
            {
                if (Mathf.Abs(cp.normal.x) > 0.5f)
                {
                    Flip();
                    break;
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (isDead) return;
        if (!other.gameObject.CompareTag(playerTag)) return;

        HandlePlayerContact(other.gameObject, other.contacts);
    }

    private void HandlePlayerContact(GameObject player, ContactPoint2D[] contacts)
    {
        float topY = col.bounds.max.y;

        foreach (ContactPoint2D cp in contacts)
        {
            // Player is landing on top of the goomba
            if (cp.point.y >= topY - stompWindowY)
            {
                // Bounce the player up
                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, stompBounceForce);

                Die();
                return;
            }
        }

        // Side or bottom contact → kill player
        player.SendMessage(playerDeathMethod, SendMessageOptions.DontRequireReceiver);
    }

    // ── death ─────────────────────────────────────────────────────────────────

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType  = RigidbodyType2D.Kinematic;
        col.enabled  = false;

        if (sr != null && stompedSprite != null)
            sr.sprite = stompedSprite;

        Destroy(gameObject, 0.4f);
    }

    // ── walk animation ────────────────────────────────────────────────────────

    private void AnimateWalk()
    {
        if (walkSprite1 == null || walkSprite2 == null || sr == null) return;

        animTimer += Time.deltaTime;
        if (animTimer >= walkAnimInterval)
        {
            animTimer = 0f;
            animFrame = !animFrame;
            sr.sprite = animFrame ? walkSprite1 : walkSprite2;
        }
    }

    // ── editor gizmos ─────────────────────────────────────────────────────────

    private void OnDrawGizmosSelected()
    {
        // Patrol range
        Gizmos.color = Color.yellow;
        Vector3 sp = Application.isPlaying ? (Vector3)spawnPos : transform.position;
        Gizmos.DrawLine(sp + Vector3.left  * patrolDistance, sp + Vector3.right * patrolDistance);

        // Ground-check rays
        if (col == null) col = GetComponent<Collider2D>();
        if (col == null) return;

        Gizmos.color = Color.cyan;
        Bounds b = col.bounds;

        Vector3 leftOrigin  = new Vector3(b.min.x - 0.05f, b.min.y + 0.05f, 0f);
        Vector3 rightOrigin = new Vector3(b.max.x + 0.05f, b.min.y + 0.05f, 0f);

        Gizmos.DrawRay(leftOrigin,  Vector3.down * groundCheckDistance);
        Gizmos.DrawRay(rightOrigin, Vector3.down * groundCheckDistance);

        // Stomp zone
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(b.center.x, b.max.y - stompWindowY * 0.5f, 0f),
                            new Vector3(b.size.x, stompWindowY, 0f));
    }
}