using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class coinRotater : MonoBehaviour
{

    public GameObject winButton, lostButton;
    public GameObject confetti;
    private Vector3 rotation;
    void Start()
    {

        rotation = new Vector3(0, 0, 1);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation);
    }

    void OnDestroy()
    {
        if (collector.whoWin() == "me")
        {
            Instantiate(confetti);
            Destroy(confetti, 1.0f);
            winButton.SetActive(true);
        }
        else
        {
            lostButton.SetActive(true);
        }
    }

}
