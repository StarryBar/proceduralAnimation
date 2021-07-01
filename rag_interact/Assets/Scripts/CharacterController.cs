using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private ConfigurableJoint hipJoint;
    [SerializeField] private Rigidbody hip;
    [SerializeField] private Transform hipPos;

    [SerializeField] private Animator targetAnimator;

    private bool isWalking = false;
    private bool isClimbing = false;
    private Vector3 ddr;
    // Start is called before the first frame update
    void Start()
    {
        ddr = new Vector3(1, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        /*float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

            this.hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

            this.hip.AddForce(direction * this.speed);

            this.walk = true;
        }  else {
            this.walk = false;
        }*/
        if (hipPos.position.x > 20f && ddr.x == 1)
        {
            this.hipJoint.targetRotation = Quaternion.Euler(0, 180, 0);
            ddr = new Vector3(-1, 0, 0);
        }

        if (hipPos.position.x < -10f && ddr.x == -1)
        {
            this.hipJoint.targetRotation = Quaternion.Euler(0, 0, 0);
            ddr = new Vector3(1, 0, 0);
        }
        isClimbing = this.targetAnimator.GetBool("Climb");
        isWalking = this.targetAnimator.GetBool("Walk");
        
        
        if (isWalking)
        {
            Debug.Log(hipPos.transform.forward);
            this.hip.AddForce(-hipPos.forward * this.speed);
        }

        initWalkState();


        this.targetAnimator.SetBool("Walk", this.isWalking);
    }


    private void initWalkState()
    {
        
        if (!isWalking && !isClimbing)
        {
            float seed = Random.Range(0f, 1f);

            isWalking = seed > 0.97 ? true : false;
        }
        //else if (isWalking && !isClimbing)
        //{

        //    float seed = Random.Range(0f, 1f);
        //    isWalking = seed > 0.9999 ? false : true;
        //}

    }

}
