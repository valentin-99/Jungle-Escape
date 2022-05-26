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
    private PhotonView view;

    private enum State { idle, run, jump, fallJump }
    private State state = State.idle;

    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource bounce;

    private GameObject canvas;
    private Text scoreCounter;
    private Text timerElapsed;

    private GameObject spawnPlayer;

    private int score;

    // lock control of jumping
    private bool jumpEnabled = false;

    private float timeRemaining = 240;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        audioListener = GetComponent<AudioListener>();

        // find canvas and the texts
        canvas = GameObject.Find("PlayerUI");
        Transform scoreCounterTransform = canvas.transform.Find("ScoreCounter");
        Transform timerElapsedTransform = canvas.transform.Find("TimerElapsed");
        scoreCounter = scoreCounterTransform.GetComponent<Text>();
        timerElapsed = timerElapsedTransform.GetComponent<Text>();

        // get spawn player script
        spawnPlayer = GameObject.Find("SpawnPlayer");

        if (view.IsMine)
        {
            audioListener.enabled = true;

            // Follow player
            GameObject vcam = GameObject.Find("CM vcam1");
            CinemachineVirtualCamera cvcam;
            cvcam = vcam.GetComponent<CinemachineVirtualCamera>();
            cvcam.Follow = this.gameObject.transform;

            // initialize score
            score = 0;
            scoreCounter.text = "0";
        }

        // initialize time left
        timerElapsed.text = timeRemaining.ToString();
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
                // disconnect from server and load scene
                PhotonNetwork.Disconnect();
                SceneManager.LoadScene("LevelSelection");
            }

            // update score text
            scoreCounter.text = score.ToString();
        }

        // same time left for all players
        HandleTimer();
    }

    private void HandleTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerElapsed.text = ((int)timeRemaining).ToString();
        }
        else
        {
            SceneManager.LoadScene("LoseLevel");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool destroyObject = false;

        if (view.IsMine)
        {
            // Collision between player and cherries
            if (collision.CompareTag("Collectable"))
            {
                destroyObject = true;
                score++;
            }

            if (collision.gameObject.name == "FallingZone")
            {
                SpawnPlayer sp = spawnPlayer.GetComponent<SpawnPlayer>();
                Vector2 spawnPos = sp.Spawn();
                transform.position = spawnPos;
            }
        }

        // destroy object for everyone
        if (destroyObject)
        {
            Destroy(collision.gameObject);
            destroyObject = false;
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
