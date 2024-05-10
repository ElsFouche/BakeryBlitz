using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance {  get { return _instance; } }

    [Header("Player Values")]
    public int maxHealth;
    public int currHealth;
    public int currResources = 100;

    public Canvas UI;
    public string healthSymbol;
    public string resourceSymbol;
    private UIManager uiManager;
    private PlayerController playerController;
    private BakeryController playerBakery;

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

    public void SwitchScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void EndLevel()
    {
        uiManager.NextLevelButton();
    }
    public int GetHealth() { return currHealth; }
    public void SetHealth(int newHealth) 
    { 
        currHealth = newHealth;
        uiManager.UpdateHealth(currHealth, healthSymbol);
    }
    public void PlayerHurt(int damage) 
    { 
        currHealth -= damage;
        uiManager.UpdateHealth(currHealth, healthSymbol);
    }
    public int GetResources() { return currResources; }
    public void AddResources(int resource) 
    { 
        currResources += resource;
        uiManager.UpdateResource(currResources);
    }
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

    public void QuitGame()
    {
        // This is where we would add a confirmation screen. 
        Debug.Log("Exiting");
        Application.Quit();
    }
}
