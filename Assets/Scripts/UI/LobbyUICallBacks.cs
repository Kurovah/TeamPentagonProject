using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyUICallBacks : MonoBehaviourPun
{
    public Transform roomMaker,roomJoiner,teamSelector,listingArea, roomListingArea;
    public TMP_InputField roomNameField;

    public TMP_Text tally;

    int playersInRoom;
    int ReadiedPlayers;

    public GameObject playerListingObject, roomListingObject;

    // Start is called before the first frame update
    void Start()
    {
        NetworkingManager.instance.joinedRoomAction += OnRoomJoined;
        NetworkingManager.instance.joinedRoomAction += IncrementPlayers;
        NetworkingManager.instance.leftRoomAction += DecrementPlayers;
        NetworkingManager.instance.onRoomListUpdated += OnRoomListUpdate;
    }

    private void OnDestroy()
    {
        NetworkingManager.instance.joinedRoomAction -= OnRoomJoined;
        NetworkingManager.instance.joinedRoomAction -= IncrementPlayers;
        NetworkingManager.instance.leftRoomAction -= DecrementPlayers;
        NetworkingManager.instance.onRoomListUpdated -= OnRoomListUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        tally.text = $"{ReadiedPlayers}/{playersInRoom}";
    }

    public void CreateRoom()
    {
        var t = roomNameField.text;
        Debug.Log("roomName:" + t);
        NetworkingManager.instance.TryCreateRoom(t);
    }

    public void JoinRoom(string roomName)
    {

    }

    public void JoinRoomRandom()
    {

    }

    public void SetTeam(int team)
    {
        NetworkingManager.instance.team = team;
    }


    #region player count
    [PunRPC]
    public void IncrementPlayers()
    {

        photonView.RPC("IPRPC", RpcTarget.AllBuffered);
    }

    public void DecrementPlayers()
    {
        photonView.RPC("DPRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void IPRPC()
    {
        playersInRoom++;
    }

    [PunRPC]
    public void DPRPC()
    {
        playersInRoom--;
    }
    #endregion

    #region readying players
    public void RUCallback()
    {
        photonView.RPC("ReadyUp", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void ReadyUp()
    {
        ReadiedPlayers++;
        NetworkingManager.instance.SetReady(true);
        CheckAllReady();
    }

    public void UnRUCallback()
    {
        photonView.RPC("UnReadyUp", RpcTarget.All);
    }

    [PunRPC]
    public void UnReadyUp()
    {
        ReadiedPlayers--;
        NetworkingManager.instance.SetReady(false);
    }

    void CheckAllReady()
    {
        //just to make sure one player doesn't go in by themselves
        //if (ReadiedPlayers == playersInRoom && playersInRoom > 1)
        if (ReadiedPlayers == playersInRoom)
            NetworkingManager.instance.GoToPlayArea();
    }
    #endregion


    public void SwitchTeams()
    {
        if (NetworkingManager.instance.team == 0)
            NetworkingManager.instance.SetTeam(1);
        else
            NetworkingManager.instance.SetTeam(0);
    }

    public void OnRoomJoined()
    {
        roomMaker.gameObject.SetActive(false);
        roomJoiner.gameObject.SetActive(false);
        teamSelector.gameObject.SetActive(true);
        photonView.RPC("AddListing", RpcTarget.AllBuffered);
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //clear old list
        foreach(Transform t in roomListingArea)
        {
            Destroy(t.gameObject);
        }


        //Add new entries
        foreach(var i in roomList)
        {
            var l = Instantiate(roomListingObject, roomListingArea);
            l.GetComponent<RoomListing>().roomName = i.Name;
        }
    }

    [PunRPC]
    public void AddListing()
    {
        var a = PhotonNetwork.Instantiate(playerListingObject.name, Vector3.zero, Quaternion.identity);
        a.transform.parent = listingArea;
    }

    public void LeaveRoom()
    {
        NetworkingManager.instance.LeaveRoom();
    }

    public void BackToMain()
    {
        GameManager.instance.LoadNewScenewithFade("MainMenu");
    }

}
