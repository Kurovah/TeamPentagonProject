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
    public MeshRenderer bodyRenderer;
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
        SetAxisLock(movementAxis);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnGrabbed()
    {
        print("on platform");
        isGrabbed = true;
        bodyRenderer.sharedMaterial.SetInt("_FresOn", 1);
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
        print("off platform");
        isGrabbed = false;
        bodyRenderer.sharedMaterial.SetInt("_FresOn", 0);
        SetVelocity(Vector3.zero);
    }

    void SetVelocity(Vector3 _velocity)
    {
        rb.velocity = _velocity;
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
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll | ~RigidbodyConstraints.FreezePositionX;
                break;
            case AxisType.ZAxis:
                meshTransform.rotation = Quaternion.Euler(0, 0, 0);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll | ~RigidbodyConstraints.FreezePositionZ;
                break;
        }
    }

}
