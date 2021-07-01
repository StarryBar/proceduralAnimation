using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compute : MonoBehaviour
{
    public ComputeShader calcMeshShader;

    private ComputeBuffer preBuffer;
    private ComputeBuffer nextBuffer;
    private ComputeBuffer resultBuffer;

    public Vector3[] array1;
    public Vector3[] array2;

    public Vector3[] resultArr;

    public int length = 16;
    private int kernel;

    // Start is called before the first frame update
    void Start()
    {
        array1 = new Vector3[length];
        array2 = new Vector3[length];
        resultArr = new Vector3[length];
        for (int i = 0; i<length; i++)
        {
            array1[i] = Vector3.one;
            array2[i] = Vector3.one * 2;
        }

        InitBuffers();
        kernel = calcMeshShader.FindKernel("CSMain");
        calcMeshShader.SetBuffer(kernel, "preVertices", preBuffer);
        calcMeshShader.SetBuffer(kernel, "nextVertices", nextBuffer);
        calcMeshShader.SetBuffer(kernel, "Result", resultBuffer);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            calcMeshShader.Dispatch(kernel, 2, 2, 1);
            resultBuffer.GetData(resultArr);
            resultBuffer.Release();
        }
    }

    void InitBuffers()
    {
        preBuffer = new ComputeBuffer(array1.Length, 12);
        preBuffer.SetData(array1);
        preBuffer = new ComputeBuffer(array2.Length, 12);
        preBuffer.SetData(array2);
        resultBuffer = new ComputeBuffer(resultArr.Length, 12);
    }

    private void OnDestroy()
    {
        preBuffer.Release();
        nextBuffer.Release();
    }
}
