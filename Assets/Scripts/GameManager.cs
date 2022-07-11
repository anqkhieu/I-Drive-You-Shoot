using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    public GameObject playerObject; 
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnPlayer", 2);
        audioSource.Play();
    }

    void SpawnPlayer() {
        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.Instantiate(playerObject.name, new Vector3(-3.44f, -3.32f, -1.5f), Quaternion.identity, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.6f);
    }

}
