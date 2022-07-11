using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SkullProjectile : MonoBehaviour
{
    public PhotonView pView; 
    public float minSpeed = 2f;
    public float maxSpeed = 3f; 

    public int minHeight = 0; 
    public int maxHeight = 0; 

    private float floatStrength = 1; 
    private bool isFloater = false; 

    private float projectileSpeed = 0f; 
    private int height = 0; 

    // Start is called before the first frame update
    void Start()
    {
        projectileSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        height = UnityEngine.Random.Range(minHeight, maxHeight);

        int num = UnityEngine.Random.Range(0, 2);
        if (num == 1) { isFloater = true; }

        floatStrength = UnityEngine.Random.Range(1.2f, 2); 

        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, height, this.gameObject.transform.position.z); 
    }
    

    // Update is called once per frame
    void Update()
    {
        // go to the left 
        this.gameObject.transform.Translate(Vector3.left * projectileSpeed * Time.deltaTime);

        // floater skull
        if (isFloater) { 
            float old_y = transform.position.y; 
            float new_y =  height + ((float)Math.Sin(Time.time * 1.2f) * floatStrength);
            transform.position = new Vector2(transform.position.x, height + ((float)Math.Sin(Time.time * 1.2) * floatStrength));
        }
    }

    void OnBecameInvisible() {
        if (((PhotonNetwork.IsMasterClient)) && (this.gameObject.transform.position.x <= -5)) { PhotonNetwork.Destroy(this.gameObject); }        
    }

    // void OnCollisionEnter2D(Collision2D col) { 
    //     Debug.Log("Colliding with..." + col.gameObject.name);
    //     if ((col.gameObject.tag == "Bullet") || (col.gameObject.name == "Bullet(Clone)")) {
    //         if (pView.IsMine) { PhotonNetwork.Destroy(this.gameObject); }
    //     }
    // }

    // void OnCollisionEnter(Collision col)
    // {
    //     if ((col.gameObject.tag == "Bullet") || (col.gameObject.name == "Bullet(Clone)")) {
    //         if (PhotonNetwork.IsMasterClient) { 
    //             PhotonNetwork.Destroy(col.gameObject);
    //             PhotonNetwork.Destroy(this.gameObject); 
    //         }
    //     }
    // }

}
