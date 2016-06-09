using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Line))]
public class LineInspector : Editor
{

    // unity event that allows drawing within scene view
    private void OnSceneGUI()
    {
        // create target as the line
        Line line = target as Line;

        // Transform points into world space
        Transform handleTransform = line.transform;
        Vector3 p0 = handleTransform.TransformPoint(line.p0);
        Vector3 p1 = handleTransform.TransformPoint(line.p1);

        // get rotation to show position handles. Determine current pivot mode 
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

        // handles control the line
        Handles.color = Color.blue;

        // draw line using target's transformed start/end points
        Handles.DrawLine(p0, p1);

        Handles.DoPositionHandle(p0, handleRotation);
        Handles.DoPositionHandle(p1, handleRotation);

        
        // to enable handles they need to be converted back to the line's local space when points are changed.
        EditorGUI.BeginChangeCheck();
        p0 = Handles.DoPositionHandle(p0, handleRotation);

        // if handle changed, move local point
        if (EditorGUI.EndChangeCheck())
        {
            // allows user to undo action and ensures changes are logged and need to be saved
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);

            line.p0 = handleTransform.InverseTransformPoint(p0);
            EditorUtility.SetDirty(line);

        }

        // check for end point
        EditorGUI.BeginChangeCheck();
        p1 = Handles.DoPositionHandle(p1, handleRotation);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            line.p1 = handleTransform.InverseTransformPoint(p1);
            EditorUtility.SetDirty(line);
        }
     


    }

}
