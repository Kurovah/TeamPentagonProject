using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelBuilder))]
public class LevelBuilderEditor : Editor
{
    LevelBuilder t;
    bool isPlacing = false;
    private void OnEnable()
    {
        t = (LevelBuilder)target;
        t.GetAllTilesOnLayer();
    }

    private void OnDisable()
    {
        
    }

    private void OnSceneGUI()
    {
        Event e = Event.current;

        Plane plane = new Plane(Vector3.up, -t.layer);

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        float enter = 0.0f;

        if (plane.Raycast(ray, out enter))
        {
            //Get the point that is clicked
            t.cursorPos = ray.GetPoint(enter);
            //Debug.Log("Mouse Pos" + resets);
        }


        if (t.isActive)
        {
            //when clicking
            if(e.isMouse && e.button == 0)
            {
                switch (e.type)
                {
                    case EventType.MouseDown:
                    case EventType.MouseDrag:
                        Tools.current = Tool.None;
                        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                        PlaceBlock();
                        break;
                }
            }
            
        }
        
    }

    void PlaceBlock()
    {
        Vector3 tp = t.cursorPos;

        tp.x = Mathf.Floor(tp.x);
        if (tp.x % 2 != 0) tp.x--;

        tp.z = Mathf.Floor(tp.z);
        if (tp.z % 2 != 0) tp.z--;

        Vector2 ret = new Vector2(tp.x, tp.z);

        t.AddTile(ret);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (t.isActive)
        {
            if (GUILayout.Button("Close Editor"))
            {
                t.isActive = false;
            }
        } else
        {
            if (GUILayout.Button("Open Editor"))
            {
                t.isActive = true;
            }
        }

        if (GUILayout.Button("ClearLayer"))
        {
            t.ClearLayer(t.layer);
        }
    }
}
