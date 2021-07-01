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
        [SerializeField] private float touchableRange = 0.05f;

        [SerializeField] public static GameObject masterObj;
        [SerializeField] private GameObject character;
        [SerializeField] private GameObject goalObj;
        //[SerializeField] private float inAngleDis = 1;

        public IKSnapshot baseIKSnapshot;
        public FreeClimbAnimHook a_hook;


        Transform helper;
        float delta;
        Vector3 tmpTargetScale;

        private bool endTaskOne = false;
        private bool endTaskTwo = false;
        private bool endTaskThree = false;
        private bool endTaskFour = false;
        private bool startTaskOne = false;
        private bool startTaskTwo = false;
        private bool startTaskThree = false;
        private bool startTaskFour = false;

        void Start()
        {
            Init();
            resetAllTask();
        }

        private void resetAllTask()
        {
            endTaskOne = false;
            endTaskTwo = false;
            endTaskThree = false;
            endTaskFour = false;
            startTaskOne = false;
            startTaskTwo = false;
            startTaskThree = false;
            startTaskFour = false;
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
            if (Physics.Raycast(origin, dir, out hit, climbRange))
            {
                Debug.Log("org: " + origin + "dir: " + dir + "hitPos" + hit.point);
                helper.position = PosWithOffset(origin, hit.point);
                InitforClimb(hit);
            }


        }
        void InitforClimb(RaycastHit hit)
        {
            isClimbing = true; isWalking = false; isJumping = false;

            //offset gravity
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;

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

            if (!goalObj)
            {
                if (GameObject.FindGameObjectsWithTag("goals").Length != 0)
                {
                    //Debug.Assert(GameObject.FindGameObjectsWithTag("goals").Length == 1, "goals more than two");
                    GameObject[] goals = GameObject.FindGameObjectsWithTag("goals");
                    goalObj = goals[goals.Length - 1];
                }
                else
                {
                    // handle drop and climb detection
                    if (!inPosition)
                    {
                        if (!isClimbing)
                        {
                            CheckForClimb();
                        }
                        
                        if (isClimbing && !inPosition)
                        {
                            GetInPosition();//这部分是一次性的
                        }
                    }
                    return;
                }
            }

            //Task 1
            navToTarget(goalObj.transform, ref endTaskOne, ref startTaskOne);
            Debug.Log("taskOneEnd?" + endTaskOne);

            //Task 2
            if (endTaskOne)
            {
                if (!endTaskTwo)
                {
                    goalObj.gameObject.GetComponent<SphereCollider>().enabled = false;
                    goalObj.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    //tmpTargetScale = goalObj.transform.localScale;
                    //goalObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    goalObj.transform.SetParent(this.transform);
                    goalObj.transform.localPosition = Vector3.down * 2f;
                }
                Debug.Log("masterPos: " + masterObj.transform.position);
                navToTarget(masterObj.transform, ref endTaskTwo, ref startTaskTwo);
            }
            Debug.Log("taskTwoEnd?" + endTaskTwo);

            //Task 3
            if (endTaskTwo)
            {
                Debug.Log("inside taskThreeEnd?" + endTaskThree);
                if (!endTaskThree)
                {
                    Debug.Log("inside");
                    
                    goalObj.transform.SetParent(masterObj.transform);
                    //goalObj.transform.localScale = tmpTargetScale;
                    addConfirguable(masterObj, goalObj);
                    masterObj = goalObj;

                    goalObj.tag = "Untagged";
                    goalObj = null;

                    endTaskThree = true;
                }

            }
            Debug.Log("taskThreeEnd?" + endTaskThree);

            //Restart
            if (endTaskThree)
            {
                Debug.Log("reset all task");
                resetAllTask();
                return;
            }


        }

        private void addConfirguable(GameObject root, GameObject child)
        {

            child.transform.localPosition = new Vector3(0, 0, 1.5f);
            child.transform.localRotation = Quaternion.identity;
            if (!root.GetComponent<ConfigurableJoint>())
            {
                root.AddComponent<ConfigurableJoint>();

            }

            child.AddComponent<ConfigurableJoint>();
            child.GetComponent<SphereCollider>().enabled = true;
            var rJoint = root.GetComponent<ConfigurableJoint>();
            var cJoint = child.GetComponent<ConfigurableJoint>();
            cJoint.connectedBody = root.GetComponent<Rigidbody>();
            cJoint.angularXMotion = ConfigurableJointMotion.Locked;
            cJoint.angularYMotion = ConfigurableJointMotion.Locked;
            cJoint.angularZMotion = ConfigurableJointMotion.Locked;

            var xSpring = cJoint.xDrive;
            var ySpring = cJoint.zDrive;
            var zSpring = cJoint.zDrive;
            xSpring.positionSpring = 20f;
            ySpring.positionSpring = 20f;
            zSpring.positionSpring = 20f;

            cJoint.xDrive = xSpring;
            cJoint.yDrive = ySpring;
            cJoint.zDrive = zSpring;


        }

        private void navToTarget(Transform goal, ref bool navFinished, ref bool navStarted)
        {
            if (navFinished) return;
            if (!navStarted)
            {
                navStarted = true;
            }

            if (isWalking)
            {
                //doWalk(Vector3.forward);
            }

            if (!isClimbing)
            {
                CheckForClimb();
            }
            
            if (isClimbing)
            {
                var climbDir = goal.position - transform.position;
                doClimb(climbDir);
            }

            if (isJumping)
            {
                doJump(Vector3.up, helper.up);
                Debug.Log("has jumped?" + isJumping);
                isJumping = false;
            }

            var distance = (goal.position - transform.position).sqrMagnitude;
            if (distance < touchableRange)
            {
                navFinished = true;
            }
        }

        private void doWalk(Vector3 dir)
        {

            Vector3 moveDir = dir;
            transform.Translate(moveDir * walkingSpeed * delta);
        }

        private void doJump(Vector3 upDir, Vector3 frontDir)
        {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().AddForce(upDir * jumpForce);
            GetComponent<Rigidbody>().AddForce(frontDir * jumpForce/3f);

            //temporarily disable iK
            a_hook.UpdateIKWeight(AvatarIKGoal.LeftHand, 0); 
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

            Vector3 tp = Vector3.Lerp(startPos, targetPos, t);
            transform.position = tp;
        }

        bool CanMove(Vector3 targetDir)
        {
            Vector3 origin = transform.position;
            float disForward = moveForwardRange;
            Vector3 dir = helper.up;
            Debug.DrawRay(origin, dir * disForward, Color.red);
            RaycastHit hit;
            
            if (Physics.Raycast(origin, dir ,out hit, disForward)) // front ray empty
            {
                Debug.Log("90 upward");
                helper.position = PosWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal, getHelperRot(targetDir, -hit.normal));
                return true;
                //return false;
            }

            origin += helper.up * disForward; // next step front ray empty
            dir = helper.forward;
            Debug.DrawRay(origin, dir * disForward, Color.blue);
            if (Physics.Raycast(origin, dir, out hit, disForward))
            {
                Debug.Log("climb upward");
                helper.position = hit.point + hit.normal * offsetFromWall;// PosWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal, getHelperRot(targetDir, -hit.normal));
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
                    helper.rotation = Quaternion.LookRotation(-hit.normal, getHelperRot(targetDir, -hit.normal));
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
