using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using QJAI.QJPathfinding;

[CustomEditor(typeof(AStarGrid))]
[CanEditMultipleObjects]
public class AStarGridEditor : Editor
{
    AStarGrid aStarGrid = null;

    public void OnEnable()
    {
        if (!IsScriptValid())
            return;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (!IsScriptValid())
            return;

        if (GUILayout.Button("Update"))
        {
            aStarGrid.UpdateNodes();
            EditorUtility.SetDirty(aStarGrid);
        }
    }

    bool IsScriptValid()
    {
        if(aStarGrid == null)
            aStarGrid = target as AStarGrid;
        return aStarGrid != null;
    }
}
