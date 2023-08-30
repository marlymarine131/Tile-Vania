using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class input : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float jumpHeight = 10f;
    [SerializeField] float climpSpeed = 5f;
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    float GravityScaleAtStart;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        GravityScaleAtStart = myRigidbody.gravityScale;
    }

    void Update()
    {
        run();
        slipChar();
        climpLadder();
    }

    private void slipChar()
    {
        bool turnSigh = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (turnSigh)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);

        }
    }

    void run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * speed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool turnSigh = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", turnSigh);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return; 
        }
        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpHeight);

        }
    }
    void climpLadder(){
        if(!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("ladder")) ){
            myRigidbody.gravityScale = GravityScaleAtStart;
            myAnimator.SetBool("isCliming", false);

            return;
        }

        Vector2 climpVelocity = new Vector2(myRigidbody.velocity.x,moveInput.y * climpSpeed );
        myRigidbody.velocity = climpVelocity;
        myRigidbody.gravityScale = 0f;
        bool turnSigh = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isCliming", turnSigh);
    }
}
