using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    private enum State { idle, run, jump, fallJump, hurt, climb }
    private State state = State.idle;

    [HideInInspector] public bool isClimbing = false;
    [HideInInspector] public bool onLadderTop = false;
    [HideInInspector] public bool onLadderBottom = false;
    public LadderController ladder;
    private float gravity;

    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask groundMarginsDown;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float hurtForce;
    [SerializeField] private Text scoreCounter;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource bounce;
    [SerializeField] private AudioSource collect;
    [SerializeField] private AudioSource boink;

    private int cherries;

    // lock control of jumping
    private bool jumpEnabled = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        speed = 6f;
        jumpForce = 14f;
        hurtForce = 6f;
        cherries = 0;
        scoreCounter.text = "0";
        gravity = rb.gravityScale;
    }

    private void Update()
    {
        if (state == State.climb)
        {
            Climb();
        }
        else if (state != State.hurt)
        {
            Move();
        }

        AnimationSwitch();
        // Sets the animation state
        anim.SetInteger("state", (int)state);

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("LevelSelection");
        }
    }

    // Collision for collectable items
    private void OnTriggerEnter2D(Collider2D col)
    {
        // Collision between player and cherries
        if (col.CompareTag("Collectable"))
        {
            Destroy(col.gameObject);
            cherries++;
            scoreCounter.text = cherries.ToString();
            Collect();
        }
    }

    // Collision for enemies
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            enemy.ChangeEnemyBodyStatic();

            if (state == State.fallJump)
            {
                //enemy.ChangeEnemyBodyStatic();
                enemy.UnableEnemyCollider();
                enemy.EnemyDeath();
                Jump();
            }

            else
            {
                HurtBoink();
                state = State.hurt;

                // Collided object is in front of the player
                if (col.gameObject.transform.position.x > transform.position.x)
                {
                    // move player back
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                    enemy.ChangeEnemyBodyDynamic();
                }
                // Collided object is behind the player
                else
                {
                    // move player in front
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                    enemy.ChangeEnemyBodyDynamic();
                }
            }
        }
    }

    // Decide movement
    private void Move()
    {
        // if I jump when I am standing on the ladder anim's speed is 1
        anim.speed = 1;

        // Left
        if (Input.GetAxis("Horizontal") < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        // Right
        else if (Input.GetAxis("Horizontal") > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        // Jump
        //if (Input.GetKey(KeyCode.Space) && coll.IsTouchingLayers(ground)) // working
        //if ((Input.GetAxis("Vertical") > 0) && coll.IsTouchingLayers(ground)) // working v2.0
        if ((Input.GetAxisRaw("Vertical") > 0) && coll.IsTouchingLayers(ground))
        {
            // verifiy lock control and cancel if locked
            if (!jumpEnabled) return;
            // lock input
            jumpEnabled = false;

            Jump();
        }
        // treath input axis released
        else if ((Input.GetAxisRaw("Vertical") <= 0) && coll.IsTouchingLayers(ground))
        {
            jumpEnabled = true;
        }

        // Climb
        //if (isClimbing && Mathf.Abs(Input.GetAxis("Vertical")) > 0) // working
        if (isClimbing && (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)))
        {
            state = State.climb;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX |
                RigidbodyConstraints2D.FreezeRotation;
            transform.position = new Vector3(ladder.transform.position.x, rb.position.y);
            rb.gravityScale = 0f;
            rb.drag = 5;
        }
    }

    // Jump util function
    private void Jump()
    {
        Bounce();
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jump;
    }

    private void Climb()
    {
        //if (Input.GetKey(KeyCode.Space)) // working
        //if (Input.GetAxis("Vertical") > 0) // working v2.0
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            // verifiy lock control and cancel if locked
            if (!jumpEnabled) return;
            // lock input
            jumpEnabled = false;

            Jump();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            isClimbing = false;
            rb.gravityScale = gravity;
            rb.drag = 0;
            anim.speed = 1;
        }
        // treath input axis released
        else if (Input.GetAxisRaw("Vertical") <= 0) {
            jumpEnabled = true;
        }

        // up
        //if (Input.GetAxis("Vertical") > 0 && !onLadderTop) //working
        if (Input.GetKey(KeyCode.Q) && !onLadderTop)
        {
            //rb.velocity = new Vector2(0f, Input.GetAxis("Vertical") * speed); //working
            rb.velocity = new Vector2(0f, speed); //working
            anim.speed = 1;
        }

        // down
        //else if (Input.GetAxis("Vertical") < 0 && !onLadderBottom) // working
        else if (Input.GetKey(KeyCode.E) && !onLadderBottom)
        {
            //rb.velocity = new Vector2(0f, Input.GetAxis("Vertical") * speed); //working
            rb.velocity = new Vector2(0f, -speed);
            anim.speed = 1;
        }

        // doesn't climb, but it's on ladder
        else
        {
            anim.speed = 0;
        }
    }

    // Decide animation
    private void AnimationSwitch()
    {
        if (state == State.climb)
        {

        }

        // If player jumps he starts falling to the ground
        else if ((state == State.jump) || (coll.IsTouchingLayers(groundMarginsDown)))
        {
            if (rb.velocity.y < .1f)
            {
                state = State.fallJump;
            }
        }

        // If player is falling to the ground and he's touching it then switch to idle
        else if (state == State.fallJump)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }

        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }

        // If player is walking
        else if (Mathf.Abs(rb.velocity.x) > 3)
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

    private void Bounce()
    {
        if (!bounce.isPlaying)
        {
            bounce.Play();
        }
    }

    private void Collect()
    {
        collect.Play();
    }

    private void HurtBoink()
    {
        if (!boink.isPlaying)
        {
            boink.Play();
        }
    }
}
