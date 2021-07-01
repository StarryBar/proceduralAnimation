using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sightSensor : MonoBehaviour
{
    [SerializeField] private GameObject hip;
    [SerializeField] private float speed = 700f;


    private void OnTriggerStay(Collider other)
    {
        float prob = Random.Range(0f, 1f);
        if (prob > 1.95)
        {
            hip.GetComponent<Rigidbody>().AddForce(Vector3.up * speed);
        }

    }
}
