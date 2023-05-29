using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Photon_Manager : MonoBehaviourPunCallbacks
{
    public static Photon_Manager _PHOTON_MANAGER;

    //generamos singleton
    private void Awake()
    {
        if (_PHOTON_MANAGER != null && _PHOTON_MANAGER != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _PHOTON_MANAGER = this;
            DontDestroyOnLoad(this.gameObject);

            //realizo conexion
            PhotonConnect();
        }
    }

    private void PhotonConnect()
    {
        //sincronizo la carga de la sala para todos los jugadores
        PhotonNetwork.AutomaticallySyncScene = true;

        //conexion al servidor con la configuracion establecida
        PhotonNetwork.ConnectUsingSettings();
    }
    //al conectarme al servidor
    public override void OnConnectedToMaster()
    {
        Debug.Log("Conexion realizada correctamente!");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    //al desconectarme
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Ha explotado porque: " + cause);
    }
    //al unirme al lobby
    public override void OnJoinedLobby()
    {
        Debug.Log("Accediendo al lobby!");
    }
    //funcion para crear salas
    public void CreateRoom (string nameRoom)
    {
        PhotonNetwork.CreateRoom(nameRoom, new RoomOptions { MaxPlayers = 2});
    }
    //funcion para unirme a salas
    public void JoinRoom (string nameRoom)
    {
        PhotonNetwork.JoinRoom(nameRoom);
    }
    //al unirme a la sala
    public override void OnJoinedRoom()
    {
        Debug.Log("Me he unido a la sala: " + PhotonNetwork.CurrentRoom.Name + " con " + PhotonNetwork.CurrentRoom.PlayerCount + " jugadores en ella.");
    }
    //al no poder unirme
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("No me he podido conectar a la sala dado el error: " + returnCode + " que significa: " + message);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }
}
