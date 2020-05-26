using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Slider mSFXVolume;
    public static float mSfxVolume;
    public void PlayGame()
    {
        // SceneManager.LoadScene("SceneManager.GetActiveScene().buildIndex + 1");
        SceneManager.LoadScene("MiniGame");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        mSfxVolume = mSFXVolume.value;
    }
}
