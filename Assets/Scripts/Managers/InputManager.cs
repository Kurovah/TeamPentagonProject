using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public InputActionAsset actions;
    // Start is called before the first frame update
    void Start()
    {
        instance = this; 
    }

}
