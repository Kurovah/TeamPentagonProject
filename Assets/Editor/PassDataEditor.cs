using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PassData))]
public class PassDataEditor : Editor
{
    PassData data;
    private void OnEnable()
    {
        data = (PassData)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();





        if (GUILayout.Button("Add new item"))
        {
            data.passItems.Add(new PassItem());
        }
        if (GUILayout.Button("Save Data"))
        {
            EditorUtility.SetDirty(target);
        }
    }
}
