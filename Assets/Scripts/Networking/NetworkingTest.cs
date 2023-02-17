using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkingTest : MonoBehaviourPunCallbacks
{

    List<RoomInfo> rooms = new List<RoomInfo>();
    string roomName = "Test Room";
    string playerName = "Test Player";
    bool joiningRoom = false;
    public GameObject playerPrefab, playerCam;

    Vector2 roomListScroll= Vector2.zero;
    bool showWindow = true;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsConnected)
        {

            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        rooms = roomList;
    }

    private void OnGUI()
    {
        if (showWindow)
        {
            GUI.Window(0, new Rect(Screen.width / 2 - 450,
                Screen.height / 2 - 200,
                900,
                400),
                LobbyWindow, "Lobby");
        }
    }

    void LobbyWindow(int index)
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("Status:" + PhotonNetwork.NetworkClientState);
        if(joiningRoom || !PhotonNetwork.IsConnected
            ||PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
        {
            GUI.enabled = false;
        }

        GUILayout.FlexibleSpace();

        roomName = GUILayout.TextField(roomName, GUILayout.Width(250));

        if(GUILayout.Button("Make new room", GUILayout.Width(125)))
        {
            if(roomName != "")
            {
                joiningRoom = true;
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.IsOpen = true;
                roomOptions.IsVisible = true;
                roomOptions.MaxPlayers = (byte)2;

                PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
            }
        }

        GUILayout.EndHorizontal();

        roomListScroll = GUILayout.BeginScrollView(roomListScroll, true, true);

        if(rooms.Count == 0)
        {
            GUILayout.Label("No Rooms Available");
        } else
        {
            for(int i = 0; i < rooms.Count; i++)
            {
                GUILayout.BeginHorizontal("Box");
                GUILayout.Label(rooms[i].Name, GUILayout.Width(400));
                GUILayout.Label(rooms[i].PlayerCount + "/" + rooms[i].MaxPlayers);

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Join"))
                {
                    joiningRoom= true;

                    PhotonNetwork.NickName = playerName;

                    PhotonNetwork.JoinRoom(rooms[i].Name);
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndScrollView();

        GUILayout.BeginHorizontal();

        GUILayout.Label("Player Name:", GUILayout.Width(85));

        playerName = GUILayout.TextField(playerName, GUILayout.Width(250));

        GUILayout.FlexibleSpace();

        GUI.enabled = (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby ||
            PhotonNetwork.NetworkClientState == ClientState.Disconnected) &&
            !joiningRoom ;
        if(GUILayout.Button("Refresh", GUILayout.Width(100)))
        {
            if(PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinLobby(TypedLobby.Default);
            } else
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        GUILayout.EndHorizontal();

        if(joiningRoom)
        {
            GUI.enabled = true;
            GUI.Label(new Rect(900 / 2 - 50, 400 / 2 - 10, 100, 20), "Connecting...");
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("JoiningLobby");
    }

    public override void OnJoinedRoom()
    {
        showWindow = false;
        var p = PhotonNetwork.Instantiate(playerPrefab.name,
            Vector3.zero,
            Quaternion.identity, 
            0);


        var cam = Instantiate(playerCam,
            Vector3.zero,
            Quaternion.identity);

        cam.GetComponent<CameraBehaviour>().target = p.transform;
    }
}
