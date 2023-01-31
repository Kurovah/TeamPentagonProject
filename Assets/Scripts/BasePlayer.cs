using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    CharacterController character;
    Vector3 velocity;
    public float jumpheight, movespeed, gravity;
    bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, 0.01f, LayerMask.GetMask("Solid"));
        velocity.x = Input.GetAxis("Horizontal") * movespeed * Time.deltaTime;

        if (grounded)
        {
            velocity.y = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = jumpheight * Time.fixedDeltaTime;
            }
                
        }

        velocity.y -= gravity * Time.deltaTime;

        character.Move(velocity);
    }
}
