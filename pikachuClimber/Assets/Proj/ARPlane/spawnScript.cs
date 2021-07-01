using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class spawnScript : MonoBehaviour
{

    [SerializeField] private GameObject prefabBarrier;
    [SerializeField] private GameObject prefabTarget;
    [SerializeField] private GameObject prefabPikachu;
    [SerializeField] private GameObject prefabBall;
    [SerializeField] private GameObject prefabCapsule;
    [SerializeField] private GameObject prefabMan;
    [SerializeField] private Transform pos;

    public static bool isRestart = false;
    float timeOffset;
    // Start is called before the first frame update

    private void Start()
    {
        prefabPikachu.SetActive(false);
        //prefabBall.SetActive(false);
        prefabCapsule.SetActive(false);
        prefabMan.SetActive(false);
        timeOffset = Time.time;
    }

    public void spawnBarrier()
    {
        GameObject barrier = Instantiate(prefabBarrier, pos.position, pos.rotation);
    }

    public void spawnTarget()
    {
        GameObject target = Instantiate(prefabTarget, pos.position, pos.rotation);
        isRestart = true;
    }

    public void spawnPikachu()
    {
        //prefabBall.SetActive(true);
        prefabPikachu.SetActive(true);
        prefabCapsule.SetActive(true);
        //prefabBall.GetComponent<Rigidbody>().AddForce(Vector3.up * 0.1f);
    }

    public void spawnMan()
    {
        prefabMan.SetActive(true);
    }

    public void throwBall()
    {
        var swordEndPos = FreeClimb.masterObj.transform;//CharacterController.masterPos;
        Debug.Assert(CharacterController.masterPos != null, "has master Pos");
        GameObject ball = Instantiate(prefabBall, swordEndPos.position, swordEndPos.rotation);
        ball.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,10));
    }

    private void Update()
    {
        //if (Time.time - timeOffset > 3f)
        //{
            
        //}
    }

}
