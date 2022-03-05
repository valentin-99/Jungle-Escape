using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : Enemy
{
    [SerializeField] private LayerMask ground;
    [SerializeField] private float leftLimit;
    [SerializeField] private float rightLimit;
    [SerializeField] private float jumpLength;
    [SerializeField] private float jumpHeight;
    [SerializeField] private AudioSource croak;
    [SerializeField] private AudioSource explosion;

    private bool moveLeft;

    protected override void Start()
    {
        base.Start();

        leftLimit = transform.position.x - 3f;
        rightLimit = transform.position.x + 3f;
        jumpLength = 2f;
        jumpHeight = 4f;
        moveLeft = true;
    }

    private void Update()
    {
        // If the frog is in jumping state and it starts reaching max height then it will start falling
        if (anim.GetBool("isJumping") && (rb.velocity.y < .1f))
        {
            anim.SetBool("isFalling", true);
            anim.SetBool("isJumping", false);
        }

        // If the frog is in the falling state it will stop when it lands the ground
        else if (anim.GetBool("isFalling") && (coll.IsTouchingLayers(ground)))
        {
            anim.SetBool("isFalling", false);
        }
    }

    private void Move()
    {
        if (moveLeft)
        {
            // If we didn't reach the limit
            if (transform.position.x > leftLimit)
            {
                // if we touch the ground we jump
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("isJumping", true);
                }
                // turn face on the left
                if (transform.localScale.x == -1)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
            else
            {
                // change direction
                moveLeft = false;
            }
        }
        else
        {
            // If we didn't reach the limit
            if (transform.position.x < rightLimit)
            {
                // if we touch the ground we jump
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("isJumping", true);
                }
                // turn face on the right
                if (transform.localScale.x == 1)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }
            else
            {
                // change direction
                moveLeft = true;
            }
        }
    }

    // Sound effects utils
    private void Croak()
    {
        croak.Play();
    }

    private void Explode()
    {
        explosion.Play();
    }
}
