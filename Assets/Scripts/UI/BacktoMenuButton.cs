using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BacktoMenuButton : MonoBehaviour
{
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => GameManager.instance.LoadNewScenewithFade("MainMenu"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
