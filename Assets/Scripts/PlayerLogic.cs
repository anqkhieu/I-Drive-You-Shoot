using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerLogic : MonoBehaviourPunCallbacks
{

    public PhotonView pView; 
    public GameObject shooter; 
    public Rigidbody2D rb; 
    public GameObject bullet; 
    public GameObject skull;

    private bool isGrounded = false; 
    public float jumpForce; 

    private float lastShotTime = 0f; 
    private float bullets = 3; 
    private float shotCooldown = 2f; 
    private bool canShoot = true; 

    private bool isAlive = true; 
    public GameObject scoreUI; 
    public GameObject controlsUI; 
    private int score = 0; 

    public int difficulty = 18;

    public AudioSource audioSource; 
    private bool DEBUG_MODE = true; 

    // Start is called before the first frame update
    void Start()
    {
        // Invoke("PrintPlayerNames", 3);

    }

    IEnumerator ReloadLevel(int delayTime) {
        yield return new WaitForSeconds(delayTime);
        PhotonNetwork.LoadLevel("Game");
    }

    void PrintPlayerNames() {
        foreach (Player p in PhotonNetwork.PlayerList) {
            Debug.Log("[Player " + p.ActorNumber.ToString() + "] " + p.NickName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // player 1 is the host client who owns the photon view
        if (pView.AmOwner) {
            if (DEBUG_MODE) { Debug.Log("> Player 1 [" + PhotonNetwork.PlayerList[0].NickName + "]:  Press **Space** to Jump"); DEBUG_MODE = false; }
            CheckInputP1();
        } else { // player 2 does not own the photon view 
            if (DEBUG_MODE) { Debug.Log("> Player 2 [" + PhotonNetwork.PlayerList[1].NickName + "]: Press**Click** to Shoot"); DEBUG_MODE = false; }
            CheckInputP2();
        }

    }

    void FixedUpdate()
    {
        if(pView.IsMine)
        {
            if (isAlive) { score = Mathf.RoundToInt(Time.timeSinceLevelLoad) - 10; } 
            if (score < 0) { score = 0; }
            pView.RPC(nameof(UpdateLabels), RpcTarget.All, score);

            if (isAlive) {
                int n = Random.Range(1, Mathf.Abs(Mathf.RoundToInt(10000 - (Time.timeSinceLevelLoad * difficulty))));
                if (n < (75 + (Time.timeSinceLevelLoad * difficulty))) { 
                    PhotonNetwork.Instantiate(skull.name, new Vector3(12, 0, 0), Quaternion.identity, 0); 
                }
            }
        }
    }


    [PunRPC]
    void WeDied() {
        if (pView.IsMine) { audioSource.Play(); }
        isAlive = false; 
        controlsUI.transform.GetComponent<Text>().text= ">> GAME OVER <<";
        controlsUI.transform.GetComponent<Text>().color = Color.red;
        controlsUI.transform.GetChild(0).GetComponent<Text>().text = "RIP " + PhotonNetwork.PlayerList[0].NickName + " & " + PhotonNetwork.PlayerList[1].NickName;
        controlsUI.transform.GetChild(1).GetComponent<Text>().text = "FINAL SCORE: " + score.ToString(); 
        controlsUI.transform.GetChild(1).GetComponent<Text>().color = Color.yellow; 

        if (PhotonNetwork.PlayerList.Length == 2) {
            Debug.Log("Resetting in 3 seconds...");
            StartCoroutine(ReloadLevel(3));
        }
    }

    [PunRPC]
    void UpdateLabels(int newScore)
    {
        scoreUI.GetComponent<Text>().text  = newScore.ToString(); 
        double cd = (Mathf.Round((shotCooldown - (Time.time - lastShotTime)) * 100)) / 100.0;
        try {
            if (isAlive) {
                // driver 
                controlsUI.transform.GetChild(0).GetComponent<Text>().text = "[P1] " + PhotonNetwork.PlayerList[0].NickName + " // Space to Jump"; 

                // shooter
                if ((Time.time - lastShotTime) >= shotCooldown) { canShoot = true; if (bullets <= 0) { bullets = 3; } }
                if (canShoot) {
                    controlsUI.transform.GetChild(1).GetComponent<Text>().text = "[P2] " + PhotonNetwork.PlayerList[1].NickName + " // Click to Shoot [Shots Left: " + bullets.ToString() + "]";
                } else {
                    controlsUI.transform.GetChild(1).GetComponent<Text>().text = "[P2] " + PhotonNetwork.PlayerList[1].NickName + " // RELOADING [" + cd.ToString() + "]";
                }
            }
        } catch {
            Debug.Log("Oh no, your duo abandoned you! Restart game to matchmake!");
            isAlive = false; 
        } 
    }

    void OnCollisionEnter2D(Collision2D col) { 
        isGrounded = true; 
        if (col.gameObject.tag == "Deathwall") { WeDied(); }
    }

    void CheckInputP1() {
        if (Input.GetKey("space")) {
            if (isGrounded) {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false; 
            }  
        }
    }

    void CheckInputP2() { 
        if ((Input.GetMouseButtonDown(0)) && (isAlive)) { 
            if ((bullets > 0) && canShoot) {
                Vector3 newPos = shooter.transform.position + new Vector3(1,0,0);
                newPos.z = 0; 
                PhotonNetwork.Instantiate(bullet.name, newPos, Quaternion.identity, 0);
                bullets--; 
                if (bullets <= 0) { canShoot = false; lastShotTime = Time.time; }
            } else {  // check whether shooter is still on cooldown
                if ((Time.time - lastShotTime) >= shotCooldown) { canShoot = true; if (bullets <= 0) { bullets = 3; } }
            }
        }
    }
}
