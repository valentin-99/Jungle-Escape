using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class CreateAndJoinRoms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;

    public PhotonHashtable hashtable;
    public static float seed;

    public void CreateRoom()
    {
        // when a player is creating a room, seed is generated
        // so all player will have same maps
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MultiplayerLevel");

        hashtable = new PhotonHashtable();

        if (PhotonNetwork.IsMasterClient)
        {
            seed = Random.Range(-10000, 10000);
            hashtable.Add("seed", seed);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }
        else
        {
            PhotonHashtable table = PhotonNetwork.CurrentRoom.CustomProperties;
            seed = (float)table["seed"];
        }
    }
}
