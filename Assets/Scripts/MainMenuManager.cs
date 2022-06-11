using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField PlayerNameField; 
    public Button EnterLobbyBtn; 
    public GameObject MotorcycleSprite; 
    public GameObject SearchingText; 

    private const int MaxPlayersPerRoom = 2; 

    void Awake() => PhotonNetwork.AutomaticallySyncScene = true; 

    void EnterLobby() {
        string playerName = PlayerNameField.text; 
        if (playerName == "") {
            playerName = SystemInfo.deviceName; 
            PlayerNameField.text = playerName;
            PlayerNameField.interactable = false;
            Debug.Log("Defaulting to device name.");
        };

        SavePlayerName(playerName);
        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.JoinRandomRoom();
            EnterLobbyBtn.GetComponentInChildren<Text>().text = "SIGNED IN";

            EnterLobbyBtn.interactable = false;
            SearchingText.SetActive(true);
            MotorcycleSprite.GetComponent<FloatBehavior>().enabled = true;

        } else {
            EnterLobbyBtn.GetComponentInChildren<Text>().text = "ERROR: RESTART.";
        }

    }

    void Start()
    {
        SearchingText.SetActive(false);
        EnterLobbyBtn.onClick.AddListener(EnterLobby); 
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update() {
        if (PhotonNetwork.IsConnected) {
        }
    }

    void SavePlayerName(string playerName) {
        PhotonNetwork.NickName = playerName;
    }

    public override void OnConnectedToMaster(){
        //Debug.Log("Connected to Master.");
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log("No rooms found, creating a room.");
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = MaxPlayersPerRoom});
    }

    public override void OnJoinedRoom() {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount; 
        Debug.Log("Client [" + PhotonNetwork.NickName + "] successfully joined room. (Num Players: " + playerCount.ToString() + ")");
    }
}
