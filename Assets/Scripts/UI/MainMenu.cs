using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        AddButtonDestinations();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ScaleButton(RectTransform buttonTransform, Vector2 newScale)
    {
        buttonTransform.localScale = newScale;
    }

    public void AddButtonDestinations()
    {
        for (int i = 0; i < 4; i++)
        {
            var b = transform.GetChild(i).gameObject.GetComponent<Button>();
            var d = transform.GetChild(i).gameObject.GetComponent<ButtonDestinationComponent>();
            b.onClick.AddListener(() => GameManager.instance.LoadNewScenewithFade(d.sceneToLoad));
        }
    }
}
