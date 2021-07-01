using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class syncCmd : NetworkBehaviour
{
    public Transform arCamera;
    [SyncVar(hook = nameof(SyncToTrans))]
    private Transform tf;

    private void SyncToTrans(Transform oldTf, Transform newTf)
    {
        Debug.Log("isServer?" + isServer + " ,isClient? " + isClient + " ,transform:" + transform.position);
        arCamera.position = newTf.position;
        arCamera.rotation = newTf.rotation;
    }
    // Start is called before the first frame update

    [Command]
    void CmdSync(Transform TTf)
    {
        Debug.Log("incmd");
        tf = TTf;
    }


    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            Debug.Log("before");

            CmdSync(arCamera);
            Debug.Log("after");

        }


    }

    [ClientRpc]
    private void RpcSync(Transform TTf)
    {
        tf = TTf;
    }
}
