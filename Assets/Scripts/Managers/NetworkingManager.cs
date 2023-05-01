using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
    public static NetworkingManager instance;
    public int team;
    bool joiningRoom, isReady;

    public UnityAction joinedRoomAction, leftRoomAction, onTeamSet;
    public UnityAction<bool> onReadySet;
    public UnityAction<List<RoomInfo>> onRoomListUpdated;

    [HideInInspector]
    public List<RoomInfo> rooms = new List<RoomInfo>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
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
        GameManager.instance.LoadNewScenewithFade("TestScene");
    }

    public void CheckAllPlayersReady()
    {
        
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void SetTeam(int i)
    {
        team = i;
        onTeamSet?.Invoke();
    }

    public void SetReady(bool ready)
    {
        isReady = ready;
        onReadySet?.Invoke(ready);
    }
}
