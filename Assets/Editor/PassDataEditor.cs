using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Codice.CM.Common;

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
        data.passName = EditorGUILayout.TextField("Pass Name:", data.passName);
        foreach(var i in data.passItems) 
        {

            EditorGUILayout.BeginHorizontal();
            i.displayInMenu = EditorGUILayout.Foldout(i.displayInMenu, i.GetItemName());
            if (GUILayout.Button("-"))
            {
                data.passItems.Remove(i);
                break;
            }
            EditorGUILayout.EndHorizontal();
            if (i.displayInMenu)
            {
                i.itemType = (PassItem.EPassItemType)EditorGUILayout.EnumPopup("Item Type:", i.itemType);
                i.value = EditorGUILayout.IntField("Value:", i.value);
            }
            
        }


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
