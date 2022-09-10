using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SandMound_Generator))]
public class SandMound_Generator_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SandMound_Generator worldgenerator = (SandMound_Generator) target;
        if (GUILayout.Button("Generate"))
        {
            worldgenerator.Create();
        }
    }
}
