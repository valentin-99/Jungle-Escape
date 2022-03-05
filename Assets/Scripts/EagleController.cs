using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleController : Enemy
{
    [SerializeField] private float leftLimit;
    [SerializeField] private float rightLimit;
    [SerializeField] private float speedHorizontal;
    [SerializeField] private AudioSource screech;
    [SerializeField] private AudioSource explosion;

    private bool moveLeft;

    protected override void Start()
    {
        base.Start();

        leftLimit = transform.position.x - 3f;
        rightLimit = transform.position.x + 3f;
        speedHorizontal = 2f;
        moveLeft = true;
    }


    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (moveLeft)
        {
            // If we didn't reach the limit (+ warning fix)
            if ((transform.position.x > leftLimit) && (rb.bodyType == RigidbodyType2D.Dynamic))
            {
                rb.velocity = new Vector2(-speedHorizontal, 0);
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
                Screech();
            }
        }

        else
        {
            // If we didn't reach the limit (+ warning fix)
            if ((transform.position.x < rightLimit) && (rb.bodyType == RigidbodyType2D.Dynamic))
            {
                rb.velocity = new Vector2(speedHorizontal, 0);
                // turn face on the left
                if (transform.localScale.x == 1)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }
            else
            {
                // change direction
                moveLeft = true;
                Screech();
            }
        }
    }

    // Sound effects utils
    private void Screech()
    {
        screech.Play();
    }

    private void Explode()
    {
        explosion.Play();
    }
}
