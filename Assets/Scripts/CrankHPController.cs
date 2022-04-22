using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankHPController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite newSprite;
    [SerializeField] private List<GameObject> grids = new List<GameObject>();

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        //grid.SetActive(false);
        foreach (GameObject grid in grids)
        {
            grid.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Cand manivela este trasa apare platforma ascunsa si schimbam
        // sprite-ul manivelei.
        if (collision.CompareTag("Player") && Input.GetKey(KeyCode.Space))
        {
            //grid.SetActive(true);
            foreach (GameObject grid in grids)
            {
                grid.SetActive(true);
            }
            spriteRenderer.sprite = newSprite;
        }
    }
}
