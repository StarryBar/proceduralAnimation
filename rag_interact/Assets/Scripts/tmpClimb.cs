using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmpClimb : MonoBehaviour
{
    [SerializeField] private GameObject entity;
    [SerializeField] private float speed = 2500f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 10) return;
        Debug.Log("handHit");

        entity.GetComponent<Rigidbody>().AddForce((new Vector3(0, 1, 0) - entity.transform.forward.normalized) * this.speed);
    }
}
