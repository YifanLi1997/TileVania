using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] Collider2D footCol;
    [SerializeField] Vector2 deathKick = new Vector2(200f, 200f);

    // State
    bool isAlive = true;
    float myGravity;

    // Cached component references 
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator animator;
    Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        myGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAlive)
        {
            Run();
            Jump();
            ClimbLadder();
        }
        else
        {
            rb.gravityScale = myGravity;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            rb.velocity = deathKick;
        }
    }

    private void Run()
    {
        float inputDirection = CrossPlatformInputManager.GetAxis("Horizontal");
        rb.velocity = new Vector2(inputDirection * runSpeed, rb.velocity.y);

        bool isRunning = Mathf.Abs(inputDirection) > Mathf.Epsilon;

        // flip the sprite
        if (isRunning)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }

        animator.SetBool("Running", isRunning);
    }

    private void Jump()
    {
        if (!footCol.IsTouchingLayers(LayerMask.GetMask("Jump Ground"))) { return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            // Debug.Log("Jump pressed!");
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpForce);
            rb.velocity += jumpVelocityToAdd;
        }
    }

    private void ClimbLadder()
    {
        if (!footCol.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            animator.SetBool("Climbing", false);
            rb.gravityScale = myGravity;
            return;
        }

        float climbDirection = CrossPlatformInputManager.GetAxis("Vertical");
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(rb.velocity.x, climbDirection * climbSpeed);

        bool isClimbing = Mathf.Abs(climbDirection) > Mathf.Epsilon;
        animator.SetBool("Climbing", isClimbing);
    }
}
