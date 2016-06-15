using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor
{
    // allows viewing + manipulation of bezier path

    private BezierCurve curve;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private const int lineSteps = 10;
    private const float directionScale = 0.5f;

    private void OnSceneGUI()
    {
        // set editor target
        curve = target as BezierCurve;
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

        // show 3 main points on line
        Vector3 p0 = ShowPoint(0);
        Vector3 p1 = ShowPoint(1);
        Vector3 p2 = ShowPoint(2);
        Vector4 p3 = ShowPoint(3);

        // draw path
        Handles.color = Color.blue;
        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p2, p3);

        ShowDirections();
        Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
        //// draw straight lines between successive steps on the curve.
    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = curve.GetPoint(0f);
        Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
        for (int i = 1; i <= lineSteps; i++)
        {
            point = curve.GetPoint(i / (float)lineSteps);
            Handles.DrawLine(point, point + curve.GetDirection(i / (float)lineSteps) * directionScale);
        }
    }

    private Vector3 ShowPoint(int index)
    {
        // calculate point in world space. Show handles - update if changed

        Vector3 point = handleTransform.TransformPoint(curve.points[index]);

        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.points[index] = handleTransform.InverseTransformPoint(point);
        }
        
        return point;
    }

}
