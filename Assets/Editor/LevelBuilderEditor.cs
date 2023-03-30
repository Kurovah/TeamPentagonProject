using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelBuilder))]
public class LevelBuilderEditor : Editor
{
    LevelBuilder t;
    private void OnEnable()
    {
        t = (LevelBuilder)target;
        t.isActive = true;
    }

    private void OnDisable()
    {
        t.isActive = false;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
    }
}
