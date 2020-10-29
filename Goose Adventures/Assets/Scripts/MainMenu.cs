using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Update()
    {

    }

    public void NewGameButton()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("LevelSelector");

    }

    public void ContinueButton()
    {
        SceneManager.LoadScene("LevelSelector");

    }

    public void CreditsButton()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

}
