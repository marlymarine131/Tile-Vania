using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float jumpHeight = 10f;
    [SerializeField] float climpSpeed = 5f;
    [SerializeField] Vector2 deathAnimation  = new Vector2(10f,10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    bool isAlive = true;
    float GravityScaleAtStart;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        GravityScaleAtStart = myRigidbody.gravityScale;
    }

    void Update()
    {
        if (!isAlive) { return; }
        run();
        slipChar();
        climpLadder();
        die();
    }

    private void die()
    {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) ||
        myFeetCollider.IsTouchingLayers(LayerMask.GetMask("trap"))){
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathAnimation;
        }
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
        if (!isAlive) { return; }

        Vector2 playerVelocity = new Vector2(moveInput.x * speed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool turnSigh = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", turnSigh);
    }
    void OnFire(InputValue value){
        if (!isAlive) { return; }
        Instantiate(bullet, gun.position,transform.rotation);

    }
    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }

        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }

        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("map")))
        {
            return;
        }
        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpHeight);

        }
    }
    
    void climpLadder()
    {
        if (!isAlive) { return; }

        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("ladder")))
        {
            myRigidbody.gravityScale = GravityScaleAtStart;
            myAnimator.SetBool("isCliming", false);
            return;
        }

        Vector2 climpVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climpSpeed);
        myRigidbody.velocity = climpVelocity;
        myRigidbody.gravityScale = 0f;

        bool turnSigh = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isCliming", turnSigh);
    }
}
