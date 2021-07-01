using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterController : MonoBehaviour
{
    public GameObject targetFeet;

    private Vector3 orgPos;
    private float offsetTime;
    void Start()
    {
        orgPos = targetFeet.transform.localPosition;
        offsetTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        var prob = Random.Range(0, 15);
        transform.Translate(transform.forward * 1f * 0.01f);
        if (Time.time - offsetTime > 2f)
        {
            transform.Rotate(Vector3.up * prob);
            offsetTime = Time.time;
        }
        //transform.Rotate(transform.InverseTransformVector(new Vector3(0, h * 110f * 0.01f, 0)));

        targetFeet.transform.localPosition = Mathf.Sin(Time.time) * 2f * transform.forward + orgPos;
    }
}
