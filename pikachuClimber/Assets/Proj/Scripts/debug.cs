using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debug : MonoBehaviour
{
    public static bool debugEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (debugEnabled)
        {
            this.enabled = true;
        } else
        {
            this.enabled = false;
        }
    }
}
