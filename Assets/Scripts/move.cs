using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class move : MonoBehaviour
{

    private Rigidbody rigidBody;
    private AudioSource audioSource;
    private float rotationSpeed = 25.0f;
    private float jumpHeight = 6.0f;
    PhotonView photonView;
    private Vector3 offset;
    private int lives = 4;
    public static bool isFinish;
    public static bool isStart;
    private VirtualJoystick joystick;

    public GameObject joystickGameObject;

    public GameObject buttonGameObject;
    private Button jumpButton;
    private Vector3 startPosition;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        isFinish = false;
        isStart = false;
        Debug.Log("Merhaba");
        startPosition = new Vector3(0, 5, 0);
        if (photonView.IsMine)
        {
            GetComponent<Renderer>().material.color = Color.blue;
            cameraController.player = this.gameObject;
            cameraController.playerTransform = transform;
            joystick = Instantiate(joystickGameObject, FindObjectOfType<Canvas>().transform).GetComponent<VirtualJoystick>();
            jumpButton = Instantiate(buttonGameObject, FindObjectOfType<Canvas>().transform).GetComponent<Button>();
            jumpButton.onClick.AddListener(onClick);
        }
        startPosition = startPosition + transform.position;
        rigidBody.velocity = Vector3.zero;
    }

    void onClick()
    {
        if (isStart)
        {
            if (transform.position.y > 0 && transform.position.y < 1)
            {
                rigidBody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                audioSource.Play();
            }
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            movement();
            if (transform.position.y <= -3)
            {
                lives--;
                if (lives < 1)
                {
                    Debug.Log("Kaybettin. " + lives);
                    Destroy(photonView.gameObject);
                    manage.disconnect();
                    SceneManager.LoadScene(0);
                }
                else
                {
                    Debug.Log("Kalan: " + lives.ToString());
                    transform.position = startPosition;
                    rigidBody.velocity = Vector3.zero;
                }
            }
        }
    }

    void movement()
    {
        if (!isFinish)
        {
            float X = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            float Z = Input.GetAxis("Vertical") * Time.deltaTime * 10.0f;
            Vector3 vector = new Vector3(X, 0.0F, Z);

            if (vector.magnitude > 1)
            {
                vector.Normalize();
            }
            if (isStart)
                rigidBody.AddForce(vector, ForceMode.Impulse);
            if (Input.GetKeyDown(KeyCode.Space) && (transform.position.y > 0 && transform.position.y < 1))
            {
                rigidBody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                audioSource.Play();
            }
        }
        else
        {
            rigidBody.velocity = Vector3.zero;
        }

        if (!isFinish)
        {
            Vector3 dir = Vector3.zero;

            dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");
            /*
            if (dir.magnitude > 2)
            {
                dir.Normalize();
            }*/
            dir = joystick.InputDirection;
            if (isStart)
                rigidBody.AddForce(dir * rotationSpeed);
        }
        else
        {
            rigidBody.velocity = Vector3.zero;
        }
    }
}
