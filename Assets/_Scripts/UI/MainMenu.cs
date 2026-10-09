using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Canvases
    [Header("Canvases")]
    public Canvas menuCanvas;
    public Canvas settingsCanvas;
    public Canvas creditsCanvas;
    public Canvas controlsCanvas;

    public void StartGame()    //Name is very specific be sure it matches
    {
        SceneManager.LoadScene("0");
    }

    public void OpenSettingsMenu()
    {
        menuCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);
    }

    public void OpenCredits()
    {
        menuCanvas.gameObject.SetActive(false);
        creditsCanvas.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
        
    }

    public void ExitSettingsMenu()
    {
        menuCanvas.gameObject.SetActive(true);
        settingsCanvas.gameObject.SetActive(false);
    }

    public void ExitCredits()
    {
        menuCanvas.gameObject.SetActive(true);
        creditsCanvas.gameObject.SetActive(false);
    }

    public void OpenControls()
    {
        controlsCanvas.gameObject.SetActive(true);
        settingsCanvas.gameObject.SetActive(false);
    }

    public void ExitControls()
    {
        controlsCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);
    }

    
}
