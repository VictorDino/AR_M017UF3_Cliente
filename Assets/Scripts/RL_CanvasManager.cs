using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RL_CanvasManager : MonoBehaviour
{
    public static RL_CanvasManager _CANVAS_MANAGER;

    [SerializeField] GameObject login_cnv;
    [SerializeField] GameObject register_cnv;

    private void Awake()
    {
        if (_CANVAS_MANAGER != null && _CANVAS_MANAGER != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _CANVAS_MANAGER = this;

            login_cnv.SetActive(false);
            register_cnv.SetActive(false);
        }
    }

    public void ActivateLogin()
    {
        login_cnv.SetActive(true);
        register_cnv.SetActive(false);
    }

    public void ActivateRegister()
    {
        login_cnv.SetActive(false);
        register_cnv.SetActive(true);
    }

    public void GoNextScene()
    {
        PhotonNetwork.NickName = login_cnv.GetComponent<Login_Manager>().GetNickText();
        SceneManager.LoadScene("Launch_Scene");
    }

    public void ClearLogin()
    {
        login_cnv.GetComponent<Login_Manager>().ClearInputs();
    }

    public void ClearRegister()
    {
        register_cnv.GetComponent<Register_Manager>().ClearInputs();
    }
}