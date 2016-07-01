using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System;
using System.Collections.Generic;

public class RS3DOccluder : MonoBehaviour
{

    public float absorption_percent = 75.0f;

    // Use this for initialization
    void Start()
    {
     //   gameObject.AddComponent<MeshFilter>();
        //        gameObject.AddComponent<MeshRenderer>();
        //Mesh mesh = GetComponent<MeshFilter>().mesh;

        ////For debugging, print out all triangles
        //int num_triangles = mesh.triangles.Length / 3;
        //for(int i = 0; i < num_triangles; ++i)
        //{
        //    Debug.Log("Triangle "+ i + ": " + mesh.vertices[mesh.triangles[i * 3 + 0] ] + ", " + mesh.vertices[mesh.triangles[i * 3 + 1]] + ", " + mesh.vertices[mesh.triangles[i * 3 + 2]]);
        //}

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Get triangle vertex list of parent mesh (size is divisible by 3)
    public List<Vector3> GetTriangleVertexList()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        List<Vector3> list = new List<Vector3>();
        int num_triangles = mesh.triangles.Length / 3;
        for (int i = 0; i < num_triangles; ++i)
        {
            list.Add(transform.TransformPoint(mesh.vertices[mesh.triangles[i * 3 + 0]]));
            list.Add(transform.TransformPoint(mesh.vertices[mesh.triangles[i * 3 + 1]]));
            list.Add(transform.TransformPoint(mesh.vertices[mesh.triangles[i * 3 + 2]]));
        }
        return list;
    }

    void OnDrawGizmosSelected()
    {
        float scale_multiplier = 1.05f;
        //Draw mesh
        Gizmos.color = new Color(1.0f, 1.0f, 1.0f);
        Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, transform.position, transform.rotation, transform.localScale * scale_multiplier);

        Gizmos.color = new Color(1.0f, 1.0f, 0.0f, absorption_percent / 100.0f * 0.5f);
        Gizmos.DrawMesh(GetComponent<MeshFilter>().sharedMesh, transform.position, transform.rotation, transform.localScale * scale_multiplier);

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(RS3DOccluder))]
public class RS3DOccluderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RS3DOccluder myTarget = (RS3DOccluder)target;

        myTarget.absorption_percent = EditorGUILayout.FloatField("Wall Sound Absorption Percent", myTarget.absorption_percent);
    }

}
#endif