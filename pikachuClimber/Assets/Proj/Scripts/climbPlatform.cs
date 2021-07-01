using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class climbPlatform : MonoBehaviour
{

    [SerializeField] private Transform targetLimb;

    Vector3 offsetPos;

    private void Start()
    {
        offsetPos = this.transform.localPosition;
    }

    private void Update()
    {
        //this.transform.rotation = targetLimb.rotation;
        Debug.Log("targetLimb:" + targetLimb.position);
        
        this.transform.position = offsetPos + targetLimb.position;
        Debug.Log("offset:" + offsetPos);
    }


}
