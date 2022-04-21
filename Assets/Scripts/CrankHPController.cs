using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankHPController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite newSprite;
    [SerializeField] private GameObject grid;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        grid.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Cand manivela este trasa apare platforma ascunsa si schimbam
        // sprite-ul manivelei.
        if (collision.CompareTag("Player") && Input.GetKey(KeyCode.C))
        {
            grid.SetActive(true);
            spriteRenderer.sprite = newSprite;
        }
    }
}
