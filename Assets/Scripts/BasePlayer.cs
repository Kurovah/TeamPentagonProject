using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    CharacterController character;
    Vector3 velocity, actualVelocity;
    public float jumpheight, movespeed, gravity;
    float lastYVel;
    bool grounded;
    public Transform meshTransform;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, 0.01f, LayerMask.GetMask("Solid"));
        Vector3 rightVec = Vector3.Cross(-Camera.main.transform.forward, Vector3.up);
        Vector3 forwardVec = Vector3.Cross(rightVec, Vector3.up);

        Vector2 inputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        velocity = rightVec * inputAxis.x  + forwardVec * inputAxis.y;
        velocity  = velocity.normalized * movespeed * Time.deltaTime;
        velocity.y = lastYVel;

        

        if (grounded)
        {
            velocity.y = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = jumpheight * Time.fixedDeltaTime;
            }
                
        }



        velocity.y -= gravity * Time.deltaTime;
        actualVelocity = Vector3.Lerp(actualVelocity, velocity, 0.1f);
        actualVelocity.y = velocity.y;
        character.Move(actualVelocity);

        if (inputAxis.magnitude > 0)
            SetModelFacing(velocity);

        lastYVel = velocity.y;
    }

    void SetModelFacing(Vector3 _facing)
    {
        _facing.y = 0;
        meshTransform.forward = Vector3.Slerp(meshTransform.forward, _facing, 0.1f);
    }
}
