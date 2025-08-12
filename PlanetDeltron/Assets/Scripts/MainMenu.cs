using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   
    public void PlayGame()
    {
        SceneManager.LoadScene("StartGame");
     
    }

    public void aboutMenu(){
        SceneManager.LoadScene("aboutMenu");

    }
    public void mainMenu(){
        SceneManager.LoadScene("MainMenu");

    }
    public void howToMenu(){
        SceneManager.LoadScene("howToMenu");

    }

    public void OpenGit()
    {
        Application.ExternalEval("window.open(\"https://github.com/akosbujdi/PlanetDeltron\")");
    }

}
