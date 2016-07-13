using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RS3DRoom : MonoBehaviour {

    public int order_early_reflection = 3;
    public int order_late_reflection = 0;
    public float reverb_length_ms = 0;

    //Reflection percentages
    public float refl_percent_left = 75.0f;
    public float refl_percent_right = 75.0f;
    public float refl_percent_front = 75.0f;
    public float refl_percent_back = 75.0f;
    public float refl_percent_floor = 75.0f;
    public float refl_percent_ceil = 75.0f;
    public float all_refl = -1f;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (all_refl != -1)
        {
            refl_percent_left = all_refl;
            refl_percent_right =all_refl;
            refl_percent_front =all_refl;
            refl_percent_back = all_refl;
            refl_percent_floor =all_refl;
            refl_percent_ceil = all_refl;
        }
}

    void drawWall(Vector3 center, Vector3 size, Color colorRGB, float refl_percent)
    {
        Gizmos.color = new Color(1.0f, 1.0f, 1.0f);
//        Gizmos.color = new Color(colorRGB.r, colorRGB.g, colorRGB.b, refl_percent / 100.0f);
//        Gizmos.color = new Color(colorRGB.r, colorRGB.g, colorRGB.b);
        Gizmos.DrawWireCube(center, size);

        Gizmos.color = new Color(0.0f, 1.0f, 1.0f, refl_percent / 100.0f * 0.75f);
        //Gizmos.color = new Color(colorRGB.r, colorRGB.g, colorRGB.b, refl_percent / 100.0f / 2);
        Gizmos.DrawCube(center, size);

    }

    void OnDrawGizmosSelected()
    {
        //wall thickness
        float thickness = 0.1f;
        float thickness_shrink_coef = 0.95f;
        //Left wall
        drawWall(new Vector3(transform.position.x - (transform.localScale.x + thickness) / 2, transform.position.y, transform.position.z), new Vector3(thickness * thickness_shrink_coef, transform.localScale.y, transform.localScale.z), 
            new Color(1.0f, 0.0f, 0.0f), refl_percent_left);

        //Right  wall
        drawWall(new Vector3(transform.position.x + (transform.localScale.x + thickness) / 2, transform.position.y, transform.position.z), new Vector3(thickness * thickness_shrink_coef, transform.localScale.y, transform.localScale.z),
            new Color(1.0f, 0.0f, 0.0f), refl_percent_right);

        //Front  Wall 
        drawWall(new Vector3(transform.position.x, transform.position.y, transform.position.z + (transform.localScale.z + thickness) / 2), new Vector3(transform.localScale.x, transform.localScale.y, thickness * thickness_shrink_coef),
            new Color(0.0f, 0.0f, 1.0f), refl_percent_front);

        //Back Wall 
        drawWall(new Vector3(transform.position.x, transform.position.y, transform.position.z - (transform.localScale.z + thickness) / 2), new Vector3(transform.localScale.x, transform.localScale.y, thickness * thickness_shrink_coef),
            new Color(0.0f, 0.0f, 1.0f), refl_percent_back);

        //Floor  Wall 
        drawWall(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y + thickness) / 2, transform.position.z), new Vector3(transform.localScale.x, thickness * thickness_shrink_coef, transform.localScale.z),
            new Color(0.0f, 1.0f, 0.0f), refl_percent_floor);

        //Ceiling Wall 
        drawWall(new Vector3(transform.position.x, transform.position.y + (transform.localScale.y + thickness) / 2, transform.position.z), new Vector3(transform.localScale.x, thickness * thickness_shrink_coef, transform.localScale.z),
            new Color(0.0f, 1.0f, 0.0f), refl_percent_floor);


    }

}

#if UNITY_EDITOR

////////////////////////////////////////////////////////////////////////////////////////

[CustomEditor(typeof(RS3DRoom))]
public class RS3DRoomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RS3DRoom myTarget = (RS3DRoom)target;

        myTarget.order_early_reflection = EditorGUILayout.IntField("Early Reflection Order", myTarget.order_early_reflection);
        myTarget.order_late_reflection = EditorGUILayout.IntField("Late Reflection Order (0=auto)", myTarget.order_late_reflection);
        myTarget.reverb_length_ms = EditorGUILayout.FloatField("Reverb Length (ms) (0=auto)", myTarget.reverb_length_ms);

        EditorGUILayout.BeginVertical();

        GUILayout.Label("Wall Reflection Percentages");
        EditorGUI.indentLevel++;
        myTarget.refl_percent_left = EditorGUILayout.FloatField("Left", myTarget.refl_percent_left);
        myTarget.refl_percent_right = EditorGUILayout.FloatField("Right", myTarget.refl_percent_right);
        myTarget.refl_percent_front = EditorGUILayout.FloatField("Front", myTarget.refl_percent_front);
        myTarget.refl_percent_back = EditorGUILayout.FloatField("Back", myTarget.refl_percent_back);
        myTarget.refl_percent_floor = EditorGUILayout.FloatField("Floor", myTarget.refl_percent_floor);
        myTarget.refl_percent_ceil = EditorGUILayout.FloatField("Ceiling", myTarget.refl_percent_ceil);

        // add all
        myTarget.all_refl = EditorGUILayout.FloatField("All", myTarget.all_refl);
        EditorGUI.indentLevel--;

        EditorGUILayout.EndVertical();
    }

}

#endif