using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private float curTime;
    private bool startTimer = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(transform.forward * v * 0.01f);
        transform.Rotate(transform.InverseTransformVector(new Vector3(0, h * 110f * 0.01f, 0)));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            curTime = Time.time;
            startTimer = true;
            
        }
        if (startTimer && Time.time - curTime < 3f)
        {
            transform.Translate(transform.InverseTransformVector(Vector3.up * 0.05f));
        } else
        {
            startTimer = false;
        }
    }
}
