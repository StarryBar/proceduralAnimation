using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float walkingSpeed = 0.3f;
    [SerializeField] private float climbingSpeed = 0.3f;
    [SerializeField] private float climbingFreq = 0.5f;
    //[SerializeField] private float climbingForce = 150f;
    [SerializeField] private ConfigurableJoint hipJoint;
    [SerializeField] private Rigidbody hip;
    [SerializeField] private Transform hipPos;
    [SerializeField] private GameObject hp;
    //[SerializeField] private Transform goalPos;
    //[SerializeField] private GameObject goalObj;
    [SerializeField] private GameObject prefabBall;
    [SerializeField] private Transform orgMasterPos;
    [SerializeField] private GameObject orgMasterObj;
    [SerializeField] private float touchableRange = 0.5f;

    static public Transform masterPos;
    static public GameObject masterObj;

    static public Transform goalPos;
    static public GameObject goalObj;

    [SerializeField] private Animator targetAnimator;

    [SerializeField] private GameObject navMeshDebug;
    [SerializeField] private GameObject navFreeDebug;
    [SerializeField] private GameObject retargetDebug;
    [SerializeField] private GameObject climbHelper;

    private AnimatorStateController animatorStateController;
    public static NavMeshAgent agent;

    private bool endTaskOne = false;
    private bool endTaskTwo = false;
    private bool endTaskThree = false;
    private bool endTaskFour = false;
    private bool startTaskOne = false;
    private bool startTaskTwo = false;
    private bool startTaskThree = false;
    private bool startTaskFour = false;
    private bool isBodyHitBefore = false;
    private bool isForward;
    private bool isPerlinTurnEnabled = false;
    private float climbingTimeOffset;
    private float navAttemptPeriod;

    // Start is called before the first frame update
    private void Awake()
    {
        masterPos = orgMasterPos;
        masterObj = orgMasterObj;
        resetAllTask();
    }
    void Start()
    {
        isBodyHitBefore = false;
        isForward = true;
        animatorStateController = new AnimatorStateController(targetAnimator);
        agent = hp.GetComponent<NavMeshAgent>();
        agent.enabled = false;
        StartCoroutine(PerlinTurn());

        navMeshDebug.SetActive(false);
        navFreeDebug.SetActive(false);

        
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
    void Update()
    {

        // run back and force
        //if (hipPos.position.x > 20f && isForward)
        //{
        //    this.hipJoint.targetRotation = Quaternion.Euler(0, 180, 0);
        //    isForward = false;
        //}

        //if (hipPos.position.x < -10f && !isForward)
        //{
        //    this.hipJoint.targetRotation = Quaternion.Euler(0, 0, 0);
        //    isForward = true;
        //}

        if (!goalObj)
        {
            if (GameObject.FindGameObjectsWithTag("goals").Length != 0)
            {
                //Debug.Assert(GameObject.FindGameObjectsWithTag("goals").Length == 1, "goals more than two");
                GameObject[] goals = GameObject.FindGameObjectsWithTag("goals");
                goalObj = goals[goals.Length - 1];
                goalPos = goalObj.transform;
            }
            else
            {
                return;
            }
        }

            

        //Task 1
        navToTarget(agent, goalPos, ref endTaskOne, ref startTaskOne);
        Debug.Log("taskOneEnd?" + endTaskOne);

        //Task 2
        if (endTaskOne)
        {
            if (!endTaskTwo)
            {
                goalObj.gameObject.GetComponent<SphereCollider>().enabled = false;
                goalObj.gameObject.GetComponent<Rigidbody>().useGravity = false;
                goalObj.transform.SetParent(hipPos);
                goalObj.transform.localPosition = Vector3.forward * 0.3f + Vector3.up * 1f;
            }
            Debug.Log("masterPos: " + masterPos.position);
            navToTarget(agent, masterPos, ref endTaskTwo, ref startTaskTwo);
        }
        Debug.Log("taskTwoEnd?" + endTaskTwo);

        //Task 3
        if (endTaskTwo)
        {
            Debug.Log("inside taskThreeEnd?" + endTaskThree);

            
            if (!endTaskThree)
            {
                Debug.Log("inside");
                //var tmp = goalObj;

                //goalObj = Instantiate(prefabBall, tmp.transform.position, tmp.transform.rotation);
                //Destroy(tmp);
                goalObj.transform.SetParent(masterObj.transform);
                addConfirguable(masterObj, goalObj);
                masterObj = goalObj;
                masterPos = goalPos;


                goalObj.tag = "Untagged";
                goalObj = null;
                goalPos = null;

                //Debug.Log("parent: " + goalObj.transform.parent.position);

                if (animatorStateController.getState() != PlayerState.Playing)
                {
                    animatorStateController.toPlaying();
                }
                    
                endTaskThree = true;
                Debug.Log("endTaskThree!");
                

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

        //Task 4
        if (endTaskThree)
        {
            if (spawnScript.isRestart)
            {
                GameObject[] nextTargets = GameObject.FindGameObjectsWithTag("nextTargets");
                Debug.Log("inside task four, target length: " + nextTargets.Length);
                GameObject nextTarget = nextTargets[nextTargets.Length - 1];
                navToTarget(agent, nextTarget.transform, ref endTaskFour, ref startTaskFour);

                if (endTaskFour)
                {
                    Debug.Log("in Task Four restart");
                    spawnScript.isRestart = false;
                    endTaskFour = false;
                }

            } 
        }
        Debug.Log("taskFourEnd?" + endTaskFour);





        // randomly change walking/idle state
        //initWalkState();

        // add force to walk
        //if (animatorStateController.getState() == PlayerState.Walking)
        //{
        //    float targetAngle = Mathf.Atan2(hipPos.forward.z, hipPos.forward.x) * Mathf.Rad2Deg;
        //    this.hipPos.Translate(Vector3.forward * this.walkingSpeed * Time.deltaTime);
        //}

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

    private void navToTarget(NavMeshAgent agent, Transform goal, ref bool navFinished, ref bool navStarted)
    {
        //可能enabled但没有destination，提前关闭coroutine，直接自己寻路
        //一旦进入二，若不是一中coroutine提前开启，没有理由再进入一分支
        if (navFinished)  return;
        if (!navStarted)
        {
            navAttemptPeriod = Time.time;
            navStarted = true;
        }
        Debug.Log("destination" + agent.destination + "isGrounded?" + bodySensor.isbodyHit);
        Debug.Log("isBodyHitBefore?" + isBodyHitBefore + "isbodyHit?" + bodySensor.isbodyHit);
        Debug.Log("goalPos: " + goal.position + "agent dest pos: " + agent.destination);


        if (!isBodyHitBefore && bodySensor.isbodyHit)
        {
            retargetDebug.SetActive(!retargetDebug.activeSelf);
        }
        //if (!isBodyHitBefore && bodySensor.isbodyHit && !goal.position.Equals(agent.destination))
        //{
        //    Debug.Log("set goal pos");

        //    agent.enabled = true;
        //    agent.SetDestination(goal.position);
        //    Debug.Log("after set agent dest pos: " + agent.destination);

        //}
        isBodyHitBefore = bodySensor.isbodyHit;
        if (bodySensor.isbodyHit)
        {
            if (Time.time - navAttemptPeriod > 1f && animatorStateController.getState() != PlayerState.Climbing)
            {
                Debug.Log("periodically set goal pos");

                agent.enabled = true;
                agent.SetDestination(goal.position);
                navAttemptPeriod = Time.time;
                Debug.Log("periodically after set agent dest pos: " + agent.destination);
            }
        }

        //agent.enabled = true;
        //Debug.Log("agent enabled: " + agent.enabled);
        //Debug.Assert(agent.enabled == true, "fuckyoubefore");
        //agent.SetDestination(goal.position);
        //Debug.Log("agent enabled after: " + agent.enabled);
        //Debug.Assert(agent.enabled == true, "fuckyou");
        if (agent.enabled)
        {
            Debug.Log("has path branch");
            startSearchTarget();

            NavMeshPath nmPath = new NavMeshPath();
            agent.CalculatePath(goal.position, nmPath);
            Debug.Log("goalPos: " + goal.position);
            Debug.Log("has Path? " + (nmPath.status == NavMeshPathStatus.PathComplete));

            //for debug nav type
            if (nmPath.status == NavMeshPathStatus.PathComplete)
            {
                navMeshDebug.SetActive(true);
                navFreeDebug.SetActive(false);
            }

            Debug.Log("destination:" + agent.destination + "nmPath.status:?" + nmPath.status);
            Debug.Assert(!Vector3.positiveInfinity.Equals(agent.destination), "INF dest");
            Debug.Assert(nmPath.status == NavMeshPathStatus.PathComplete, "has dest, but no path");
            //if (!Vector3.positiveInfinity.Equals(agent.destination) && nmPath.status != NavMeshPathStatus.PathComplete)
            //{
            //    Debug.LogError("why???");
            //    Debug.Log("disable nav");
            //    agent.enabled = false;
            //    //StartCoroutine(disableNav());
            //}

        }
        else
        if (!agent.enabled)
        {
            Debug.Log("has no path branch");
            //for debug nav type
            navMeshDebug.SetActive(false);
            navFreeDebug.SetActive(true);

            moveToTarget(goal.position);

        }

        var distance = (goal.position - hipPos.position).sqrMagnitude;
        if (distance < touchableRange)
        {
            Debug.Log("fuckkk");
            agent.enabled = false;
            navFinished = true;
            animatorStateController.toIdle();
        }

    }
    
    //IEnumerator recheckNav(Transform goal)
    //{

    //    while(true)
    //    {
    //        if (animatorStateController.getState() != PlayerState.Climbing)
    //        {
    //            Debug.Log("periodically set goal pos");

    //            agent.enabled = true;
    //            agent.SetDestination(goal.position);
    //            Debug.Log("periodically after set agent dest pos: " + agent.destination);
    //            yield return new WaitForSeconds(1f);
    //        }
            
    //    }
    //}


    IEnumerator PerlinTurn()
    {
        while (isPerlinTurnEnabled)
        {
            var waitTime = 5f;
            yield return new WaitForSeconds(waitTime);
            //perlin noise based direction
            float scale = 90;
            float targetAngle = Perlin.Noise(Mathf.Sin(Time.time/ (waitTime))) * scale;
            Debug.Log("targetAngle: " + targetAngle);
            this.hipJoint.targetRotation = Quaternion.AngleAxis(targetAngle, hipPos.up);
        }
    }


    private void initWalkState()
    {
        var isWalking = animatorStateController.getState() == PlayerState.Walking;
        var isClimbing = animatorStateController.getState() == PlayerState.Climbing;

        if (!isWalking && !isClimbing)
        {
            float seed = Random.Range(0f, 1f);

            if (seed > 0.9f)
            {
                animatorStateController.toWalking();
            }
            
        }
        else if (isWalking && !isClimbing)
        {

            float seed = Random.Range(0f, 1f);
            if (seed > 0.9999)
            {
                animatorStateController.toIdle();
            }
            
        }

    }

    private void startSearchTarget()
    {

        if (animatorStateController.getState() != PlayerState.Climbing)
        {
            animatorStateController.toWalking();
        }

        var dir = agent.velocity.normalized;

        var targetAngle = Quaternion.LookRotation(dir, Vector3.up).eulerAngles.y;
        this.hipJoint.targetRotation = Quaternion.AngleAxis(-targetAngle, hipPos.up);
    }


    private void moveToTarget(Vector3 goalPos)
    {
        if (animatorStateController.getState() == PlayerState.Idle)
        {
            animatorStateController.toWalking();
        }
        if (animatorStateController.getState() == PlayerState.Playing)
        {
            animatorStateController.toWalking();
        }

        if (animatorStateController.getState() == PlayerState.Walking)
        {
            var dir = goalPos - hipPos.position;
            //var targetAngle = Quaternion.LookRotation(dir, Vector3.up).eulerAngles.y;
            //this.hipJoint.targetRotation = Quaternion.AngleAxis(-targetAngle, hipPos.up);
            Debug.Log("back:" + Vector3.back + ",forward:" + hipPos.forward + "dir:" + dir);
            this.hipJoint.targetRotation = Quaternion.Inverse(Quaternion.FromToRotation(Vector3.forward, dir));
            this.hipPos.Translate(Vector3.forward * this.walkingSpeed * Time.deltaTime);

            
        }
        if (animatorStateController.getState() == PlayerState.Climbing)
        {
            //if (Time.time - climbingTimeOffset > climbingFreq)
            //{

            //    var dir = Vector3.up + 0.1f * hip.transform.forward;
            //    Debug.Log("add force: " + dir);
            //    hip.velocity = dir * this.climbingSpeed;
            //    //hip.AddForce(dir * this.climbingForce);
            //    climbingTimeOffset = Time.time;
            //}

            climbHelper.transform.Translate(Vector3.up * climbingSpeed);
        }

    }


}
