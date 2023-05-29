using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pantallaCarga : MonoBehaviour
{
    [SerializeField] private float nextSceneTime = 5f;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > nextSceneTime) { GoNextScene(); }
    }

    public void GoNextScene()
    {
        SceneManager.LoadScene("Register_Login");
    }


}
