using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyMesh : MonoBehaviour
{
    public float Mass = 1f;
    public float stiffness = 1f;
    public float damping = 0.9f;
    private Mesh OriginalMesh, MeshClone;
    private MeshRenderer renderer;
    private JellyVertex[] jv_world;
    private Vector3[] jv_obj;
    // Start is called before the first frame update
    void Start()
    {
        OriginalMesh = GetComponent<MeshFilter>().sharedMesh;
        MeshClone = Instantiate(OriginalMesh);
        GetComponent<MeshFilter>().sharedMesh = MeshClone;
        renderer = GetComponent<MeshRenderer>();
        jv_world = new JellyVertex[MeshClone.vertices.Length];
        for (int i = 0; i < MeshClone.vertices.Length; i++)
            jv_world[i] = new JellyVertex(i, transform.TransformPoint(MeshClone.vertices[i]));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        jv_obj = OriginalMesh.vertices;

        for (int i=0; i < jv_world.Length; i++)
        {
            int idd = jv_world[i].ID;
            Vector3 jv_world_static = transform.TransformPoint(jv_obj[idd]);
            jv_world[i].Shake(jv_world_static, Mass, stiffness, damping);
            Vector3 jv_obj_shaked = transform.InverseTransformPoint(jv_world[i].Position);
            jv_obj[idd] = jv_obj_shaked;

        }

        MeshClone.vertices = jv_obj;
    }

    public class JellyVertex
    {
        public int ID;
        public Vector3 Position;
        public Vector3 velocity, Force;
        public JellyVertex(int _id, Vector3 _pos)
        {
            ID = _id;
            Position = _pos;
        }

        public void Shake(Vector3 target, float m, float s, float d)
        {
            Force = (target - Position) * s;
            velocity = (velocity + Force / m) * d;
            Position += velocity;
            if ((velocity + Force / m).magnitude < 0.001f)
                Position = target;
        }
    }
}
