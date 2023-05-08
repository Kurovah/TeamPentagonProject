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
    public MeshRenderer bodyRenderer;
    public Transform arrowPivot;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        LockMovement();
    }

    void LockMovement()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
    }

    void UnlockMovement()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGrabbed()
    {
        arrowPivot.gameObject.SetActive(true);
        isGrabbed = true;
        UnlockMovement();
        bodyRenderer.sharedMaterial.SetInt("_FresOn", 1);
    }

    public void OnInput(Vector2 input)
    {
        SetVelocity(new Vector3(input.x, 0, input.y));
        arrowPivot.forward = new Vector3(input.x, 0, input.y);
        //SetPosition(transform.position + velocity * dragSpeed * Time.deltaTime);
    }

    public void OnReleased()
    {
        isGrabbed= false;
        SetVelocity(Vector3.zero);
        LockMovement();
        bodyRenderer.sharedMaterial.SetInt("_FresOn", 0);
        arrowPivot.gameObject.SetActive(false);
    }

    void SetVelocity(Vector3 _velocity)
    {
        rb.velocity = _velocity * dragSpeed;
    }

    void SetPosition(Vector3 _position)
    {
        transform.position = _position;
    }
}
