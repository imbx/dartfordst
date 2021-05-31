using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_R : MonoBehaviour
{
    public void LoadScene(string nivel)
    {
        SceneManager.LoadScene(nivel);
    
    }

    public void Exit()
    {
        Application.Quit();
    }


}
