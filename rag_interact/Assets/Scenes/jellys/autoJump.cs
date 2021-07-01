using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoJump : MonoBehaviour
{
    private float curTime;
    private bool shouldJump;
    private float jumpDuration;
    // Start is called before the first frame update
    void Start()
    {
        shouldJump = Random.Range(0f, 1f) > 0.5 ? true : false;
        jumpDuration = Random.Range(0f, 3f);
        curTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
    }

    void Jump()
    {
        
        if (Time.time - curTime > jumpDuration)
        {
            shouldJump = !shouldJump;
            curTime = Time.time;
        }


        if (shouldJump)
        {
            transform.Translate(transform.InverseTransformVector(Vector3.up * 0.04f));
        }
    }
}
