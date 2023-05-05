using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelBuilder : MonoBehaviour
{
    public bool isActive;
    [HideInInspector]
    public int layer;

    [Header("Scriptable objects")]
    public TileSet tiles;
    public StageItems stageItems;
    public GameObject tileObject;

    [Header("Level Stuff")]
    [HideInInspector]
    public Vector3  cursorPos;
    public Vector3  EndPointOffset;
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

    #region tile functions
    public void AddTile(Vector2 tilePos)
    {
        //check if the layer exists
        if (!LayerExists(layer))
        {
            //layer transform
            GameObject g = new GameObject();
            g.name = $"Layer_{layer}";
            g.transform.localScale = Vector3.one * 100;
            g.transform.parent = transform;
            g.transform.position = new Vector3(0, layer * 2, 0);
        }

        //create tile if one isn't in this pos
        GameObject obj;
        if (IsAreaObstructed(tilePos))
        {
            obj = tileObject;
        } else
        {
            obj = Instantiate(tileObject, GetLayer(layer).transform);
            obj.transform.localPosition = new Vector3(tilePos.x / 100, 0, tilePos.y / 100);
            obj.name = $"tile x:{tilePos.x}, y:{tilePos.y}";
        }
        

        //set tile mesh and materials
        int sideIndex = GetSideIndex(tilePos);
        int objIndex;
        float r;

        //set object index and rotation
        GetOIndexandRotation(sideIndex, out objIndex, out r);
        obj.GetComponent<MeshRenderer>().sharedMaterials = tiles.tileObjects[objIndex].GetComponent<MeshRenderer>().sharedMaterials;
        obj.GetComponent<MeshFilter>().sharedMesh = tiles.tileObjects[objIndex].GetComponent<MeshFilter>().sharedMesh;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.Rotate(Vector3.up, r);

        // UpdateAllTiles();
         
    }
    public void RemoveTile(Vector2 tilePos)
    {
        var tt = GetTileTransforms();
        foreach (Transform t in tt)
        {

            //if the tile is found remove it
            Vector2 checkedTilePos = new Vector2(t.position.x, t.position.z);
            if (checkedTilePos == tilePos)
            {
                DestroyImmediate(t.gameObject);
                UpdateAllTiles();
                break;
            }
        }
    }
    public void RefreshLayer()
    {
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
        //make sure it updates properly
        var tt = GetTileTransforms();

        foreach(Transform t in tt)
        {
            var g = t.gameObject;
            Vector2 tpos = new Vector2(g.transform.position.x, g.transform.position.z);

            int sideIndex = GetSideIndex(tpos);
            int oIndex;
            float rot;
            GetOIndexandRotation(sideIndex, out oIndex, out rot);

            t.gameObject.GetComponent<MeshRenderer>().sharedMaterials = tiles.tileObjects[oIndex].GetComponent<MeshRenderer>().sharedMaterials;
            t.gameObject.GetComponent<MeshFilter>().sharedMesh = tiles.tileObjects[oIndex].GetComponent<MeshFilter>().sharedMesh;
            t.gameObject.transform.localRotation = Quaternion.identity;
            t.gameObject.transform.Rotate(Vector3.up, rot);
        }

        
    }
    int GetSideIndex(Vector2 tilePos)
    {
        int ret = 0;
        var list = GetTileTransforms();


        //for each trasform check if it is near this tile and change side index accordingly
        foreach (Transform t in list)
        {
            //tile posisiton
            Vector2 p = new Vector2(t.position.x, t.position.z);
            if (tilePos + Vector2.up * 2 == p) ret += 1;
            if (tilePos + Vector2.left * 2 == p) ret += 2;
            if (tilePos - Vector2.up * 2 == p) ret += 4;
            if (tilePos - Vector2.left * 2 == p) ret += 8;

        }

        
        return ret;
    }
    public List<Transform> GetTileTransforms()
    {

        List<Transform> ret = new List<Transform>();
        if (LayerExists(layer))
        {
            foreach (Transform t in GetLayer(layer).transform)
            {
                ret.Add(t);
            }
        }
        return ret;
    }
    public int GetTileNumber()
    {
        if (LayerExists(layer))
        {
            var l = GetLayer(layer).transform;

            return l.childCount;
        }

        return 0;
    }
    #endregion
    #region layer functions
    bool LayerExists(int layer)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            var t = transform.GetChild(i);
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
                var p = GetLayer(layer).transform.GetChild(0).position;
                RemoveTile(new Vector2(p.x, p.z));
            }
                
        }
    }
    #endregion

    public void ChangeLayer(int increment)
    {
        layer += increment;
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
                new Vector3(pos.x - size - transform.position.x, layer, pos.y - size - transform.position.z) + Vector3.up * 0.01f,
                new Vector3(pos.x + size - transform.position.x, layer, pos.y - size - transform.position.z) + Vector3.up * 0.01f,
                new Vector3(pos.x + size - transform.position.x, layer, pos.y + size - transform.position.z) + Vector3.up * 0.01f,
                new Vector3(pos.x - size - transform.position.x, layer, pos.y + size - transform.position.z) + Vector3.up * 0.01f,

            };

            List<int> tris = new List<int>
            {
                0,2,1,
                0,3,2
            };

            m.SetVertices(v);
            m.SetTriangles(tris, 0);
            m.RecalculateBounds();

            meshFilter.sharedMesh = m;

            
        }
    }

    public Vector3 GetLevelEnd()
    {
        return transform.position + EndPointOffset;
    }

    bool IsAreaObstructed(Vector2 pos)
    {
        //checking tiles
        foreach(Transform t in GetLayer(layer).transform)
        {
            Vector2 p = new Vector2(t.position.x, t.position.z);
            if (p == pos) return true;
        }
        return false;
    }

    public static float RoundTo(float value, float multipleOf)
    {
        return Mathf.Round(value / multipleOf) * multipleOf;
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.DrawWireSphere(transform.position + EndPointOffset, 1);
    }
}
