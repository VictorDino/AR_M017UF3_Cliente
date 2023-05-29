using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Room_UI_Maneger : MonoBehaviour
{

    [SerializeField] private TMP_Text createText;
    [SerializeField] private Button createButton;
    [SerializeField] private TMP_Text joinText;
    [SerializeField] private Button joinButton;

    private void Awake()
    {
        createButton.onClick.AddListener(CreateRoom);
        joinButton.onClick.AddListener(JoinRoom);
    }

    private void CreateRoom()
    {
        Photon_Manager._PHOTON_MANAGER.CreateRoom(createText.text.ToString());
    }
    
    private void JoinRoom()
    {
        Photon_Manager._PHOTON_MANAGER.JoinRoom(joinText.text.ToString());
    }
}
