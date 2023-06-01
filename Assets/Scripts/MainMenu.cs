using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject HowToPlayMenu;
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void HowToPlay()
    {
        HowToPlayMenu.SetActive(true);
    }

    public void QuitHowToPlay()
    {
        HowToPlayMenu.SetActive(false);
    }
}
