using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LevelBuilder : MonoBehaviour
{
    public bool isActive;
    public int layer, lastLevel;
    public TileSet ties;
    public GameObject tileObject;
    public Vector3  cursorPos;
    Dictionary<Vector2, GameObject> tilePlacements = new Dictionary<Vector2, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnRenderObject()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddTile(Vector2 tilePos)
    {
        //check if the layer exists
        if (!LayerExists(layer))
        {
            GameObject g = new GameObject();
            g.name = $"Layer_{layer}";
            g.transform.localScale = Vector3.one * 100;
            g.transform.parent = transform;
        }

        //create tile
        if (!tilePlacements.ContainsKey(tilePos))
        {
            var obj = Instantiate(tileObject, GetLayer(layer).transform);
            obj.transform.position = new Vector3(tilePos.x,layer, tilePos.y);
            obj.name = $"tile x:{tilePos.x}, y:{tilePos.y}";
            tilePlacements.Add(tilePos, obj);

            //set tile mesh and materials
            int sideIndex = GetSideIndex(tilePos);
            int objIndex;
            float r = 0;
            //set object index
            switch (sideIndex)
            {
                //single edge
                case 1: objIndex = 1; r = 180; break;
                case 2: objIndex = 1; r = 90; break;
                case 4: objIndex = 1; r = 0; break;
                case 8: objIndex = 1; r = -90; break;

                //double edge
                case 5: objIndex = 2; r = 180; break;
                case 10: objIndex = 2; r = 0;  break;
                
                //triple edge
                case 7: objIndex = 3; r = 0; break;
                case 14: objIndex = 3; r = 0; break;
                case 13: objIndex = 3; r = 0; break;
                case 11:objIndex = 3; r = 180; break;

                //4 edge
                case 15:
                    objIndex = 4;
                    break;

                //triple edge
                case 3:
                case 6:
                case 12:
                case 9:
                    objIndex = 5;
                    break;

                default:
                    objIndex = 0;
                    break;
            }

            Debug.Log("s" + sideIndex);
            obj.GetComponent<MeshRenderer>().sharedMaterials = ties.tileObjects[objIndex].GetComponent<MeshRenderer>().sharedMaterials;
            obj.GetComponent<MeshFilter>().sharedMesh = ties.tileObjects[objIndex].GetComponent<MeshFilter>().sharedMesh;
            obj.transform.Rotate(Vector3.up, r);
        }
        
    }

    int GetSideIndex(Vector2 tilePos)
    {
        int ret = 0;
        if (tilePlacements.ContainsKey(tilePos + Vector2.up * 2)) ret += 1;
        if (tilePlacements.ContainsKey(tilePos + Vector2.left * 2)) ret += 2;
        if (tilePlacements.ContainsKey(tilePos + Vector2.down * 2)) ret += 4;
        if (tilePlacements.ContainsKey(tilePos + Vector2.right * 2)) ret += 8;
        return ret;
    }

    bool LayerExists(int layer)
    {
        foreach(Transform t in transform)
        {
            if (t.gameObject.name == $"Layer_{layer}")
                return true;
        }
        return false;
    }

    GameObject GetLayer(int layer)
    {
        foreach (Transform t in transform)
        {
            if (t.gameObject.name == $"Layer_{layer}")
                return t.gameObject;
        }

        return null;
    }

    public void ClearLayer(int layer)
    {
        if (LayerExists(layer))
        {
            foreach (Transform t in GetLayer(layer).transform)
            {
                DestroyImmediate(t.gameObject);
            }
        }
    }

    public void GetAllTilesOnLayer()
    {
        tilePlacements = new Dictionary<Vector2, GameObject>();
        foreach (Transform t in GetLayer(layer).transform)
        {
            Vector2 v = new Vector2(t.position.x, t.position.z) * 100;
            GameObject g = t.gameObject;
            tilePlacements.Add(v, g);
        }
    }

    private void OnDrawGizmos()
    {
        if (isActive)
        {
            Gizmos.DrawCube(cursorPos, Vector3.one);
        }
            
    }
}
