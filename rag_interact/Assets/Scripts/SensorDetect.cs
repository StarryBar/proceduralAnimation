using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorDetect : MonoBehaviour
{
    [SerializeField] private Rigidbody hip;
    [SerializeField] private float speed = 500f;
    [SerializeField] private Animator targetAnimator;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 10) return;
        
        Vector3 up = new Vector3(0, 1, 0);
        float seed = Random.Range(0f, 1f);
        if (seed > 0.999)
            this.hip.AddForce(up * this.speed);
    }
}
