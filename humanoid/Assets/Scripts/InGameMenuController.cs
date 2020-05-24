using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuController : MonoBehaviour
{
    public static bool mGameIsPaused = false;
    public GameObject mInGameMenu;

    public void PauseGame()
    {
        mInGameMenu.SetActive(true);
        Time.timeScale = 0f;
        mGameIsPaused = true;
    }

    public void ResumeGame()
    {
        mInGameMenu.SetActive(false);
        Time.timeScale = 1f;
        mGameIsPaused = false;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
