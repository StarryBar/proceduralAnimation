using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handSensor : MonoBehaviour
{
    [SerializeField] private GameObject hip;
    [SerializeField] private float climbingForce = 300f;
    [SerializeField] private Animator targetAnimator;

    private AnimatorStateController animatorStateController;

    private void Start()
    {
        animatorStateController = new AnimatorStateController(targetAnimator);
    }


    private void OnTriggerStay(Collider other)
    {

    }
}