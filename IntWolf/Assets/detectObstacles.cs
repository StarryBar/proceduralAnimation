using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectObstacles : MonoBehaviour
{
    public Rigidbody hip;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 9) return;
        Debug.Log("hit1!!!!!!!");
        this.hip.AddForce(new Vector3(0, 1600f, 0));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
