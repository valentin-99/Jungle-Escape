using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pos1, pos2;
    public float speed;
    public Transform startPos;
    Vector3 nextPos;

    void Start()
    {
        nextPos = startPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        // daca platforma a ajuns la pos1, urmatoarul target va fi pos2 si invers.
        if (transform.position == pos1.position)
        {
            nextPos = pos2.position;
        }

        if (transform.position == pos2.position)
        {
            nextPos = pos1.position;
        }

        // MoveTowards ia pozitia curenta si se misca catre target (nextPos)
        // la viteza speed * Time.deltaTime
        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }
}
