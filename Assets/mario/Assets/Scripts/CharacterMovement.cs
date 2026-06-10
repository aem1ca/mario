using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    bool turbo = false;


    public static CharacterMovement Instance;
    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            MakeJump();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            MakeJump();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Shoot();
            turbo = true;
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            turbo = false;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    public void MakeJump()
    {
        jump = true;
        animator.SetBool("IsJumping", true);
    }


    public GameObject bulletPrefab;
    public Transform bulletTarget;
    void Shoot()
    {
        GameObject go=Instantiate(bulletPrefab);
        go.transform.position = bulletTarget.position;
        float shootingSpeed = 20;
        float side = 1;
        if (!controller.FacingRight())
        {
            side = -1;
        }
        go.GetComponent<Rigidbody2D>().AddForce(new Vector2(side * 1,-0.4f)*shootingSpeed, ForceMode2D.Impulse);

    }
    
    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }

    void FixedUpdate()
    {
        // Move our character
        float speedMultiply = 1;
        if (turbo)
        {
            speedMultiply = 2;
        }

        controller.Move(horizontalMove * Time.fixedDeltaTime * speedMultiply, crouch, jump);
        jump = false;
    }


    private Coroutine hitCoroutine;
    public bool invincibility = false;
    public void GetHit()
    {
        if (!invincibility)
        {
            //Debug.LogWarning("Get hit!!!");
            MakeJump();
            if (hitCoroutine != null)
            {
                StopCoroutine(hitCoroutine);
            }
            hitCoroutine = StartCoroutine(HitCoroutine());
        }
        else
        {
            //Debug.LogWarning("Dont get hit!!!");
        }
    }

    IEnumerator HitCoroutine()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float invincibilityTime = 3f;
        float blinkingIntensivity = .1f;
        invincibility = true;
        float startTime = Time.time;
        
        
        while (Time.time-startTime<invincibilityTime)
        {
            spriteRenderer.color = new Color(1,1,1,0);
            yield return new WaitForSeconds(blinkingIntensivity);
            spriteRenderer.color = new Color(1,1,1,1);
            yield return new WaitForSeconds(blinkingIntensivity);
        }
        
        invincibility = false;
    }
}