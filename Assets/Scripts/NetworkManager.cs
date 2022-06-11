using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance; 

    void Awake () {
        // ensure there is only one active network manager
        if (instance != null && instance != this)
            gameObject.SetActive(false);
        else {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
    }

    public override void OnConnectedToMaster() {
        Debug.Log("Connected to the master server...");
        CreateRoom("testroom");
    }

    public override void OnCreatedRoom() {
        Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
    }

    void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom(string roomName) {
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom(string roomName) {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void ChangeScene(string sceneName) {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
