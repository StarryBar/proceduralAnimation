using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchDetector : MonoBehaviour
{
    [SerializeField] private float speed = 500f;
    [SerializeField] private Animator targetAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 10) return;
        //Debug.Log("hit!!!!");

        this.targetAnimator.SetBool("Walk", false);
        this.targetAnimator.SetBool("Climb", true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 10) return;
        this.targetAnimator.SetBool("Climb", false);
    }
}
