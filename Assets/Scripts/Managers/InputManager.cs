using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public TeamPentagonProject actions;
    // Start is called before the first frame update
    void Start()
    {
        actions = new TeamPentagonProject();
        actions.Enable();
        instance = this; 
    }

}
