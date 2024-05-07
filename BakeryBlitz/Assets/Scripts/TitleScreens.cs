using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
/*
 * Symon Belcher
 * 4/25/2024
 * Manages the title screen, and game over screen
 */


public class TitleScreens : MonoBehaviour
{
    /// <summary>
    /// Switches scenes when Player dies, clicks restart, or play
    /// </summary>
    public void QuitGame() // exits game
    {

        // print("quite game");
        Application.Quit();
    }

    public void SwitchScene(int buildIndex)// switches scene to platofmer game from end scene
    {
        SceneManager.LoadScene(buildIndex);
    }

}