using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Network_Manager : MonoBehaviour
{
    public static Network_Manager _NETWORK_MANAGER;

    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    private bool connected = false;

    
    const string host = "127.0.0.1";
    const int port = 6543;

    private void Awake()
    {
        
        if (_NETWORK_MANAGER != null && _NETWORK_MANAGER != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            
            _NETWORK_MANAGER = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Update()
    {
        
        if (connected)
        {
            
            if (stream.DataAvailable)
            {
                
                string data = reader.ReadLine();

                
                if (data != null)
                {
                    ManageData(data);
                }
            }
        }
    }

    public void ConnectToServer(string nick, string password, int mode, string raza = "-1")
    {
        try
        {
            //Instancia la clase para gestionar la conexion y el streaming de datos
            socket = new TcpClient(host, port);
            stream = socket.GetStream();

            //Si hay streaming de datos hay conexion
            connected = true;

            //Instancio clases de lectura y escritura
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            //Envio los parametros al servidor separados por '/'
            writer.WriteLine(mode + "/" + nick + "/" + password + "/" + raza);

            //Limpio el writer de datos
            writer.Flush();

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void ManageData(string data)
    {
        //Si recibo ping devuelvo 1 como respuesta al servidor
        if (data == "ping")
        {
            Debug.Log("Recibo ping");
            writer.WriteLine("0");
            writer.Flush();
        }
        else if (data == "correctLogin")
        {
            Debug.Log("Correct LogIn");
            if (SceneManager.GetActiveScene().name == "Register_Login")
            {
                RL_CanvasManager._CANVAS_MANAGER.GoNextScene();
            }
        }
        else if (data == "incorrectLogin")
        {
            Debug.Log("Incorrect LogIn");
            if (SceneManager.GetActiveScene().name == "Register_Login")
            {
                RL_CanvasManager._CANVAS_MANAGER.ClearLogin();
            }
        }
        else if (data == "correctRegister")
        {
            Debug.Log("Correct Register");
            if (SceneManager.GetActiveScene().name == "Register_Login")
            {
                RL_CanvasManager._CANVAS_MANAGER.ActivateLogin();
            }
        }
        else if (data == "incorrectRegister")
        {
            Debug.Log("Incorrect Register");
            if (SceneManager.GetActiveScene().name == "Register_Login")
            {
                RL_CanvasManager._CANVAS_MANAGER.ClearRegister();
            }
        }
    }
}
