using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter(Collision collision)
    {
        print("Enter:" + this.transform.position);
        
    }

    private void OnCollisionExit(Collision collision)
    {
        print("Exit:" + this.transform.position); 
    }
}
