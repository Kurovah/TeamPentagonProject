using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class AlienBehaviour : MonoBehaviourPunCallbacks
{
    public float moveSpeed;
    Vector3 velocity;
    Rigidbody rb;
    public Transform meshTransform;
    public GameObject critterObject;
    public enum CharacterStates
    {
        Normal,
        Selecting
    }
    public CharacterStates characterState;

    public Dictionary<ResourceTypes, int> resources = new Dictionary<ResourceTypes, int>
    {
        { ResourceTypes.Destron, 0},
        { ResourceTypes.Anima, 0},
        { ResourceTypes.Aquidia, 0},
    };
    List<ResourceSpot> resourceSpots = new List<ResourceSpot>();
    Coroutine siphonCR;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            switch (characterState)
            {
                case CharacterStates.Normal:
                    NormalState();
                    break;
                case CharacterStates.Selecting:

                break;
            }

            rb.velocity = velocity;
        }
    }

    void NormalState()
    {
        Vector3 rightVec = Vector3.Cross(-Camera.main.transform.forward, Vector3.up);
        Vector3 forwardVec = Vector3.Cross(rightVec, Vector3.up);
        Vector2 inputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        velocity = rightVec * inputAxis.x + forwardVec * inputAxis.y;
        velocity = velocity.normalized * moveSpeed;

        if (inputAxis.magnitude > 0)
            SetModelFacing(new Vector3(inputAxis.x, 0, inputAxis.y));

        if (Input.GetKeyDown(KeyCode.J))
        {
            StartSiphon();
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            EndSiphon();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SummonCritter();
        }
    }

    void SummonCritter()
    {
        RaycastHit place;
        if(HasResources(new Dictionary<ResourceTypes, int> { 
            { ResourceTypes.Destron , 1} 
        }) && 
        Physics.Raycast(meshTransform.position, Vector3.down, out place,Mathf.Infinity,LayerMask.GetMask("Solid")))
        {
            Instantiate(critterObject, place.point, Quaternion.identity);
            resources[ResourceTypes.Destron] -= 1;
        }
    }

    void StartSiphon()
    {
        if (siphonCR != null)
            StopCoroutine(siphonCR);

        siphonCR = StartCoroutine(Siphon());
    }

    void EndSiphon()
    {
        StopCoroutine(siphonCR);
    }
    
    IEnumerator Siphon()
    {
        if(resourceSpots.Count > 0)
        {
            for(int i = 0; i < resourceSpots.Count; i++)
            {
                resources[resourceSpots[i].resource] += 1;
            }
        }

        yield return new WaitForSeconds(1);
        siphonCR = StartCoroutine(Siphon());
    }

    bool HasResources(Dictionary<ResourceTypes, int> _resourceCheck)
    {
        foreach(KeyValuePair<ResourceTypes, int> p in _resourceCheck)
        {
            if (resources[p.Key] < p.Value)
                return false;
        }

        return true;
    }

    void SetModelFacing(Vector3 _facing)
    {
        _facing.y = 0;
        meshTransform.forward = Vector3.Slerp(meshTransform.forward, _facing, 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        var r = other.gameObject.GetComponent<ResourceSpot>();
        if (r != null) resourceSpots.Add(r);
    }

    private void OnTriggerExit(Collider other)
    {
        var r = other.gameObject.GetComponent<ResourceSpot>();
        if (r != null) resourceSpots.Remove(r);
    }
}
