using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Codice.Client.BaseCommands.BranchExplorer.Layout;

[CustomEditor(typeof(LevelBuilder))]
public class LevelBuilderEditor : Editor
{
    LevelBuilder t;

    //enum
    public enum TilePenMode : int
    {
        Draw,
        Erase
    }
    public TilePenMode currentTilePenMode;

    int stageItemIndex = 0;
    private bool shouldDraw = false;

    private void OnEnable()
    {
        t = (LevelBuilder)target;
        t.GetMeshFilter();
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

        Vector3 tp = t.cursorPos;

        tp.x = LevelBuilder.RoundTo(tp.x, 2);
        tp.z = LevelBuilder.RoundTo(tp.z, 2);

        Vector2 p = new Vector2(tp.x, tp.z);

        if (t.isActive)
        {
            
            //when clicking
            if(e.isMouse && e.button == 0)
            {
                Tools.current = Tool.None;
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                
                switch (e.type)
                {
                    case EventType.MouseUp:
                        break;
                    case EventType.MouseDown:
                        // toggle
                        shouldDraw = !shouldDraw;
                        break;
                    case EventType.MouseMove:
                        if(shouldDraw)
                            PerformTileAction(p);
                        break;
                }
            }
            e.Use();
            t.DrawCursor(p);
        }
    }

    void PerformTileAction(Vector2 pos)
    {
        switch (currentTilePenMode)
        {
            case TilePenMode.Draw:
                t.AddTile(pos);
                break;
            case TilePenMode.Erase:
                t.RemoveTile(pos);
                break;
        }
        EditorUtility.SetDirty(t);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if (t.isActive)
        {
            GUILayout.Label("Pen Mode");
            currentTilePenMode = (TilePenMode)GUILayout.SelectionGrid((int)currentTilePenMode, TilePenMode.GetNames(typeof(TilePenMode)), 2);

            #region layer related
            GUILayout.Label("Layer Stats");
            GUILayout.Label($"Current layer:{t.layer}");
            GUILayout.Label($"Tiles On Layer:{t.GetTileTransforms().Count}");

            if (GUILayout.Button("Clear Layer"))
            {
                t.ClearLayer(t.layer);
            }

            if (GUILayout.Button("Refresh Layer"))
            {
                t.RefreshLayer();
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button('\u2B06'.ToString()))
            {
                t.ChangeLayer(1);
            }

            if (GUILayout.Button('\u2B63'.ToString()))
            {
                t.ChangeLayer(-1);
            }
            GUILayout.EndHorizontal();
            #endregion



            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

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

        if (GUILayout.Button("Save Configuration"))
        {
            EditorUtility.SetDirty(t);
        }
    }
}
