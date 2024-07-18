using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;

    Vector2 movement;

    [Header("Import")]
    public Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
 
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        anim.SetFloat("speed", movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.Movable)
        {
            //physics
            rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime); //constant movement spd by applying vector
            sr.flipX = movement.x > 0f; //by default is walking left, flip when walking right
        }
       
    }


}
