using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class FreeClimb : MonoBehaviour //必须用humanoid avatar，因为AvatarIKGoal.LeftFoot和动画绑定
    {
        public Animator anim;
        public bool isClimbing;
        public bool isWalking;
        public bool isJumping;

        bool inPosition;
        bool isLerping;
        float t;

        
        //float posT;
        Vector3 startPos;
        Vector3 targetPos;
        //Quaternion startRot;
        //Quaternion targetRot;

        [SerializeField] private float positionOffset = 0.05f;
        [SerializeField] private float offsetFromWall = 0.03f;
        [SerializeField] private float speed_multiplier = 0.02f;
        [SerializeField] private float climbSpeed = 0.3f;
        [SerializeField] private float rotateSpeed = 0.5f;
        [SerializeField] private float walkingSpeed = 0.3f;
        [SerializeField] private float jumpForce = 0.3f;
        [SerializeField] private float climbRange = 0.1f;
        [SerializeField] private float moveForwardRange = 0.1f;
        [SerializeField] private float moveDownRange = 0.3f;

        [SerializeField] private GameObject goalObj;
        //[SerializeField] private float inAngleDis = 1;

        public IKSnapshot baseIKSnapshot;
        private IKSnapshot defaultSnapshot;
        public FreeClimbAnimHook a_hook;

        Transform helper;
        float delta;
        private void Awake()
        {
            defaultSnapshot = new IKSnapshot();
        }
        void Start()
        {
            Init();
        }

        public void Init()
        {
            helper = new GameObject().transform;
            helper.name = "climb helper";

            a_hook.Init(this, helper);
            isClimbing = false; isWalking = true; isJumping = false;
        }

        public void CheckForClimb()
        {
            Vector3 origin = transform.position;
            //origin.y += 0f;
            Vector3 dir = transform.forward;

            
            RaycastHit hit;
            Debug.DrawRay(origin, dir * climbRange, Color.green);
            if (Physics.Raycast(origin, dir, out hit, climbRange))
            {
                Debug.Log("org: " + origin.ToString() + "dir: " + (dir*climbRange).ToString() + "hitPos" + hit.point.ToString());
                helper.position = PosWithOffset(origin, hit.point);
                InitforClimb(hit);
            }


        }
        void InitforClimb(RaycastHit hit)
        {
            isClimbing = true; isWalking = false; isJumping = false;
            GetComponent<Rigidbody>().useGravity = false;   //change4 offset gravity && remove collider component
            GetComponent<Rigidbody>().velocity = Vector3.zero;//
            helper.transform.rotation = Quaternion.LookRotation(-hit.normal);
            startPos = transform.position;
            targetPos = hit.point + (hit.normal * offsetFromWall);
            t = 0;
            inPosition = false;
            anim.CrossFade("climb_idle", 2);

        }


        private void Update()
        {
            delta = Time.deltaTime;
            navToTarget(goalObj.transform);
        }

        private void navToTarget(Transform goal)
        {



            if (isWalking)
            {
                //doWalk(Vector3.forward);// change 1    
            }

            if (!isClimbing) // change2,   change3  climb range 0.1, jump force 25,   rigid drag 10
            {
                CheckForClimb();
            }
            
            if (isClimbing)
            {
                doClimb(goal.position - transform.position); // change11
            }

            if (isJumping)
            {
                doJump(Vector3.up, helper.up);
                Debug.Log("has jumped?" + isJumping);
                isJumping = false;
            }
        }

        private void doWalk(Vector3 dir)
        {

            Vector3 moveDir = dir;
            transform.Translate(moveDir * walkingSpeed * delta);
        }

        private void doJump(Vector3 upDir, Vector3 frontDir)
        {
            //GetComponent<BoxCollider>().enabled = true; // change5 remove collider
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().AddForce(upDir * jumpForce);
            GetComponent<Rigidbody>().AddForce(frontDir * jumpForce/3f);

            a_hook.UpdateIKWeight(AvatarIKGoal.LeftHand, 0); //change8 tmp disable ik
            a_hook.UpdateIKWeight(AvatarIKGoal.RightHand, 0);
            a_hook.UpdateIKWeight(AvatarIKGoal.LeftFoot, 0);
            a_hook.UpdateIKWeight(AvatarIKGoal.RightFoot, 0);

        }


        private void doClimb(Vector3 upDir)
        {
            if (!inPosition)
            {
                GetInPosition();//这部分是一次性的
                return;
            }

            if (!isLerping) // set helper pos && detect canmove && set limb pos
            {
                //float hor = Input.GetAxis("Horizontal");
                //float vert = Input.GetAxis("Vertical");
                //float m = Mathf.Abs(hor) + Mathf.Abs(vert);

                //Vector3 h = helper.right * Input.GetAxis("Horizontal");
                //Vector3 v = helper.up * Input.GetAxis("Vertical");
                //Vector3 moveDir = (h + v).normalized;
                Vector3 moveDir = upDir;

                bool canMove = CanMove(moveDir);  //其中定义helper pos
                if (!canMove || moveDir == Vector3.zero)
                {
                    if (!canMove)
                    {
                        Debug.Log("isJumping?" + isJumping);
                        isClimbing = false; isWalking = false; isJumping = true;
                    }
                    return;
                }
                    

                t = 0;
                isLerping = true;
                startPos = transform.position;
                targetPos = helper.position;
                a_hook.CreatePosition(targetPos); //其中定义四肢pos with事前传进去的ik大致位置


            }
            else
            {
                t += delta * climbSpeed;
                if (t > 1)
                {
                    t = 1;
                    isLerping = false;
                }

                Vector3 cp = Vector3.Lerp(startPos, targetPos, t);
                transform.position = cp;
                transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);
            }
        }


        void GetInPosition()
        {
            t += delta;

            if (t > 1)
            {
                t = 1;
                inPosition = true;

                //enable the ik
                a_hook.CreatePosition(targetPos);

            }

            Vector3 tp = Vector3.Lerp(startPos, targetPos, t); // change7 in pos 1->t
            transform.position = tp;
        }

        bool CanMove(Vector3 targetDir) // change11 better naming scope, goalpos,dir = helper.up
        {
            Vector3 origin = transform.position;
            float disForward = moveForwardRange;
            Vector3 dir = helper.up;// change11 dir = helper.up
            Debug.DrawRay(origin, dir * disForward, Color.red);
            RaycastHit hit;
            
            if (Physics.Raycast(origin, dir ,out hit, disForward)) // front ray empty
            {
                Debug.Log("90 upward");
                helper.position = PosWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal, getHelperRot(targetDir, -hit.normal));//change10
                return true;
                //return false;
            }

            origin += helper.up * disForward; // next step front ray empty  // change11 dir = helper.up
            dir = helper.forward;
            Debug.DrawRay(origin, dir * disForward, Color.blue);
            if (Physics.Raycast(origin, dir, out hit, disForward))
            {
                Debug.Log("climb upward");
                helper.position = hit.point + hit.normal * offsetFromWall;// PosWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal, getHelperRot(targetDir, -hit.normal));//change10
                return true;
            }

            origin += helper.forward * disForward; // next step front + down ray empty
            dir = Vector3.down;
            float disDown = moveDownRange;
            
            if (Physics.Raycast(origin, dir ,out hit, disDown))
            {
                Debug.Log("90 downward");
                Debug.DrawRay(origin, dir * 10, Color.yellow);
                float angle = Vector3.Angle(helper.up, hit.normal);
                if (angle < 40)
                {
                    helper.position = hit.point + hit.normal * offsetFromWall;//PosWithOffset(origin, hit.point); //hitPos->origin with offset
                    helper.rotation = Quaternion.LookRotation(-hit.normal, getHelperRot(targetDir, -hit.normal));//change10
                    return true;
                }
            }

            return false;
        }



        Vector3 PosWithOffset(Vector3 origin, Vector3 target)
        {
            Vector3 direction = (origin - target).normalized;
            Vector3 offset = direction * offsetFromWall;
            return target + offset;
        }

        Vector3 getHelperRot(Vector3 targetDir, Vector3 negNormal) // change10 direction
        {
            Vector3 right = Vector3.Cross(targetDir, Vector3.up);
            Vector3 helperFront = Vector3.Cross(right, negNormal);
            return helperFront;

        }

    }



    [System.Serializable]
    public class IKSnapshot
    {
        public Vector3 rh, lh, lf, rf;
    }


}
