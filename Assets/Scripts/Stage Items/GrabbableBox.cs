using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrabbableBox : MonoBehaviourPunCallbacks, IGrabbable 
{
    public bool isGrabbed;
    Rigidbody rb;
    Vector3 velocity;
    public float dragSpeed; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if(isGrabbed)
            rb.velocity = velocity * dragSpeed;
    }

    public void OnGrabbed()
    {
        isGrabbed = true;
        SetPosition(transform.position + Vector3.up);
        SetUsingGrav(false);
    }

    public void OnInput(Vector2 input)
    {
        SetVelocity(new Vector3(input.x, 0, input.y));
    }

    public void OnReleased()
    {
        isGrabbed= false;
        SetVelocity(Vector3.zero);
        SetUsingGrav(true);
    }

    [PunRPC]
    void SetVelocity(Vector3 _velocity)
    {
        velocity = _velocity;

        if(photonView.IsMine)
        {
            photonView.RPC("SetVelocity", RpcTarget.OthersBuffered, _velocity);
        }
    }

    [PunRPC]
    void SetPosition(Vector3 _position)
    {
        transform.position = _position;

        if (photonView.IsMine)
        {
            photonView.RPC("SetPosition", RpcTarget.OthersBuffered, _position);
        }
    }

    [PunRPC]
    void SetUsingGrav(bool value)
    {
        rb.useGravity = value;

        if (photonView.IsMine)
        {
            photonView.RPC("SetUsingGrav", RpcTarget.OthersBuffered, value);
        }
    }
}
