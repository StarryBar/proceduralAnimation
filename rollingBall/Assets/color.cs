using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class color : NetworkBehaviour
{
    [SerializeField]
    private Material matPrefab;
    [SerializeField]
    private Material mat0;

    [SerializeField]
    private GameObject go;

    Material[] TSetArray;
    Material[] rawArray;

    [SyncVar(hook = nameof(SyncToCol))]
    private bool isHit = false;

    [ClientRpc]
    private void RpcSync(bool hasHitCol)
    {
        if (hasHitCol)
        {
            Debug.Log("?WithinSync: " + hasHitCol + ",isServer?" + isServer + ",isClient?" + isClient);
            go.GetComponent<MeshRenderer>().materials = TSetArray;
        }
        else
        {
            Debug.Log("?WithinSync: " + hasHitCol + ",isLocalPlayer?" + isLocalPlayer);
            go.GetComponent<MeshRenderer>().materials = rawArray;

        }
    }


    private void Start()
    {
        TSetArray = new Material[1];
        TSetArray[0] = matPrefab;

        rawArray = new Material[1];
        rawArray[0] = mat0;

    }
    private void SyncToCol(bool oldCol, bool newCol)
    {
        
        if (newCol)
        {
            Debug.Log("?WithinSync: " + newCol + ",isServer?" + isServer + ",isClient?" + isClient);
            go.GetComponent<MeshRenderer>().materials = TSetArray;
        }
        else
        {
            Debug.Log("?WithinSync: " + newCol + ",isLocalPlayer?" + isLocalPlayer);
            go.GetComponent<MeshRenderer>().materials = rawArray;

        }
    }

    // Start is called before the first frame update
    void OnCollisionEnter(Collision collision)
    {
        if (isServer)
        {
            if ("leftHand".Equals(collision.gameObject.name) || "rightHand".Equals(collision.gameObject.name))
            {
                go.GetComponent<MeshRenderer>().materials = TSetArray;
                isHit = true;

            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (isServer)
        {
            if ("leftHand".Equals(collision.gameObject.name) || "rightHand".Equals(collision.gameObject.name))
            {
                go.GetComponent<MeshRenderer>().materials = rawArray;
                isHit = false;

            }
        }
    }

}
