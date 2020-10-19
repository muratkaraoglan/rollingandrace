using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class collector : MonoBehaviour
{

    PhotonView photonView;
    public static string winner;
    void Start()
    {
        photonView = GetComponent<PhotonView>();

    }
    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.tag == "finish_coin")
        {
            Destroy(collision.gameObject);

            if (photonView.IsMine && this.gameObject.tag == "Player")
            {
                Debug.Log("You Won");
                winner = "me";
            }
            else
            {
                Debug.Log("You Lost");
                winner = "other";
            }
            move.isFinish = true;
        }
    }

    public void Click()
    {
        manage.disconnect();
        SceneManager.LoadScene(0);
    }

    public static string whoWin()
    {
        return winner;
    }

}
