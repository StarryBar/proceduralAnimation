using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footSensor : MonoBehaviour
{
    [SerializeField] private GameObject hip;
    [SerializeField] private float speed = 0.3f;

    private bool isWalking = false;

    void Start()
    {
        isWalking = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isWalking)
        {
            Debug.Log("inSider fool sensor hitter");
            //var dir = hip.transform.forward + new Vector3(0f, 0.3f, 0f);
            //hip.GetComponent<Rigidbody>().AddForce(dir * speed);
            hip.transform.Translate(hip.transform.forward * this.speed * Time.deltaTime);
        }
        
    }
}
