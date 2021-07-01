using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class sync : NetworkBehaviour
{
    [SyncVar(hook = nameof(SyncToPos))]
    private Vector3 pos;

    [SyncVar(hook = nameof(SyncToRot))]
    private Quaternion rot;

    private float startTime;
    private void Awake()
    {
        pos = this.transform.position;
        rot = this.transform.rotation;
    }
    private void SyncToPos(Vector3 oldPos, Vector3 newPos)
    {
        Debug.Log("isServer?" + isServer + " ,isClient? " + isClient + " ,transform:" + transform.position);
        transform.position = newPos;
    }

    private void SyncToRot(Quaternion oldRot, Quaternion newRot)
    {
        transform.rotation = newRot;
    }


    // Start is called before the first frame update

    [Command]
    void CmdSync(Transform TTf)
    {
        pos = TTf.position;
        rot = TTf.rotation;
    }


    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            startTime = Time.time;
            //RpcSync(transform);
            pos = transform.position;
            rot = transform.rotation;
            
        }
        
        
    }

    [ClientRpc]
    private void RpcSync(Transform TTf)
    {
        pos = TTf.position;
        rot = TTf.rotation;
    }
}
