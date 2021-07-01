using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyLimbMy : MonoBehaviour
{
    [SerializeField] private Transform targetLimb;
    [SerializeField] private ConfigurableJoint m_ConfigurableJoint;

    Quaternion targetInitialRotation;
    // Start is called before the first frame update
    void Start()
    {
        this.m_ConfigurableJoint = this.GetComponent<ConfigurableJoint>();
        this.targetInitialRotation = this.targetLimb.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //this.m_ConfigurableJoint.angularXMotion = ConfigurableJointMotion.Free;
        //this.m_ConfigurableJoint.angularYMotion = ConfigurableJointMotion.Free;
        //this.m_ConfigurableJoint.angularZMotion = ConfigurableJointMotion.Free;
        this.m_ConfigurableJoint.targetRotation = copyRotation();
        //this.m_ConfigurableJoint.angularXMotion = ConfigurableJointMotion.Locked;
        //this.m_ConfigurableJoint.angularYMotion = ConfigurableJointMotion.Locked;
        //this.m_ConfigurableJoint.angularZMotion = ConfigurableJointMotion.Locked;
    }

    private Quaternion copyRotation()
    {
        return Quaternion.Inverse(this.targetLimb.localRotation) * this.targetInitialRotation;
    }
}
