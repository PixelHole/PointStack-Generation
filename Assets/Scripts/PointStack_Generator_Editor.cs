using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PointStack_Generator))]
public class PointStack_Generator_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PointStack_Generator worldgenerator = (PointStack_Generator) target;
        if (GUILayout.Button("Generate"))
        {
            worldgenerator.Create();
        }
    }
}
