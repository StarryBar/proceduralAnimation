using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foxController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private ConfigurableJoint hipJoint;

    [SerializeField]
    private Rigidbody hip;
    [SerializeField]
    private Animator sourceAnimator;

    public Transform hipMove;

    float moveX = 1f;
    float moveZ = 0f;

    private bool walk;
    private bool run;
    private bool getUp;
    Vector3 ddr;

    // Start is called before the first frame update
    void Start()
    {
        walk = false;
        run = false;
        getUp = false;
        ddr = -transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");
        //moveX = Mathf.Cos(10 * Time.time);
        //moveZ = Mathf.Sin(10 * Time.time);

        //Vector3 dir = new Vector3(moveX, 0, moveZ).normalized;

        //if (moveZ > 0.1f)
        //{
            //float targetAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        float theta = moveX * 90f;
        this.hipJoint.targetRotation = Quaternion.Euler(0f, theta, 0);
        //transform.rotation(this.hipJoint.targetRotation);
        
        if (hipMove.position.x < -8.0f && ddr.x == -1)
        {
            this.hipJoint.targetRotation = Quaternion.Euler(0f, 180, 0);
            Debug.Log("changeLeft, ddr= " +  ddr);
            ddr = new Vector3(1, 0, 0);
        }

        if (hipMove.position.x > 3.0f && ddr.x == 1f)
        {
            this.hipJoint.targetRotation = Quaternion.Euler(0f, 180, 0);
            Debug.Log("changeRight, ddr= " + ddr);
            ddr = new Vector3(-1, 0, 0);
        }
        Debug.Log("addforce, ddr= " + ddr);
        this.hip.AddForce(ddr * this.speed * 40f);

        /*
         //dir += new Vector3(0, 10, 0);
         //Vector3 hight = new Vector3(0, 15, 0);
         //this.hip1.AddForce(hight * this.speed);
         //this.hip2.AddForce(hight * this.speed);
         //this.hip3.AddForce(hight * this.speed);
         //this.hip4.AddForce(hight * this.speed);
         if (Input.GetKeyDown(KeyCode.LeftShift))
         {
             //this.run = true;
         }
         else
         {
             //this.walk = true;
         }

     }
     else
     {
         //this.walk = false;
         //this.run = false;
     }*/

        //Debug.Log(Input.GetKey(KeyCode.Space));
        if (Input.GetKey(KeyCode.Space))
        {

            this.hip.AddForce(new Vector3(0f, 200f, 0f));
        }
        
        if (Input.GetKey(KeyCode.R))
        {
            this.getUp = true;
        } else
        {
            this.getUp = false;
        }


        this.sourceAnimator.SetBool("Walk", this.walk);
        this.sourceAnimator.SetBool("Run", this.run);
        this.sourceAnimator.SetBool("GetUp", this.getUp);
    }
}
