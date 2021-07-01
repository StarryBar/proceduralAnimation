using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodySensor : MonoBehaviour
{
    public static bool isbodyHit;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit ground:" + other.gameObject.layer);
        if (other.gameObject.layer == 7)
        {
            isbodyHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("leave ground:" + other.gameObject.layer);
        if (other.gameObject.layer == 7)
        {
            isbodyHit = false;
        }
    }
}
