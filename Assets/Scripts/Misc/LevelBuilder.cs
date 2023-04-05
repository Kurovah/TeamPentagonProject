using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LevelBuilder : MonoBehaviour
{
    public bool isActive;
    [HideInInspector]
    public int layer;
    public TileSet ties;
    public GameObject tileObject;
    public Vector3  cursorPos;
    Dictionary<Vector2, GameObject> tilePlacements = new Dictionary<Vector2, GameObject>();

    MeshFilter meshFilter;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void GetMeshFilter()
    {
        meshFilter = GetComponent<MeshFilter>();
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
            int layerPos = (int)RoundTo(layer, 2);
            

            obj.transform.position = new Vector3(tilePos.x,layerPos, tilePos.y);
            obj.name = $"tile x:{tilePos.x}, y:{tilePos.y}";
            tilePlacements.Add(tilePos, obj);

            //set tile mesh and materials
            int sideIndex = GetSideIndex(tilePos);
            int objIndex;
            float r = 0;
            //set object index

            GetOIndexandRotation(sideIndex, out objIndex, out r);

            obj.GetComponent<MeshRenderer>().sharedMaterials = ties.tileObjects[objIndex].GetComponent<MeshRenderer>().sharedMaterials;
            obj.GetComponent<MeshFilter>().sharedMesh = ties.tileObjects[objIndex].GetComponent<MeshFilter>().sharedMesh;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.Rotate(Vector3.up, r);
        }

        UpdateAllTiles();
         
    }

    void GetOIndexandRotation(int sideIndex ,out int objIndex, out float r)
    {
        switch (sideIndex)
        {
            //single edge
            case 1: objIndex = 1; r = 180; break;
            case 2: objIndex = 1; r = 90; break;
            case 4: objIndex = 1; r = 0; break;
            case 8: objIndex = 1; r = -90; break;

            //double edge
            case 5: objIndex = 2; r = 90; break;
            case 10: objIndex = 2; r = 0; break;

            //triple edge
            case 7: objIndex = 3; r = 90; break;
            case 14: objIndex = 3; r = 0; break;
            case 13: objIndex = 3; r = -90; break;
            case 11: objIndex = 3; r = 180; break;

            //4 edge
            case 15:
                objIndex = 4; r = 0;
                break;

            //corner
            case 3: objIndex = 5; r = 90; break;
            case 6: objIndex = 5; r = 0; break;
            case 12: objIndex = 5; r = -90; break;
            case 9: objIndex = 5; r = 180; break;

            default:
                objIndex = 0; r = 0;
                break;
        }
    }

    void UpdateAllTiles()
    {
        foreach(KeyValuePair<Vector2, GameObject> t in tilePlacements)
        {
            int sideIndex = GetSideIndex(t.Key);
            int oIndex;
            float rot;
            GetOIndexandRotation(sideIndex, out oIndex, out rot);

            t.Value.GetComponent<MeshRenderer>().sharedMaterials = ties.tileObjects[oIndex].GetComponent<MeshRenderer>().sharedMaterials;
            t.Value.GetComponent<MeshFilter>().sharedMesh = ties.tileObjects[oIndex].GetComponent<MeshFilter>().sharedMesh;
            t.Value.transform.localRotation = Quaternion.identity;
            t.Value.transform.Rotate(Vector3.up, rot);
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
            for (int i = GetLayer(layer).transform.childCount; i > 0; --i)
            {
                DestroyImmediate(GetLayer(layer).transform.GetChild(0).gameObject);
            }
                
        }
    }

    public void GetAllTilesOnLayer()
    {
        tilePlacements.Clear();
        if (LayerExists(layer))
        {
            var l = GetLayer(layer).transform;

            for (int i = 0; i < l.childCount; i++)
            {
                var t = l.GetChild(i);

                Vector2 v = new Vector2(t.position.x, t.position.z) * 100;
                GameObject g = t.gameObject;
                tilePlacements.Add(v, g);
            }
        }
    }

    public void ChangeLayer(int increment)
    {
        layer += increment;
        if(LayerExists(layer))
            GetAllTilesOnLayer();
    }

    public void DrawCursor(Vector2 pos)
    {
        if(meshFilter != null)
        {

            Mesh m;

            //if it has no mesh, create a new one
            if (meshFilter.sharedMesh == null)
            {
                m = new Mesh();
                meshFilter.sharedMesh = m;
                m.name = "Cursor";
            }

            m = meshFilter.sharedMesh;

            //update the mesh verticies
            float size = 1.0f;
            List<Vector3> v = new List<Vector3>
            {
                new Vector3(pos.x - size, layer, pos.y - size),
                new Vector3(pos.x + size, layer, pos.y - size),
                new Vector3(pos.x + size, layer, pos.y + size),
                new Vector3(pos.x - size, layer, pos.y + size),
                
            };

            List<int> tris = new List<int>
            {
                0,2,1,
                0,3,2
            };

            m.SetVertices(v);
            m.SetTriangles(tris, 0);

            meshFilter.sharedMesh = m;

            
        }
    }

    public static float RoundTo(float value, float multipleOf)
    {
        return Mathf.Round(value / multipleOf) * multipleOf;
    }
}
