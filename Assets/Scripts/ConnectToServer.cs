using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public void Start()
    {
        // connect to the Photon Server
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // Connect to the lobby
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        // Load the lobby scene
        SceneManager.LoadScene("Lobby");
    }
}
