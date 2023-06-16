using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
    public static NetworkingManager instance;
    public int team;
    bool joiningRoom, isReady;

    public UnityAction joinedRoomAction, leftRoomAction;
    public UnityAction<List<RoomInfo>> onRoomListUpdated;
    Hashtable playerProperties;
   [HideInInspector]
    public List<RoomInfo> rooms = new List<RoomInfo>();

    private void Awake()
    {
        Pun2LocalConnector.Setup();
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //GoOffline();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TryConnect()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void TryCreateRoom(string roomName)
    {
        if (roomName != "")
        {
            joiningRoom = true;
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            roomOptions.MaxPlayers = (byte)4;

            //set nickname
            PhotonNetwork.NickName = GameManager.instance.playerData.playerName;

            //joining own room
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        }
    }

    public void GoOffline()
    {
        PhotonNetwork.OfflineMode = true;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        rooms = roomList;
        onRoomListUpdated?.Invoke(roomList);
    }

    public void JoinRoom(string roomName)
    {
        joiningRoom = true;
        PhotonNetwork.NickName = GameManager.instance.playerData.playerName;
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        //setting custom properties
        playerProperties = new Hashtable();
        playerProperties.Add("playerTeam", 0);
        playerProperties.Add("isReady", false);
        playerProperties.Add("HeadItem", GameManager.instance.playerData.HeadGearSetting);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

        joinedRoomAction?.Invoke();
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        leftRoomAction?.Invoke();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        leftRoomAction?.Invoke();
    }
    public void GoToPlayArea()
    {
        Debug.Log("Entering Play Space");
        GameManager.instance.LoadNewScenewithFade("PlayScene");
    }

    public void CheckAllPlayersReady()
    {
        
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public int GetTeam()
    {
        return (int)playerProperties["playerTeam"];
    }
    public void SetTeam(int i)
    {
        playerProperties["playerTeam"] = i;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    public void SetReady(bool ready)
    {
        playerProperties["isReady"] = ready;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    public void BackToLobby()
    {
        SetReady(false);
        GameManager.instance.LoadNewScenewithFade("LobbyScene");
    }
}
