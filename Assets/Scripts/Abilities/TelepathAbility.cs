using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelepathAbility : MonoBehaviourPunCallbacks, IRangerAbility
{
    public List<GameObject> grabbables;
    public GameObject grabTarget;
    bool abilityActive = false; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (abilityActive)
        {
            Vector2 InputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            OnInput(InputAxis);
        }
    }


    [PunRPC]
    public void OnAbilityStart()
    {
        Debug.Log("Starting ability");
        //don't do anything
        if (grabbables == null || grabbables.Count <= 0)
        {
            Debug.Log("No Targets");
            return;
        }
            

        grabTarget = grabbables[0];

        foreach (GameObject grabbable in grabbables)
        {
            if (Vector3.Distance(transform.position, grabbable.transform.position) < Vector3.Distance(transform.position, grabTarget.transform.position))
            {
                grabTarget = grabbable;
            }
        }

        if (grabTarget!= null)
        {
            grabTarget.GetComponent<IGrabbable>().OnGrabbed();
            abilityActive = true;
        }

        if(photonView.IsMine)
        {
            photonView.RPC("OnAbilityStart", RpcTarget.OthersBuffered);
        }
            
    }

    [PunRPC]
    public void OnAbilityEnd()
    {
        if (abilityActive && grabTarget != null)
        {
            grabTarget.GetComponent<IGrabbable>().OnReleased();
            grabTarget = null;
            abilityActive = false;
        }


        if (photonView.IsMine)
        {
            photonView.RPC("OnAbilityEnd", RpcTarget.OthersBuffered);
        }
    }

    
    public void OnInput(Vector2 input)
    {
        if(grabTarget != null)
        {
            grabTarget.GetComponent<IGrabbable>().OnInput(input);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<IGrabbable>() != null)
        {
            grabbables.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //remove item from list
        if(grabbables.Contains(other.gameObject)) { grabbables.Remove(other.gameObject); }  

        //remove target and release
        if(grabTarget == other.gameObject) { grabTarget.GetComponent<IGrabbable>().OnReleased(); grabTarget = null; }
    }
}
