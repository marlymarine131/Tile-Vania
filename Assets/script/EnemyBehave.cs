using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehave : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D rigidBodyEnemy;
    void Start()
    {
        rigidBodyEnemy = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rigidBodyEnemy.velocity = new Vector2(moveSpeed, 0f);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        flipEnemy();

    }

    private void flipEnemy()
    {
        bool turnCondition = Mathf.Abs(rigidBodyEnemy.velocity.x) > Mathf.Epsilon;
        if (turnCondition)
        {
            transform.localScale = new Vector2(-(Mathf.Sign(rigidBodyEnemy.velocity.x)), 1f);

        }
    }
}
