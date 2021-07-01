using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private ConfigurableJoint hipJoint;
    [SerializeField]
    private Rigidbody hip;
    [SerializeField]
    private Animator sourceAnimator;
    float moveX = 1f;
    float moveZ = 0f;

    private bool walk = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //moveX = Input.GetAxisRaw("Horizontal");
        //moveZ = Input.GetAxisRaw("Vertical");
        moveX = Mathf.Cos(10*Time.time);
        moveZ = Mathf.Sin(10*Time.time);

        Vector3 dir = new Vector3(moveX, 0, moveZ).normalized;

        if (dir.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            this.hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            this.hip.AddForce(dir * this.speed);
            this.walk = true;
        } else
        {
            this.walk = false;
        }
        Debug.Log(Input.GetKey(KeyCode.Space));
        if (Input.GetKey(KeyCode.Space))
        {
            
            this.hip.AddForce(new Vector3(0f,200f,0f));
        }

        this.sourceAnimator.SetBool("Walk", this.walk);
    }
}
