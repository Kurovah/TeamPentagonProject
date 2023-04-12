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
    public enum BasePenMode:int
    {
        Tile,
        Object,
    }
    public enum TilePenMode : int
    {
        Draw,
        Erase
    }
    public enum ObjectPenMode: int
    {
        Place,
        Rotate,
        Remove
    }
    public BasePenMode currentBasePenMode;
    public TilePenMode currentTilePenMode;
    public ObjectPenMode currentObjPenMode;

    int stageItemIndex = 0;

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
                    case EventType.MouseDown:
                    case EventType.MouseDrag:
                        switch (currentBasePenMode)
                        {
                            case BasePenMode.Tile:
                                PerformTileAction(p);
                                break;
                            case BasePenMode.Object:
                                PerformObjectAction(p);
                                break;
                        }
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

    void PerformObjectAction(Vector2 pos)
    {
        switch (currentObjPenMode)
        {
            case ObjectPenMode.Place:
                t.PlaceObject(stageItemIndex,pos);
                break;
            case ObjectPenMode.Rotate:
                break;
            case ObjectPenMode.Remove:
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
            #region for drawmode
            GUILayout.Label("Pen Mode");
            currentBasePenMode = (BasePenMode)GUILayout.SelectionGrid((int)currentBasePenMode, BasePenMode.GetNames(typeof(BasePenMode)), 2);
            switch (currentBasePenMode)
            {
                case BasePenMode.Tile:
                    currentTilePenMode = (TilePenMode)GUILayout.SelectionGrid((int)currentTilePenMode, TilePenMode.GetNames(typeof(TilePenMode)), 2);
                    break;
                case BasePenMode.Object:
                    currentObjPenMode = (ObjectPenMode)GUILayout.SelectionGrid((int)currentObjPenMode, ObjectPenMode.GetNames(typeof(ObjectPenMode)), 3);
                    break;
            }
            #endregion

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

            //to select what object to place
            if (currentBasePenMode == BasePenMode.Object)
            {
                stageItemIndex = EditorGUILayout.Popup("Item", stageItemIndex, t.GetStageObjectNames().ToArray());
            }


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
