using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.instance.actions.UI.Proceed.ReadValue<float>() > 0)
        {
            Debug.Log("proceeding");
            GameManager.instance.LoadNewScenewithFade("MainMenu");
        }

        
    }
}
