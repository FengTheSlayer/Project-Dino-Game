using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    //sfx
    public AudioSource hurtSFX;
    public AudioSource dashSFX;
    
    public ParticleSystem sparks;
    public ParticleSystem dust;
    //for player controls
    public float stunTimer;
    public bool grindRail = false;
    private float horizontal;
    private float speed = 12.5f;
    private float jumpPower = 18f;
    private bool isFacingRight = true;
    
    public healthmanager healthbar;
    private Animator anim;

    //for wallJumps
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWalljumping;
    private float wallJumpDirection;
    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;
    private float wallJumpDuration = 0.4f;
    private Vector2 wallJumpPower = new Vector2(6f, 17f);
    
    //for dashing
    private bool canDash = true;
    private bool isDashing;
    private float dashPower = 11.5f;
    private float dashTime = 0.2f;
    private float dashCooldown = 1.2f;
    private int resource = 3;
    
    //for dashingVertical
    private bool canDashVertical = true;
    private float dashPowerVertical = 11.5f;
    private float dashTimeVertical = 0.2f;
    private float dashVerticalCooldown = 1.2f;
    
    //for dashDown
    private bool canDashDown = true;
    
    //jump buffering (makes jumping satisfying to use)
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    //require link to source
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public TrailRenderer trailRenderer;
    public Transform wallCheck;
    public LayerMask wallLayer;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    /*
    onCollisionEnter(){
        if(the thing i hit was an enemy){
            //play stun animation (you're doing this already)
            StunTimer = 1; (for a 1 second stun)
            //any other code that happens here, like taking damage;
        }
    }
    */
    // Update is called once per frame
    void Update()
    {
        
        if(stunTimer > 0)
        { 
            //(make sure this is at the top of update)
            stunTimer -= Time.deltaTime; //(make the stun timer always be going down)
            print("stun");
            return;
        }
        
        if (isDashing)
        {
            return;
        }
        
        //rail grinding mechanic
        if (grindRail == true)
        {
            if (isFacingRight)
            {
                rb.velocity = new Vector2(20, 0);
                sparkEffect();
                print("on Rail");
            }
            else
            {
                rb.velocity = new Vector2(-20, 0);
                sparkEffect();
            }

            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                grindRail = false;
            }
            
            Debug.Log("RAIL: " + rb.velocity);
            return;
        }
        
        horizontal = Input.GetAxisRaw("Horizontal");

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
            dustEffect();
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jumpBufferCounter = 0f;
        }
        
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            dustEffect();
            coyoteTimeCounter = 0f;
        }
        
        //dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.P))
        {
            dashSFX.Play();
            
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && canDash)
            {
                StartCoroutine(Dash());
            }
            else if (Input.GetKey(KeyCode.W) && canDashVertical)
            {
                StartCoroutine(DashUp());
            }
            
            else if (Input.GetKey(KeyCode.S) && canDashDown)
            {
                StartCoroutine(DashDown());
            }
        }

        anim.SetFloat("speed", Mathf.Abs(horizontal));
        anim.SetBool("jump", IsGrounded());
        anim.SetBool("wallJump", isWallSliding);
        wallSlide();
        WallJump();
        if (!isWalljumping)
        {
            flip();
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        if (!isWalljumping && !grindRail)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    
    private void flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashTime);
        trailRenderer.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    
    private IEnumerator DashUp()
    {
        canDashVertical = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(0f, transform.localScale.y * dashPowerVertical);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashTimeVertical);
        trailRenderer.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashVerticalCooldown);
        canDashVertical = true;
    }
    
    private IEnumerator DashDown()
    {
        canDashDown = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(0f, transform.localScale.y * -dashPowerVertical);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashTimeVertical);
        trailRenderer.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDashDown = true;
    }

    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    private void wallSlide()
    {
        if (isWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWalljumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpCounter = wallJumpTime;
            
            CancelInvoke(nameof(stopWallJumping));
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpCounter > 0f)
        {
            isWalljumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpCounter = 0f;
            
            //swap player direction after jumping off wall
            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            Invoke(nameof(stopWallJumping), wallJumpDuration);
        }
    }
    private void stopWallJumping()
    {
        isWalljumping = false;
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("bullet"))
        {
            healthbar.takeDamage(10);
            anim.Play("player hit");
            stunTimer = .5f;
            hurtSFX.Play();
        }
        
        //player grind rail
        if (col.gameObject.CompareTag("rail"))
        {
            anim.Play("player jump");
            grindRail = true;
        } 
        /*
        if (col.gameObject.CompareTag("uphealth"))
        {
            healthbar.heal(5);
            
        }
        */
    }
    
    
    private void OnCollisionExit2D(Collision2D col)
    {
        grindRail = false;
    }
    
    //partical effects
    void sparkEffect()
    {
        sparks.Play();
    }
    
    void dustEffect()
    {
        dust.Play();
    }
}
