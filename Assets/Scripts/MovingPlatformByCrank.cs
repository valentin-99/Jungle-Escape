using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformByCrank : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        // retine game objectul MovingPlatformCrank
        GameObject MPC = GameObject.Find("MovingPlatformCrank");
        // retine PlatformGrid-ul si ii dezactiveaza componenta script MovingPlatform
        GameObject child = MPC.transform.GetChild(0).gameObject;
        child.GetComponent<MovingPlatform>().enabled = false;

        Debug.Log("HELLOOOO\n");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        GameObject MPC = GameObject.Find("MovingPlatformCrank");
        // retine PlatformGrid-ul si ii dezactiveaza componenta script MovingPlatform
        GameObject child = MPC.transform.GetChild(0).gameObject;

        if (col.gameObject.CompareTag("Crank")) {
            Debug.Log("HELLOOOO\n");
        }

        if (col.gameObject.CompareTag("Crank") && Input.GetKey(KeyCode.Space))
        {
            child.GetComponent<MovingPlatform>().enabled = true;
        }
    }
}
