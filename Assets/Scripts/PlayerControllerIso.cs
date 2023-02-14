using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField] GameObject loading;
    [SerializeField] Slider loadingSlider;

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

    IEnumerator LoadSceneAsync(string scene)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);

        loading.SetActive(true);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            loadingSlider.value = progress;
            yield return null;
        }
    }

    // Level selection
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("solo1"))
        {
            //SceneManager.LoadScene("Level1");
            StartCoroutine(LoadSceneAsync("Level1"));
        }
        else if (col.CompareTag("solo2"))
        {
            //SceneManager.LoadScene("Level2");
            StartCoroutine(LoadSceneAsync("Level2"));
        }
        else if (col.CompareTag("solo3"))
        {
            //SceneManager.LoadScene("Level3");
            StartCoroutine(LoadSceneAsync("Level3"));
        }
        else if (col.CompareTag("solo4"))
        {
            //SceneManager.LoadScene("Level4");
            StartCoroutine(LoadSceneAsync("Level4"));
        }
        else if (col.CompareTag("solo5"))
        {
            //SceneManager.LoadScene("Level5");
            StartCoroutine(LoadSceneAsync("Level5"));
        }
        else if (col.CompareTag("solo6"))
        {
            //SceneManager.LoadScene("Level6");
            StartCoroutine(LoadSceneAsync("Level6"));
        }
        else if (col.CompareTag("runner_day"))
        {
            //SceneManager.LoadScene("RunnerDay");
            StartCoroutine(LoadSceneAsync("RunnerDay"));
        }
        else if (col.CompareTag("runner_sunset"))
        {
            //SceneManager.LoadScene("RunnerSunset");
            StartCoroutine(LoadSceneAsync("RunnerSunset"));
        }
        else if (col.CompareTag("runner_night"))
        {
            //SceneManager.LoadScene("RunnerNight");
            StartCoroutine(LoadSceneAsync("RunnerNight"));
        }
        else if (col.CompareTag("multiplayer"))
        {
            SceneManager.LoadScene("Loading");
        }
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal") * speed;
        float y = Input.GetAxisRaw("Vertical") * speed;
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
