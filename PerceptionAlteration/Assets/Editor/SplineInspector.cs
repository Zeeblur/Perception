using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Spline))]
public class SplineInspector : Editor
{
    private const int lineSteps = 10;               // how smooth it is 
    private const float directionScale = 0.5f;      // how big the direction lines to be drawn

    private Spline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private const int stepsPerCurve = 10;           // equal direction lines

    // handle attributes
    private const float handleSize = 0.04f;
    private const float pickSize = 0.06f;

    // only show handles for selected curve
    private int selectedIndex = -1;

    // Colours for mode points
    private static Color[] modeColours = {
		Color.white,
		Color.yellow,
		Color.cyan
	};


    private void OnSceneGUI()
    {
        spline = target as Spline;
        handleTransform = spline.transform;

        // if local pivot use it's own rotation
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

        // set start point for curve
        Vector3 p0 = ShowPoint(0);

        // iterate over each curve
        for (int i = 1; i < spline.ControlPointCount; i += 3 ) 
        {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);
            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            
            // set start as ending point
            p0 = p3;
        }

        ShowDirections();

    }

    // Override to show button on inspector
    public override void OnInspectorGUI()
    {

        spline = target as Spline;
        DrawDefaultInspector();


        // set looping true/false
        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Loop", spline.Loop);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Toggle Loop");
            EditorUtility.SetDirty(spline);
            spline.Loop = loop;
        }

        // show text fields only for selected point in the inspector
        if (selectedIndex >= 0 && selectedIndex < spline.ControlPointCount)
        {
            DrawSelectedPointInspector();
        }

        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(spline, "Add Curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);
        }

    }

    private void DrawSelectedPointInspector()
    {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetControlPoint(selectedIndex));

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            spline.SetControlPoint(selectedIndex, point);
        }

        // check if point mode has changed
        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)EditorGUILayout.EnumPopup("Mode", spline.GetControlPointMode(selectedIndex));
       
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Change Point Mode");
            spline.SetControlPointMode(selectedIndex, mode);
            EditorUtility.SetDirty(spline);
        }
     
    }

    private Vector3 ShowPoint(int index)
    {
        // get position
        Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));

        // scale of dots
        float size = HandleUtility.GetHandleSize(point); 

        // double size of first dot to see easier
        if (index == 0)
        {
            size *= 2f;
        }

        Handles.color = modeColours[(int)spline.GetControlPointMode(index)];  // colour coordinate modes

        // draw button. if selected handle will draw 
        if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotCap))
        {
            selectedIndex = index;
            Repaint();                  // updates inspector with new point
        }

        // draw selected handle only 
        if (selectedIndex == index)
        {
            // check if changed
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);

            if (EditorGUI.EndChangeCheck())
            {
                // undo-able
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
            }
        }

        return point;
    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = spline.GetPoint(0f);
        Handles.DrawLine(point, point + spline.GetDirection(0f) * directionScale);
        int steps = stepsPerCurve * spline.CurveCount;

        for (int i = 1; i <= steps; i++)
        {
            point = spline.GetPoint(i / (float)steps);
            Handles.DrawLine(point, point + spline.GetDirection(i / (float)steps) * directionScale);
        }
    }



}
