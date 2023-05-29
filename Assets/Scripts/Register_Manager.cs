using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Register_Manager : MonoBehaviour
{
    [SerializeField] private TMP_Text nickText;
    [SerializeField] private TMP_Text passwordText;
    [SerializeField] private TMP_Dropdown razaDropdown;
    [SerializeField] private Button registerButton;

    [SerializeField] private TMP_InputField nickIF;
    [SerializeField] private TMP_InputField passwordIF;

    private void Awake()
    {
        //Defino el listener para cada vez que se haga click al boton
        registerButton.onClick.AddListener(Log);
    }

    private void Log()
    {
        //Llamo a la funcion del network manager para conectarme al servidor pasando nick y contraseña
        Network_Manager._NETWORK_MANAGER.ConnectToServer(nickText.text.ToString(), passwordText.text.ToString(), 2, (razaDropdown.value + 1).ToString());
    }

    public void ClearInputs()
    {
        nickIF.text = string.Empty;
        passwordIF.text = string.Empty;
        razaDropdown.value = 0;
    }
}
