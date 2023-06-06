using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float Speed;
    [SerializeField] private float JumpStrength;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float WallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        //creates reference from the object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * Speed, body.velocity.y);
        
        //flips to left and right
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(1.4f, 1.4f, 1);  
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1.4f, 1.4f, 1);

        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) && isGrounded())
            Jump();

        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("isGrounded", isGrounded());

        //Wall jump logic 
        if (WallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * Speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 2.45f;
            
            if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)))
                Jump();
        }
        else
            WallJumpCooldown += Time.deltaTime;
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, JumpStrength);
            anim.SetTrigger("jump");   
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else 
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            WallJumpCooldown = 0;
            
        }
    }
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != false;
    }
    
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0,
            new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != false;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
    
}
