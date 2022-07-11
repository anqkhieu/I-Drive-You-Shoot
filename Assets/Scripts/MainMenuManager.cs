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

    [SerializeField] private const int MaxPlayersPerRoom = 2; 

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
            EnterLobbyBtn.GetComponentInChildren<Text>().text = "ERROR";
        }

    }

    void Start()
    {
        SearchingText.SetActive(false);
        EnterLobbyBtn.onClick.AddListener(EnterLobby); 
        if (PhotonNetwork.IsConnected == false) { PhotonNetwork.ConnectUsingSettings(); }
    }

    void Update() {

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

        // feedback for the player who just joined
        if (playerCount == MaxPlayersPerRoom) {
            Debug.Log("Duo found, game ready to begin!");
            SearchingText.GetComponentInChildren<Text>().text = "DUO FOUND. GAME STARTING.";
            Invoke("GoToGameplay", 2f);
        }
    }

    public override void OnPlayerEnteredRoom(Player player) {
        // feedback for the player who was already in lobby
        if(PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom) {
            PhotonNetwork.CurrentRoom.IsOpen = false; 
            Debug.Log("Duo found, game ready to begin!");
            SearchingText.GetComponentInChildren<Text>().text = "DUO FOUND. GAME STARTING.";
            Invoke("GoToGameplay", 2f);
        }
    }

    void GoToGameplay() {
        PhotonNetwork.LoadLevel("Game");
    }
}
