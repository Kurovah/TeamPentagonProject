using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_Text currency_text;
    public Transform headGearPlace;
    // Start is called before the first frame update
    void Start()
    {
        AddButtonDestinations();
        PlaceHeadGear(GameManager.instance.playerData.HeadGearSetting);
    }

    // Update is called once per frame
    void Update()
    {
        currency_text.text = GameManager.instance.playerData.medals.ToString();
    }

    public void ScaleButton(RectTransform buttonTransform, Vector2 newScale)
    {
        buttonTransform.localScale = newScale;
    }

    public void GotoTestRoom()
    {
        GameManager.instance.LoadNewScenewithFade("TestMap");
    }

    public void AddButtonDestinations()
    {
        for (int i = 0; i < 5; i++)
        {
            var b = transform.GetChild(i).gameObject.GetComponent<Button>();
            var d = transform.GetChild(i).gameObject.GetComponent<ButtonDestinationComponent>();
            b.onClick.AddListener(() => GameManager.instance.LoadNewScenewithFade(d.sceneToLoad));

            //for connecting to lobby when you do quick play
            if (i == 0)
                b.onClick.AddListener(() => NetworkingManager.instance.TryConnect());
        }
    }

    void PlaceHeadGear(int index)
    {
        if (headGearPlace.childCount > 0)
        {
            Destroy(headGearPlace.GetChild(0).gameObject);
        }

        if (GameManager.instance.headGear[index] != null)
        {
            var i = Instantiate(GameManager.instance.headGear[index], headGearPlace);
            i.transform.localScale = Vector3.one * 0.1f;
        }

    }
}
