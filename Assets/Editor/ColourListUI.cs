using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(ColourList))]
public class ColourListUI : Editor
{
    public override void OnInspectorGUI()
    {
        ColourList obj = (ColourList)target;
        if(GUILayout.Button("Add Colour"))
        {
            obj.colors.Add(new ColorListing());
        }

        for(int i = 0; i<obj.colors.Count; i++)
        {
            var c = obj.colors[i];

            c.collapsed = EditorGUILayout.Foldout(c.collapsed, c.c_Name);
            if (!c.collapsed)
            {
                //name
                c.c_Name = EditorGUILayout.TextField("Name:", c.c_Name);
                //color
                c.color = EditorGUILayout.ColorField("Colour:", c.color);
                //button to remove listing
                if (GUILayout.Button("-"))
                {
                    obj.colors.RemoveAt(i);
                    break;
                }
            }  
        }

        if (GUILayout.Button("Save List"))
        {
            EditorUtility.SetDirty(obj);
        }
    }
}
