using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class move : MonoBehaviour
{
    public static bool isFinish;
    public static bool isStart;
    public float speed = 10f;
    public float jumpHeight = 6.0f;
    public GameObject joystickGameObject;
    public GameObject buttonGameObject;



    private Rigidbody rigidBody;
    private AudioSource audioSource;
   
    PhotonView photonView;
    private int lives = 4;
    private VirtualJoystick joystick;
    private Button jumpButton;
    private Vector3 startPosition;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        isFinish = false;
        isStart = false;
        //Debug.Log("Merhaba");
        startPosition = new Vector3(0, 5, 0);
        if ( photonView.IsMine )
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
        if ( isStart )
        {
            if ( transform.position.y > 0 && transform.position.y < 1 )
            {
                rigidBody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                audioSource.Play();
            }
        }
    }

    void Update()
    {
        if ( photonView.IsMine )
        {
            //movement();
            if ( transform.position.y <= -3 )
            {
                lives--;
                if ( lives < 1 )
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

    private void FixedUpdate()
    {
        if ( photonView.IsMine )
        {
            movement();
        }
    }

    void movement()
    {
        if ( !isFinish )
        {
            //float X = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            //float Z = Input.GetAxis("Vertical") * Time.deltaTime * 10.0f;
            //Vector3 vector = new Vector3(X, 0.0F, Z);

            //if ( vector.magnitude > 1 )
            //{
            //    vector.Normalize();
            //}

            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

            if ( isStart )
            {
                rigidBody.AddForce(input * speed);
            }

            if ( Input.GetKeyDown(KeyCode.Space) && (transform.position.y > 0 && transform.position.y < 1) )
            {
                rigidBody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                audioSource.Play();
            }
        }
        else
        {
            rigidBody.velocity = Vector3.zero;
        }

        if ( !isFinish )
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
            if ( isStart )
                rigidBody.AddForce(dir * speed);
        }
        else
        {
            rigidBody.velocity = Vector3.zero;
        }
    }
}
