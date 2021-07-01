using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CreateBall : NetworkBehaviour
{
    public float mass = 0.1f;
    public float drag = 0.05f;
    public float angularDrag = 0.05f;
    public GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnBall()
    {
        GameObject b = Instantiate(ball);
        b.AddComponent<Rigidbody>();
        b.GetComponent<Rigidbody>().mass = mass;
        b.GetComponent<Rigidbody>().drag = drag;
        b.GetComponent<Rigidbody>().useGravity = true;
        b.GetComponent<Rigidbody>().angularDrag = angularDrag;
        NetworkServer.Spawn(b);
    }
}
