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
    public enum CharacterStates
    {
        Normal,
        Selecting
    }
    public CharacterStates characterState;
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
    }
    void SetModelFacing(Vector3 _facing)
    {
        _facing.y = 0;
        meshTransform.forward = Vector3.Slerp(meshTransform.forward, _facing, 0.1f);
    }
}
