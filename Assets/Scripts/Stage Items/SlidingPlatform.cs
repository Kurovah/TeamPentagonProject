using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPlatform : MonoBehaviourPunCallbacks, IGrabbable
{
    public bool isGrabbed;
    Rigidbody rb;
    Vector3 velocity;
    public float dragSpeed = 5;
    public Transform meshTransform;
    public enum AxisType
    {
        XAxis,
        ZAxis
    }
    public AxisType movementAxis = AxisType.XAxis;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetAxisLock(AxisType.XAxis);
    }

    // Update is called once per frame
    void Update()
    {

        if (isGrabbed)
            rb.velocity = velocity * dragSpeed;
    }

    public void OnGrabbed()
    {
        isGrabbed = true;
    }

    public void OnInput(Vector2 input)
    {
        var moveVec = new Vector3(input.x, 0, input.y);
        switch (movementAxis)
        {
            case AxisType.XAxis:
                moveVec.z = 0;
                break;
            case AxisType.ZAxis:
                moveVec.x = 0;
                break;
        }
        SetVelocity(moveVec);
    }

    public void OnReleased()
    {
        isGrabbed = false;
        SetVelocity(Vector3.zero);
    }

    [PunRPC]
    void SetVelocity(Vector3 _velocity)
    {
        velocity = _velocity;

        if (photonView.IsMine)
        {
            photonView.RPC("SetVelocity", RpcTarget.OthersBuffered, _velocity);
        }
    }

    public void SwitchAxis()
    {
        switch (movementAxis)
        {
            case AxisType.XAxis:
                SetAxisLock(AxisType.ZAxis);
                break;
            case AxisType.ZAxis:
                SetAxisLock(AxisType.XAxis);
                break;
        }
    }

    void SetAxisLock(AxisType newAxis)
    {
        movementAxis = newAxis;
        switch (movementAxis)
        {
            case AxisType.XAxis:
                meshTransform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case AxisType.ZAxis:
                meshTransform.rotation = Quaternion.Euler(0, 0, 0);
                break;
        }
    }

}
