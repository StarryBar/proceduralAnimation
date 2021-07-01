using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AnimatorStateController
{
    private Animator animator;


    public AnimatorStateController(Animator _animator)
    {
        animator = _animator;
    }


    public void toWalking()
    {
        animator.SetInteger("PlayerState", (int)PlayerState.Walking);
    }

    public void toClimbing()
    {
        animator.SetInteger("PlayerState", (int)PlayerState.Climbing);
    }

    public void toIdle()
    {
        animator.SetInteger("PlayerState", (int)PlayerState.Idle);
    }

    public void toPlaying()
    {
        animator.SetInteger("PlayerState", (int)PlayerState.Playing);
    }

    public PlayerState getState()
    {
        var state = animator.GetInteger("PlayerState");
        switch (state)
        {
            case (int)PlayerState.Idle:
                return PlayerState.Idle;
            case (int)PlayerState.Walking:
                return PlayerState.Walking;
            case (int)PlayerState.Climbing:
                return PlayerState.Climbing;
            case (int)PlayerState.Playing:
                return PlayerState.Playing;
            default:
                Debug.Log("Can not get a state!?");
                return PlayerState.Error;
        }



    }
}