using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerIso : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    private enum State { idle, run, jump, fallJump }
    private State state = State.idle;

    //[SerializeField] private LayerMask ground;
    [SerializeField] private float speed;
    [SerializeField] private AudioSource footstep;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Move();

        AnimationSwitch();
        // Sets the animation state
        anim.SetInteger("state", (int)state);
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal") * speed;
        float y= Input.GetAxisRaw("Vertical") * speed;
        rb.velocity = new Vector2(x, y);

        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector2(.2f, .2f);
        }

        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector2(-.2f, .2f);
        }
    }

    // Decide animation
    private void AnimationSwitch()
    {
        // If player is walking
        if (Mathf.Abs(rb.velocity.x) > 0 || Mathf.Abs(rb.velocity.y) > 0)
        {
            state = State.run;
        }

        else
        {
            state = State.idle;
        }
    }

    // Sound effects utils
    private void Footstep()
    {
        footstep.Play();
    }
}
