using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Author:  Fouche', Els
 * Updated: 05/09/2024
 * Notes:   Singleton for handling interoperability of systems.
 *          Receives information from playerController (player movement system),
 *          playerBakery (player health system) and dispatches information to the
 *          UI. Also provides accessible methods for other scripts to retrieve 
 *          information. 
 */

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance {  get { return _instance; } }

    [Header("Player Values")]
    public int maxHealth;
    public int currHealth;
    public int currResources = 100;

    [Header("User Interface")]
    public Canvas UI;
    public string healthSymbol;
    public string resourceSymbol;   // Currently unused

    private UIManager uiManager;
    private PlayerController playerController;
    private BakeryController playerBakery;

    /// <summary>
    /// Checker to ensure only one GameController is active in the scene. 
    /// </summary>
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            _instance = this;
        }
    }

    /// <summary>
    /// Initialization that searches for our UI Manager, our bakery (health)
    /// system, and our player controller (movement) system. Sets the UI to
    /// default values. 
    /// </summary>
    void Start()
    {
        if (!uiManager)
        {
            uiManager = GameObject.FindFirstObjectByType<UIManager>();
        } else { Debug.Log("UIManager not found."); }
        if (!playerBakery)
        {
            playerBakery = GameObject.FindFirstObjectByType<BakeryController>();
        } else { Debug.Log("BakeryController not found."); }
        if (!playerController)
        {
            playerController = GameObject.FindFirstObjectByType<PlayerController>();
        } else { Debug.Log("PlayerController not found."); }
        if (playerBakery)
        {
            playerBakery.SetMaxHealth(maxHealth);
            playerBakery.SetHealth(currHealth);
        } else { Debug.Log("BakeryController not found. Health values not set."); }

        uiManager.UpdateResource(currResources);
        uiManager.UpdateHealth(currHealth, healthSymbol);
    }

    /// <summary>
    /// Accessible method for switching scene. Probably unnecessary. 
    /// </summary>
    /// <param name="scene"></param>
    public void SwitchScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    /// <summary>
    /// Interconnection method for communication between 
    /// end of level state check performed in the enemy spanwer 
    /// and ui button for going to next level. 
    /// </summary>
    public void EndLevel()
    {
        uiManager.NextLevelButton();
    }

    /// <summary>
    /// Accessor method for retrieving the player's health value.
    /// </summary>
    /// <returns></returns>
    public int GetHealth() { return currHealth; }

    /// <summary>
    /// Mutator method for setting the player's health directly. Also
    /// updates the UI to reflect this change. 
    /// </summary>
    /// <param name="newHealth"></param>
    public void SetHealth(int newHealth) 
    { 
        currHealth = newHealth;
        uiManager.UpdateHealth(currHealth, healthSymbol);
    }

    /// <summary>
    /// Method for inflicting damage on the player and also updating
    /// the UI to reflect the damage. Unused as decrementing health is
    /// normally handled in the BakeryController but allows for other
    /// sources of player damage to be handled should the need arise.
    /// </summary>
    /// <param name="damage"></param>
    public void PlayerHurt(int damage) 
    { 
        currHealth -= damage;
        uiManager.UpdateHealth(currHealth, healthSymbol);
    }

    /// <summary>
    /// Accessor method for getting the player's current resource amount.
    /// </summary>
    /// <returns></returns>
    public int GetResources() { return currResources; }

    /// <summary>
    /// Mutator method for adding resources to the player and updating the
    /// UI. 
    /// </summary>
    /// <param name="resource"></param>
    public void AddResources(int resource) 
    { 
        currResources += resource;
        uiManager.UpdateResource(currResources);
    }

    /// <summary>
    /// This is called by the PlayerController to determine if they have 
    /// enough resources for a tower, decrementing their resources and 
    /// returning true and updating the UI if they do; otherwise returns false. 
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public bool PayForTower(int resource) 
    {
        if (currResources >= resource)
        {
            currResources -= resource;
            uiManager.UpdateResource(currResources);
            return true;
        } else
        {
            return false;
        }
    }

    /// <summary>
    /// Quits te game when called. This is where we would add additional 
    /// elements such as a confirmation button appearing. 
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Exiting");
        Application.Quit();
    }
}
