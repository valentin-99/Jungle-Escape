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
    [SerializeField] private Text timerElapsed;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource bounce;
    [SerializeField] private AudioSource collect;
    [SerializeField] private AudioSource boink;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;

    // lock control of jumping
    private bool jumpEnabled = false;

    private int score;
    private int totalScore;
    private float timeRemaining = 150;

    private bool hasFinished = false;

    [HideInInspector] public int score1, score2, score3, score4, score5, score6;
    [HideInInspector] public int time1, time2, time3, time4, time5, time6;
    [HideInInspector] public int record1, record2, record3, record4, record5, record6;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        score = 0;
        scoreCounter.text = "0";
        timerElapsed.text = timeRemaining.ToString();
        gravity = rb.gravityScale;
    }

    private void Update()
    {
        if (state == State.climb)
        {
            Climb();
        }
        else if (state != State.hurt && !hasFinished)
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

        // update score text
        scoreCounter.text = score.ToString();

        HandleTimer();
    }

    private void HandleTimer()
    {
        if (timeRemaining > 0 && !hasFinished)
        {
            timeRemaining -= Time.deltaTime;
            timerElapsed.text = ((int) timeRemaining).ToString();
        }
        else if (timeRemaining <= 0)
        {
            hasFinished = true;
            rb.bodyType = RigidbodyType2D.Static;
            anim.enabled = false;

            playerUI.SetActive(false);
            loseCanvas.SetActive(true);
            // TODO make player rb static.
            Transform scoreCounterTransform = loseCanvas.transform.Find("ScoreCounter");
            Transform timerElapsedTransform = loseCanvas.transform.Find("TimerElapsed");
            Transform bestScoreCounterTransform = loseCanvas.transform.Find("BestScoreCounter");
            Text scoreLose = scoreCounterTransform.GetComponent<Text>();
            Text timerLose = timerElapsedTransform.GetComponent<Text>();
            Text bestScoreLose = bestScoreCounterTransform.GetComponent<Text>();

            scoreLose.text = score.ToString();
            timerLose.text = ((int)timeRemaining).ToString();

            GetHighScoreLose(bestScoreLose);
        }
    }

    private void GetHighScoreLose(Text bestScore)
    {
        // if the file doesn't exist
        if (SaveLoadSystem.LoadSoloData() == null)
        {
            bestScore.text = "0";
        }
        else
        {
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = currentScene.name;
            SoloData data = SaveLoadSystem.LoadSoloData();
            int best = 0;

            switch (sceneName)
            {
                case "Level1":
                    best = data.record1;
                    break;
                case "Level2":
                    best = data.record2;
                    break;
                case "Level3":
                    best = data.record3;
                    break;
                case "Level4":
                    best = data.record4;
                    break;
                case "Level5":
                    best = data.record5;
                    break;
                case "Level6":
                    best = data.record6;
                    break;
                default:
                    break;
            }

            bestScore.text = best.ToString();
        }
    }
    private void GetSetHighScoreWin(Text bestScore, int total)
    {
        // Get current scene
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        int bestScoreData = 0;
        // if file doesn't exist
        if (SaveLoadSystem.LoadSoloData() == null)
        {
            bestScore.text = "0";
        }
        // if file exist
        else
        {
            // load the data and assign the best score
            SoloData data = SaveLoadSystem.LoadSoloData();
            switch (sceneName)
            {
                case "Level1":
                    bestScoreData = data.record1;
                    break;
                case "Level2":
                    bestScoreData = data.record2;
                    break;
                case "Level3":
                    bestScoreData = data.record3;
                    break;
                case "Level4":
                    bestScoreData = data.record4;
                    break;
                case "Level5":
                    bestScoreData = data.record5;
                    break;
                case "Level6":
                    bestScoreData = data.record6;
                    break;
                default:
                    break;
            }
        }

        // if current score is higher, update best score and the other datas
        if (total > bestScoreData)
        {
            SoloData data = SaveLoadSystem.LoadSoloData();
            switch (sceneName)
            {
                case "Level1":
                    score1 = score;
                    score2 = data.score2;
                    score3 = data.score3;
                    score4 = data.score4;
                    score5 = data.score5;
                    score6 = data.score6;
                    time1 = (int)timeRemaining;
                    time2 = data.time2;
                    time3 = data.time3;
                    time4 = data.time4;
                    time5 = data.time5;
                    time6 = data.time6;
                    record1 = total;
                    record2 = data.record2;
                    record3 = data.record3;
                    record4 = data.record4;
                    record5 = data.record5;
                    record6 = data.record6;
                    break;
                case "Level2":
                    score1 = data.score1;
                    score2 = score;
                    score3 = data.score3;
                    score4 = data.score4;
                    score5 = data.score5;
                    score6 = data.score6;
                    time1 = data.time1;
                    time2 = (int)timeRemaining;
                    time3 = data.time3;
                    time4 = data.time4;
                    time5 = data.time5;
                    time6 = data.time6;
                    record1 = data.record1;
                    record2 = total;
                    record3 = data.record3;
                    record4 = data.record4;
                    record5 = data.record5;
                    record6 = data.record6;
                    break;
                case "Level3":
                    score1 = data.score1;
                    score2 = data.score2;
                    score3 = score;
                    score4 = data.score4;
                    score5 = data.score5;
                    score6 = data.score6;
                    time1 = data.time1;
                    time2 = data.time2;
                    time3 = (int)timeRemaining;
                    time4 = data.time4;
                    time5 = data.time5;
                    time6 = data.time6;
                    record1 = data.record1;
                    record2 = data.record2;
                    record3 = total;
                    record4 = data.record4;
                    record5 = data.record5;
                    record6 = data.record6;
                    break;
                case "Level4":
                    score1 = data.score1;
                    score2 = data.score2;
                    score3 = data.score3;
                    score4 = score;
                    score5 = data.score5;
                    score6 = data.score6;
                    time1 = data.time1;
                    time2 = data.time2;
                    time3 = data.time3;
                    time4 = (int)timeRemaining;
                    time5 = data.time5;
                    time6 = data.time6;
                    record1 = data.record1;
                    record2 = data.record2;
                    record3 = data.record3;
                    record4 = total;
                    record5 = data.record5;
                    record6 = data.record6;
                    break;
                case "Level5":
                    score1 = data.score1;
                    score2 = data.score2;
                    score3 = data.score3;
                    score4 = data.score4;
                    score5 = score;
                    score6 = data.score6;
                    time1 = data.time1;
                    time2 = data.time2;
                    time3 = data.time3;
                    time4 = data.time4;
                    time5 = (int)timeRemaining;
                    time6 = data.time6;
                    record1 = data.record1;
                    record2 = data.record2;
                    record3 = data.record3;
                    record4 = data.record4;
                    record5 = total;
                    record6 = data.record6;
                    break;
                case "Level6":
                    score1 = data.score1;
                    score2 = data.score2;
                    score3 = data.score3;
                    score4 = data.score4;
                    score5 = data.score5;
                    score6 = score;
                    time1 = data.time1;
                    time2 = data.time2;
                    time3 = data.time3;
                    time4 = data.time4;
                    time5 = data.time5;
                    time6 = (int)timeRemaining;
                    record1 = data.record1;
                    record2 = data.record2;
                    record3 = data.record3;
                    record4 = data.record4;
                    record5 = data.record5;
                    record6 = total;
                    break;
                default:
                    break;
            }

            SaveLoadSystem.SaveSoloData(this);
            bestScore.text = total.ToString();
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
            score += 1000;
            Collect();
        }

        if (col.gameObject.name == "FinishFlag")
        {
            hasFinished = true;
            rb.bodyType = RigidbodyType2D.Static;
            anim.enabled = false;

            playerUI.SetActive(false);
            winCanvas.SetActive(true);
            // TODO make player rb static.
            Transform scoreCounterTransform = winCanvas.transform.Find("ScoreCounter");
            Transform timerElapsedTransform = winCanvas.transform.Find("TimerElapsed");
            Transform totalCounterTransform = winCanvas.transform.Find("TotalCounter");
            Transform bestScoreCounterTransform = winCanvas.transform.Find("BestScoreCounter");
            Text scoreWin = scoreCounterTransform.GetComponent<Text>();
            Text timerWin = timerElapsedTransform.GetComponent<Text>();
            Text totalWin = totalCounterTransform.GetComponent<Text>();
            Text bestScoreWin = bestScoreCounterTransform.GetComponent<Text>();

            scoreWin.text = score.ToString();
            timerWin.text = ((int)timeRemaining).ToString();
            int total = score * ((int)timeRemaining);
            totalWin.text = total.ToString();

            GetSetHighScoreWin(bestScoreWin, total);
        }


        if (col.gameObject.name == "FallingZone")
        {
            hasFinished = true;
            rb.bodyType = RigidbodyType2D.Static;
            anim.enabled = false;

            playerUI.SetActive(false);
            loseCanvas.SetActive(true);
            Transform scoreCounterTransform = loseCanvas.transform.Find("ScoreCounter");
            Transform timerElapsedTransform = loseCanvas.transform.Find("TimerElapsed");
            Transform bestScoreCounterTransform = loseCanvas.transform.Find("BestScoreCounter");
            Text scoreLose = scoreCounterTransform.GetComponent<Text>();
            Text timerLose = timerElapsedTransform.GetComponent<Text>();
            Text bestScoreLose = bestScoreCounterTransform.GetComponent<Text>();

            scoreLose.text = score.ToString();
            timerLose.text = ((int)timeRemaining).ToString();

            // TOOD get persistent data
            GetHighScoreLose(bestScoreLose);
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

                if (string.Equals(enemy.GetType().Name, "FrogController"))
                {
                    score += 5000;
                }
                else if (string.Equals(enemy.GetType().Name, "EagleController"))
                {
                    score += 10000;
                }
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
