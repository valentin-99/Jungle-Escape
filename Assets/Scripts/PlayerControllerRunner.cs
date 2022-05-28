using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControllerRunner : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    private enum State { idle, run, jump, fallJump, hurt}
    private State state = State.idle;

    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float hurtForce;
    [SerializeField] private Text cherriesCounter;
    [SerializeField] private Text metersCounter;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource bounce;
    [SerializeField] private AudioSource collect;
    [SerializeField] private AudioSource boink;
    [SerializeField] private Transform cameraLimit;
    [SerializeField] private GameObject runnerUI;
    [SerializeField] private GameObject summaryCanvas;

    private int cherries;
    private int meters;
    private bool startGame = false;
    private bool hasFinished = false;

    [HideInInspector] public int score1, score2, score3;
    [HideInInspector] public int record1, record2, record3;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        cherries = 0;
        cherriesCounter.text = "0";
        metersCounter.text = "0";
    }

    private void Update()
    {
        if (state != State.hurt && !hasFinished)
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

        // Update meters and speed
        ManageMeters();

        // update camera limit
        cameraLimit.transform.position = new Vector3(gameObject.transform.position.x, 0);
    }

    private void ManageMeters()
    {
        float currentX = gameObject.transform.position.x;
        meters = (int) (currentX + 8.5f);
        metersCounter.text = meters.ToString();

        if (meters >= 250 && meters < 500)
        {
            speed = 8;
        }
        else if (meters >= 500 && meters < 1000)
        {
            speed = 10;
        }
        else if (meters >= 1000)
        {
            speed = 12;
        }
    }

    private void GetSetHighScore(Text bestScore)
    {
        // Get current scene
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        int bestScoreData = 0;
        // if file doesn't exist
        if (SaveLoadSystem.LoadRunnerData() == null)
        {
            bestScore.text = "0";
        }
        // if file exist
        else
        {
            // load the data and assign the best score
            RunnerData data = SaveLoadSystem.LoadRunnerData();
            switch (sceneName)
            {
                case "RunnerDay":
                    bestScoreData = data.record1;
                    break;
                case "RunnerSunset":
                    bestScoreData = data.record2;
                    break;
                case "RunnerNight":
                    bestScoreData = data.record3;
                    break;
                default:
                    break;
            }
        }

        // if current score is higher, update best score and the other datas
        if (meters > bestScoreData)
        {
            // if the file doesn't exist we will create one
            if (SaveLoadSystem.LoadRunnerData() == null)
            {
                SaveLoadSystem.SaveRunnerData(this);
            }
            RunnerData data = SaveLoadSystem.LoadRunnerData();
            switch (sceneName)
            {
                case "RunnerDay":
                    score1 = cherries;
                    score2 = data.score2;
                    score3 = data.score3;
                    record1 = meters;
                    record2 = data.record2;
                    record3 = data.record3;
                    break;
                case "RunnerSunset":
                    score1 = data.score1;
                    score2 = cherries;
                    score3 = data.score3;
                    record1 = data.record1;
                    record2 = meters;
                    record3 = data.record3;
                    break;
                case "RunnerNight":
                    score1 = data.score1;
                    score2 = data.score2;
                    score3 = cherries;
                    record1 = data.record1;
                    record2 = data.record2;
                    record3 = meters;
                    break;
                default:
                    break;
            }

            SaveLoadSystem.SaveRunnerData(this);
            bestScore.text = meters.ToString();
        }
        else
        {
            bestScore.text = bestScoreData.ToString();
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
            cherriesCounter.text = cherries.ToString();
            Collect();
        }

        if (col.gameObject.name == "FallingZone(Clone)")
        {
            hasFinished = true;
            rb.bodyType = RigidbodyType2D.Static;
            anim.enabled = false;

            runnerUI.SetActive(false);
            summaryCanvas.SetActive(true);
            Transform cherriesCounterTransform = summaryCanvas.transform.Find("CherriesCounter");
            Transform metersCounterTransform = summaryCanvas.transform.Find("MetersCounter");
            Transform recordTransform = summaryCanvas.transform.Find("LongestDistanceCounter");
            Text cherriesText = cherriesCounterTransform.GetComponent<Text>();
            Text metersText = metersCounterTransform.GetComponent<Text>();
            Text recordText = recordTransform.GetComponent<Text>();

            cherriesText.text = cherries.ToString();
            metersText.text = metersCounter.text;
            // TOOD get persistent data
            GetSetHighScore(recordText);
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

    private void Move()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            startGame = true;
        }

        // Run
        if (startGame)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }

        // Jump continuous
        if ((Input.GetAxis("Vertical") > 0) && coll.IsTouchingLayers(ground) && startGame)
        {
            Jump();
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
