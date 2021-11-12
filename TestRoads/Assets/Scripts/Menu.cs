using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void ButtonStart()
    {
        SceneManager.LoadScene("Main");
    }
    public void ButtonExit()
    {
        Application.Quit();
    }
}
