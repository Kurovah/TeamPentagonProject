using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrabbable : MonoBehaviour, IGrabbable 
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
        rb.useGravity = false;
        transform.position += Vector3.up;
    }

    public void OnInput(Vector2 input)
    {
        velocity = new Vector3(input.x, 0, input.y);
    }

    public void OnReleased()
    {
        isGrabbed= false;
        velocity = Vector3.zero;
        rb.useGravity = true;
    }
}
