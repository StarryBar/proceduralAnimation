using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{

    CharacterController cc;
    public Transform cameraTransform;
    public GameObject arCamera;
    float pitch = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            cc = GetComponent<CharacterController>();
        } else
        {
            cameraTransform.GetComponent<Camera>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            MovePlayer();
            //Look();
        }
        
    }

    void MovePlayer()
    {
        var movex = Input.GetAxis("Horizontal");
        var movey = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(movex, 0, movey);
        arCamera.transform.Translate(move * 0.01f);
        //move = Vector3.ClampMagnitude(move, 1f);
        //move = transform.TransformDirection(move);
        //cc.SimpleMove(move * 5f);

    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * 3f;
        transform.Rotate(0, mouseX, 0);
        pitch -= Input.GetAxis("Mouse Y") * 3f;
        pitch = Mathf.Clamp(pitch, -45f, 45f);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
