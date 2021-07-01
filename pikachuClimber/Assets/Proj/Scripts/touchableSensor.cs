using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class touchableSensor : MonoBehaviour
{

    [SerializeField] private Animator targetAnimator;
    [SerializeField] private GameObject hip;
    [SerializeField] private float climbingForce = 300f;
    [SerializeField] private GameObject climbHelper;

    private AnimatorStateController animatorStateController;
    private float timeOffset;

    private void Start()
    {
        animatorStateController = new AnimatorStateController(targetAnimator);
        climbHelper.SetActive(false);
        timeOffset = Time.time;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("touch");
        if (!CharacterController.agent.enabled)
        {
            climbHelper.transform.position = hip.transform.position;
            climbHelper.transform.rotation = hip.transform.rotation;
            climbHelper.SetActive(true);
            StopCoroutine(sleep(1f));
            animatorStateController.toClimbing();

        }


    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("touch inside");
        if (!CharacterController.agent.enabled)
        {
            climbHelper.SetActive(true);
            StopCoroutine(sleep(1f));
            animatorStateController.toClimbing();
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (animatorStateController.getState() == PlayerState.Climbing)
    //    {
    //        if (Time.time - timeOffset > 0.5f)
    //        {
    //            var dir = Vector3.up + 0.1f * hip.transform.forward;
    //            hip.GetComponent<Rigidbody>().AddForce(dir * this.climbingForce);
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (!CharacterController.agent.enabled)
    //    {
    //        animatorStateController.toIdle();
    //    }
    //}
    private void OnTriggerExit(Collider other)
    {
        if (!CharacterController.agent.enabled)
        {
            climbHelper.transform.Translate((Vector3.up + Vector3.forward) * 0.1f);
            animatorStateController.toPlaying();
            StartCoroutine(sleep(1f));
            
        }
    }

    IEnumerator sleep(float time)
    {
        yield return new WaitForSeconds(time);
        
        climbHelper.SetActive(false);
        
    }

}
