using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{
    private enum Segment { onLadder, onLadderTop, onLadderBottom };
    [SerializeField] Segment part = Segment.onLadder;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            switch (part)
            {
                case Segment.onLadder:
                    player.isClimbing = true;
                    player.ladder = this;
                    break;
                case Segment.onLadderTop:
                    player.onLadderTop = true;
                    break;
                case Segment.onLadderBottom:
                    player.onLadderBottom = true;
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            switch (part)
            {
                case Segment.onLadder:
                    player.isClimbing = false;
                    break;
                case Segment.onLadderTop:
                    player.onLadderTop = false;
                    break;
                case Segment.onLadderBottom:
                    player.onLadderBottom = false;
                    break;
                default:
                    break;
            }
        }
    }
}
