using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Cinemachine;


public class PlayerControllerMulti : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    private AudioListener audioListener;

    private enum State { idle, run, jump, fallJump }
    private State state = State.idle;

    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource bounce;

    // lock control of jumping
    private bool jumpEnabled = false;

    PhotonView view;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        view = GetComponent<PhotonView>();
        audioListener = GetComponent<AudioListener>();

        if (view.IsMine)
        {
            audioListener.enabled = true;

            // Follow player
            GameObject vcam = GameObject.Find("CM vcam1");
            CinemachineVirtualCamera cvcam;
            cvcam = vcam.GetComponent<CinemachineVirtualCamera>();
            cvcam.Follow = this.gameObject.transform;
        }

        speed = 6f;
        jumpForce = 14f;
    }

    private void Update()
    {
        if (view.IsMine)
        {
            Move();

            AnimationSwitch();
            // Sets the animation state
            anim.SetInteger("state", (int)state);

            // Exit level
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("LevelSelection");
            }
        }
    }

    private void Move()
    {
        // Left
        if (Input.GetAxis("Horizontal") < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-.5f, .5f);
        }

        // Right
        else if (Input.GetAxis("Horizontal") > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(.5f, .5f);
        }

        // Jump
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
    }

    // Jump util function
    private void Jump()
    {
        Bounce();
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jump;
    }

    // Decide animation
    private void AnimationSwitch()
    {
        // If player jumps he starts falling to the ground
        if ((state == State.jump))
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
}
