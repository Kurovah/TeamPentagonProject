using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PlayerListingBehaviour : MonoBehaviourPunCallbacks
{
    public TMP_Text playerName;
    public Transform readySignal;
    // Start is called before the first frame update
    void Start()
    {
        playerName.text = PhotonNetwork.NickName;
    }

    private void OnDestroy()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetBarColor(Color newCol)
    {
        GetComponent<Image>().color = newCol;
    }

    public void SetReadySignal(bool ready)
    {

        readySignal.GetChild(0).gameObject.SetActive(!ready);
        readySignal.GetChild(1).gameObject.SetActive(ready);
    }
}
