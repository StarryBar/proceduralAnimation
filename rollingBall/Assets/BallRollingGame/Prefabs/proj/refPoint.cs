using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Mirror;

[RequireComponent(typeof(ARAnchorManager))]
public class refPoint : NetworkBehaviour
{
    public Camera mCamera;
    public GameObject spawnPrefab;
    public GameObject spawnedObject = null;
    private ARAnchorManager mARAnchorManager;
    private float distance = 0.1f;
    void Start()
    {
        mARAnchorManager = transform.GetComponent<ARAnchorManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.touchCount == 0) return;
            var touch = Input.GetTouch(0);
            spawnedObject = GameObject.Find("Cube(Clone)");
            mCamera.transform.position = spawnedObject.transform.position;
            mCamera.transform.rotation = spawnedObject.transform.rotation;
        }

    }
}
