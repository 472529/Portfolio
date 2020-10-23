using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fox : MonoBehaviour
{
    [SerializeField] public float speed = 2;

    Rigidbody2D rb;
    Animator animator;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;
    const float groundCheckRadius = 0.2f;
    float horizontalValue;
    float runSpeedModifier = 2f;
    [SerializeField] float jumpPower = 10;
    [SerializeField] bool isGrounded;
    bool isRunning;
    bool facingRight = true;
    bool jump;




    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //store horizontal value
        horizontalValue = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isRunning = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            isRunning = false;

        if (Input.GetButtonDown("Jump"))
            jump = true;

        else if (Input.GetButtonUp("Jump"))
            jump = false;
        

       
    }

    void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue, jump);
    }

    void GroundCheck()
    {
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if(colliders.Length > 0)
            isGrounded = true;
    }

    void Move(float dir, bool jumpFlag)
    {
        if(isGrounded && jumpFlag)
        {
            isGrounded = false;
            jumpFlag = false;
            rb.AddForce(new Vector2(0f, jumpPower));
        }

        float xVal = dir * speed * 100 * Time.fixedDeltaTime;

        if (isRunning)
            xVal *= runSpeedModifier;
        Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
        rb.velocity = targetVelocity;

        if(facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }

        else if(!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }
        //set xvelocity accorirding to the x value of rigidbody
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
    }

    
}
