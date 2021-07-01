using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.AI;

//[RequireComponent(typeof(NavMeshSurface), typeof(ARMeshManager))]
public class DynamicNav : MonoBehaviour
{
    Vector3 BoundsCenter = Vector3.zero;
    Vector3 BoundsSize = new Vector3(512f, 4000f, 512f);

    LayerMask BuildMask;
    LayerMask NullMask;

    NavMeshData NavMeshData;
    NavMeshDataInstance NavMeshDataInstance;
    //NavMeshSurface tempNavMeshSurface;
    //ARMeshManager m_arMeshManager;

    public Vector3 m_Size = new Vector3(80.0f, 20.0f, 80.0f);


    void Awake()
    {
        //m_arMeshManager = GetComponent<ARMeshManager>();
        AddNavMeshData();
        BuildMask = ~0;
        NullMask = 0;
    }

    private void Start()
    {
        //Build();
        StartCoroutine(periodicBuild());
    }
    //void OnEnable()
    //{
    //    m_arMeshManager.meshesChanged += ARMesh_boundaryUpdated;
    //}
    //void OnDisable()
    //{
    //    m_arMeshManager.meshesChanged -= ARMesh_boundaryUpdated;
    //}
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //Debug.Log("Build " + Time.realtimeSinceStartup.ToString());
        //Build();
        //Debug.Log("Build finished " + Time.realtimeSinceStartup.ToString());
        //}

        /*
        else if (Input.GetKeyDown(KeyCode.U)) {
            Debug.Log("Update " + Time.realtimeSinceStartup.ToString());
            UpdateNavmeshData();
        }*/
    }
    IEnumerator periodicBuild()
    {
        while(true)
        {
            Build();
            yield return new WaitForSeconds(10f);
        }
        
    }
    //void ARMesh_boundaryUpdated(ARMeshesChangedEventArgs arMeshesChangedEventArgs)
    //{
    //Build();
    /*List<MeshFilter> arMeshesAdded = arMeshesChangedEventArgs.added;
    //MeshFilter tempMesh;// = new MeshFilter();
    Debug.Log("meshSize: " + arMeshesAdded.Count);
    List<NavMeshBuildSource> sourceList = new List<NavMeshBuildSource>();
    for (int i=0; i<arMeshesAdded.Count; i++)
    {
        //tempMesh = arMeshesAdded[i];
        var mf = arMeshesAdded[i];
        if (mf == null) continue;

        var m = mf.sharedMesh;
        if (m == null) continue;
        //tempNavMeshSurface = tempMesh.gameObject.GetComponent<NavMeshSurface>();

        var s = new NavMeshBuildSource();
        s.shape = NavMeshBuildSourceShape.Mesh;
        s.sourceObject = m;
        s.transform = mf.transform.localToWorldMatrix;
        s.area = 0;
        sourceList.Add(s);


        //if (tempNavMeshSurface)
        //{
        //    Debug.Log(tempMesh.name + " already has a NavMeshSurface, will bake it!");            
        //    tempNavMeshSurface.BuildNavMesh();
        //}
        //else
        //{
        //Debug.Log(tempMesh.name + " does not have a NavMeshSurface. Will add one and bake!");
        //Debug.Log("ar mesh boundary updated");
        //tempMesh.gameObject.AddComponent<NavMeshSurface>();
        //Debug.Log("ar mesh boundary updated with:" + tempMesh.gameObject.GetComponent<NavMeshSurface>());
        //tempMesh.gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
        //}
    }

    //NavMeshBuilder.UpdateNavMeshData(new NavMeshData(), NavMesh.GetSettingsByID(0), sourceList, QuantizedBounds());
    NavMeshBuilder.BuildNavMeshData(NavMesh.GetSettingsByID(0), sourceList, QuantizedBounds(), Vector3.zero, Quaternion.identity);*/

    //}


    void AddNavMeshData()
    {
        if (NavMeshData != null)
        {
            if (NavMeshDataInstance.valid)
            {
                Debug.Log("duplicate id:" + NavMeshDataInstance);
                // NavMesh.RemoveNavMeshData(NavMeshDataInstance);
            }
            Debug.Log("I have a data");
            NavMeshDataInstance = NavMesh.AddNavMeshData(NavMeshData);
        }
    }

    void Build()
    {
        NavMeshData = NavMeshBuilder.BuildNavMeshData(
            NavMesh.GetSettingsByID(0),
            GetBuildSources(BuildMask),
            new Bounds(BoundsCenter, BoundsSize),
            Vector3.zero,
            Quaternion.Euler(0, 0, 0));
        AddNavMeshData();

    }

    List<NavMeshBuildSource> GetBuildSources(LayerMask mask)
    {
        List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
        NavMeshBuilder.CollectSources(
            new Bounds(BoundsCenter, BoundsSize),
            mask,
            NavMeshCollectGeometry.PhysicsColliders,
            0,
            new List<NavMeshBuildMarkup>(),
            sources);
        Debug.Log("Sources found: " + sources.Count.ToString());
        return sources;
    }
}