using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankHCController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite newSprite;
    [SerializeField] private List<GameObject> collectables = new List<GameObject>();
    //[SerializeField] public GameObject go;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // doesn't work at start if the script is hidden
    private void Awake()
    {
        foreach (GameObject collectable in collectables)
        {
            collectable.SetActive(false);
        }
        //go.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Cand manivela este trasa apare platforma ascunsa si schimbam
        // sprite-ul manivelei.
        if (collision.CompareTag("Player") && Input.GetKey(KeyCode.Space))
        {
            foreach (GameObject collectable in collectables)
            {
                collectable.SetActive(true);
            }
            //go.SetActive(true);
            spriteRenderer.sprite = newSprite;
        }
    }
}
