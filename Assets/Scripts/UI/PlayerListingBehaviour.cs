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
        NetworkingManager.instance.onReadySet += SetReadySignal;
        NetworkingManager.instance.onTeamSet += SetBarColor;

        SetBarColor();
    }

    private void OnDestroy()
    {
        NetworkingManager.instance.onReadySet -= SetReadySignal;
        NetworkingManager.instance.onTeamSet -= SetBarColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetBarColor()
    {
        if (photonView.IsMine)
        {
            var i = GetComponent<Image>().color =
            NetworkingManager.instance.team == 0 ? Color.red : Color.cyan;
        }
    }

    void SetReadySignal(bool ready)
    {
        if (photonView.IsMine)
        {
            readySignal.GetChild(0).gameObject.SetActive(!ready);
            readySignal.GetChild(1).gameObject.SetActive(ready);
        }
    }
}
