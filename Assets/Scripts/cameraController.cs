using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{

    static public GameObject player;
    static public Transform playerTransform;
    public GameObject heart1, heart2, heart3;
    private Vector3 offset;
    private int lives;

    private Vector3 camPosition;
    private Vector3 playerPosition;
    void Start()
    {
        heart1.SetActive(true);
        heart2.SetActive(true);
        heart3.SetActive(true);
        camPosition = transform.position;
        Debug.Log("cam position: " + camPosition);
        lives = 4;
    }

    // Update is called once per frame
    int i = 0;//offset'i ayarlamak için
    bool falling = false;
    void LateUpdate()
    {
        if (player != null && i == 0)
        {
            playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z * -1);
            offset = transform.position - playerTransform.position;
            offset = new Vector3(0f, offset.y, offset.z);
            transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z + offset.z);
            i++;
        }
        if (player != null)
        {
            transform.position = playerTransform.position + offset;
            //Debug.Log(player.transform.position.y);
            if (playerTransform.position.y <= -2)
            {
                falling = true;
            }
            if (playerTransform.position.y > 2 && playerTransform.position.y <= 5)
            {
                if (falling)
                {
                    liveControl();
                    falling = false;
                }
            }
        }
        //Debug.Log("position-1: "+transform.position);
    }

    public void liveControl()
    {
        lives--;
        if (lives == 3)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
        }
        else if (lives == 2)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(false);
        }
        else if (lives == 1)
        {
            heart1.SetActive(true);
            heart2.SetActive(false);
            heart3.SetActive(false);
        }
        else if (lives == 0)
        {
            heart1.SetActive(false);
            heart2.SetActive(false);
            heart3.SetActive(false);
        }

    }
}
