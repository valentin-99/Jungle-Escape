using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankMPController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite newSprite;
    [SerializeField] private GameObject grid;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        // Dezactivare script pentru platforma miscatoare
        grid.GetComponent<MovingPlatform>().enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Cand manivela este trasa activam platforma miscatoare si schimbam
        // sprite-ul manivelei.
        if (collision.CompareTag("Player") && Input.GetKey(KeyCode.Space))
        {
            grid.GetComponent<MovingPlatform>().enabled = true;
            spriteRenderer.sprite = newSprite;
        }
    }
}
