using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
public class manage : MonoBehaviourPunCallbacks
{

    public Text timerText;
    int currentPlayerCount = 1;
    public bool startTimer = false;
    double timerIncrementValue;
    double startTime;
    double timer = 30;
    ExitGames.Client.Photon.Hashtable CustomValue;
    static bool isConnect = false;

    void Update()
    {
        if (!startTimer) return;

        timerIncrementValue = PhotonNetwork.Time - startTime;
        timerText.text ="Waiting for other players.\nThe Game will start in "+Mathf.Round((float)(timer - timerIncrementValue)).ToString() + " seconds.";
        Debug.Log("timer: " + timerIncrementValue);
        if (timerIncrementValue >= timer)
        {
            //oyuncular hareket edebilsin
            move.isStart = true;
            if (PhotonNetwork.IsMasterClient)
                StartCoroutine(addBots());
            startTimer = false;
            timerText.gameObject.SetActive(false);
        }
    }
    public static void loadScene(int scene)
    {
        Debug.Log("Load Scene " + scene);
        PhotonNetwork.LoadLevel(scene);
        PhotonNetwork.ConnectUsingSettings();
    }
    public static void disconnect()
    {
        PhotonNetwork.Disconnect();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected Server ");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions { MaxPlayers = 3, IsOpen = true, IsVisible = true }, TypedLobby.Default);
        isConnect = true;
        Debug.Log(PhotonNetwork.Time);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        GameObject gameObject = PhotonNetwork.Instantiate("player", new Vector3(Random.Range(-4, 4), 0, Random.Range(-3, 3)), Quaternion.identity, 0, null);
        gameObject.GetComponent<PhotonView>().Owner.NickName = "Reve" + Random.Range(1, 100);
        if (PhotonNetwork.IsMasterClient && isConnect)
        {
            CustomValue = new ExitGames.Client.Photon.Hashtable();
            startTime = PhotonNetwork.Time;
            startTimer = true;
            Debug.Log("start time:" + startTime);
            CustomValue.Add("StartTime", startTime);
            PhotonNetwork.CurrentRoom.SetCustomProperties(CustomValue);
            Debug.Log("Custom Value:" + double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString()));
        }
        else
        {
            startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
            startTimer = true;
        }
    }

    IEnumerator addBots()
    {
        yield return new WaitForSeconds(0.1f);
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = currentPlayerCount; i < 3; i++)
            {
                PhotonNetwork.Instantiate("playerAI", new Vector3(Random.Range(-4, 4), 0, Random.Range(-3, 3)), Quaternion.identity, 0, null);
            }
        }
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Left Lobby");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("JoinRoomFailed");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("CreateRoomFailed");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("JoinRandomFailed");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("new player: " + newPlayer.ToString());
        currentPlayerCount++;
        Debug.Log("player count: " + currentPlayerCount.ToString());
    }

    void OnGUI()
    {
        GUI.Window(0, new Rect(Screen.width - 200, 0, 200, 50), LobbyWindow, "");

    }

    void LobbyWindow(int index)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Status: " + PhotonNetwork.NetworkClientState);

        if (!PhotonNetwork.IsConnected || PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
        {
            GUI.enabled = false;
        }
        GUILayout.FlexibleSpace();
    }
}
