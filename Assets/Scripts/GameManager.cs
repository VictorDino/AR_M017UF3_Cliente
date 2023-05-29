using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager _GAME_MANAGER;

    [SerializeField] private GameObject spawnPlayer1;
    [SerializeField] private GameObject spawnPlayer2;

    private GameObject player1;
    private GameObject player2;

    //generamos singleton
    private void Awake()
    {
        if (_GAME_MANAGER != null && _GAME_MANAGER != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _GAME_MANAGER = this;

            InstantiatePlayers();
        }
    }

    private void InstantiatePlayers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            player1 = PhotonNetwork.Instantiate("Player", spawnPlayer1.transform.position, Quaternion.identity);
        }
        else
        {
            player2 = PhotonNetwork.Instantiate("Player", spawnPlayer2.transform.position, Quaternion.identity);
        }
    }

    public void Respawn(GameObject player)
    {
        if (player == player1)
        {
            player.transform.position = spawnPlayer1.transform.position;
        }
        else
        {
            player.transform.position = spawnPlayer2.transform.position;
        }
    }
}
