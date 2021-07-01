using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class touchableFootSensor : MonoBehaviour
{

    [SerializeField] private Animator targetAnimator;
    [SerializeField] private GameObject climbHelper;

    private AnimatorStateController animatorStateController;

    //private void Start()
    //{
    //    animatorStateController = new AnimatorStateController(targetAnimator);
    //    climbHelper.SetActive(false);
        
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (!CharacterController.agent.enabled)
    //    {
    //        climbHelper.transform.Translate((Vector3.up + Vector3.forward) * 1.8f);
    //        StartCoroutine(sleep(0.1f));
    //        climbHelper.SetActive(false);
    //        animatorStateController.toIdle();
    //    }
    //}

    //IEnumerator sleep(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //}

}
