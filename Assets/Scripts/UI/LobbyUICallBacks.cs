using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class LobbyUICallBacks : MonoBehaviourPunCallbacks
{
    public Transform roomMaker,roomJoiner,teamSelector,listingArea, roomListingArea;
    public TMP_InputField roomNameField;

    public TMP_Text tally;

    int playersInRoom;
    int ReadiedPlayers;

    public GameObject ownPlayerListing;

    [Header("Prefabs")]
    public GameObject playerListingObject, roomListingObject;

    // Start is called before the first frame update
    void Start()
    {
        NetworkingManager.instance.joinedRoomAction += OnRoomJoined;
        NetworkingManager.instance.joinedRoomAction += IncrementPlayers;
        NetworkingManager.instance.leftRoomAction += DecrementPlayers;
        NetworkingManager.instance.onRoomListUpdated += OnRoomListUpdate;

        GameManager.instance.FadeIn();

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
        NetworkingManager.instance.SetReady(true);
        CheckAllReady();
    }

    public void UnRUCallback()
    {
        NetworkingManager.instance.SetReady(false);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        PlayerListUpdate();
        CheckAllReady();
    }

    void CheckAllReady()
    {
        //just to make sure one player doesn't go in by themselves
        //if (ReadiedPlayers == playersInRoom && playersInRoom > 1)
        int readyPlayer = 0;
        int playerCount = PhotonNetwork.CurrentRoom.Players.Count;
        Debug.Log($"Players in room:{playerCount}");

        for (int i = 0; i < PhotonNetwork.CurrentRoom.Players.Count; i++)
        {
            Player p = PhotonNetwork.CurrentRoom.Players.ElementAt(i).Value;
            if ((bool)p.CustomProperties["isReady"])
            {
                Debug.Log("This player is ready");
                readyPlayer++;
            }
                
        }

        if (readyPlayer == playerCount)
        {
            Debug.Log("All ready");
            NetworkingManager.instance.GoToPlayArea();
        }
            
    }
    #endregion


    public void SwitchTeams()
    {
        if (NetworkingManager.instance.GetTeam() == 0)
            NetworkingManager.instance.SetTeam(1);
        else
            NetworkingManager.instance.SetTeam(0);
    }

    public void OnRoomJoined()
    {
        roomMaker.gameObject.SetActive(false);
        roomJoiner.gameObject.SetActive(false);
        teamSelector.gameObject.SetActive(true);

        //update the player list when you join
        PlayerListUpdate();
        
    }

    [PunRPC]
    public void PlayerListUpdateRPC()
    {
        //remove all children
        for(int i = 0; i < listingArea.childCount; i++)
        {
            Destroy(listingArea.GetChild(i).gameObject);
        }

        //create listings
        
        for(int i = 0; i < PhotonNetwork.CurrentRoom.Players.Count; i++)
        {
            Player p = PhotonNetwork.CurrentRoom.Players.ElementAt(i).Value;
            GameObject a = Instantiate(playerListingObject, Vector3.zero, Quaternion.identity);
            a.transform.parent = listingArea;

            //getting properties to set the bar
            a.GetComponent<PlayerListingBehaviour>().SetBarColor(
                (int)p.CustomProperties["playerTeam"] == 0? Color.red : Color.blue
                );

            Debug.Log($"{p.NickName}'s team is:{(int)p.CustomProperties["playerTeam"]}");

            a.GetComponent<PlayerListingBehaviour>().SetReadySignal(
                (bool)p.CustomProperties["isReady"]
                );

            if (p == PhotonNetwork.LocalPlayer)
            {
                Debug.Log("My Player");
                ownPlayerListing = a;
            }
        }
    }

    
    

    public void PlayerListUpdate()
    {
        photonView.RPC("PlayerListUpdateRPC", RpcTarget.AllBuffered);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
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

    public void LeaveRoom()
    {
        NetworkingManager.instance.LeaveRoom();
    }

    public void BackToMain()
    {
        GameManager.instance.LoadNewScenewithFade("MainMenu");
    }

}
