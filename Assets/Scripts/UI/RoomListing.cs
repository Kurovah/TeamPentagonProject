using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour
{
    public string roomName;
    public TMP_Text roomNameDisplay;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        roomNameDisplay.text = roomName;
        button.onClick.AddListener(() => NetworkingManager.instance.JoinRoom(roomName));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
