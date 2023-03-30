using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class LevelBuilder : MonoBehaviour
{
    public bool isActive;
    public int level, lastLevel;
    public TileSet ties;
    Plane tilePlane;
    Vector3 cursorPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return;

        if (lastLevel != level)
            tilePlane = new Plane(Vector3.up, level);

        float distance;


        Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(Input.mousePosition);
        if (tilePlane.Raycast(ray, out distance))
        {
            cursorPos = ray.GetPoint(distance);
        }
        Debug.Log(cursorPos);
        lastLevel = level;
    }



    private void OnDrawGizmos()
    {
        
    }
}
