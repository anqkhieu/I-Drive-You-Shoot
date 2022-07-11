using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BulletMovement : MonoBehaviour
{
    public PhotonView pView; 
    public float bulletSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
       
    }
    

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime);
    }

    void OnBecameInvisible() {
        if ((pView.IsMine)) { PhotonNetwork.Destroy(this.gameObject); }        
    }

    void OnCollisionEnter2D(Collision2D col) { 
        // Debug.Log("bullet collided with " + col.gameObject.name);

        if ((col.gameObject.tag == "Skull") || (col.gameObject.name == "Skull(Clone)")) {
            if (pView.IsMine) { 
                Destroy(col.gameObject); 
                Destroy(this.gameObject); 
            } else {
                col.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
            }
        }
    }

}
