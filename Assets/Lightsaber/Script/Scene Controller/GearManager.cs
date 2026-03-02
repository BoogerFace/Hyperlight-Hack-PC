using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GearManager : MonoBehaviour
{
    public static GearManager instance;

    public int currentGears = 0;
    public TMP_Text gearText; // Assign in inspector to Canvas UI

    public GameObject tryAgainScreen; // Assign in inspector to Try Again screen
    public string startScreenScene = "StartScreen"; // Name of the Game Over scene

    public Button Continue;
    public Button Quit;






    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // 🔹 listen for scene changes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject gearObj = GameObject.Find("GearCount");
        if (gearObj != null)
        {
            gearText = gearObj.GetComponent<TMP_Text>();
        }
        // 🔹 Re-find UI references every time a scene is loaded
        tryAgainScreen = GameObject.Find("TryAgain");
        if (tryAgainScreen != null) tryAgainScreen.SetActive(false);

        // Re-assign buttons if they exist
        Button[] buttons = FindObjectsOfType<Button>(true);
        foreach (var btn in buttons)
        {
            if (btn.name == "Continue")
            {
                Continue = btn;
                Continue.onClick.RemoveAllListeners();
                Continue.onClick.AddListener(ContinueGame);
            }
            if (btn.name == "Quit")
            {
                Quit = btn;
                Quit.onClick.RemoveAllListeners();
                Quit.onClick.AddListener(QuitToGameOver);
            }
        }

        UpdateUI(); // refresh gear text after scene load
    }

    public void AddGears(int amount)
    {
        currentGears += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {

        if (gearText != null)
        {
            gearText.text = "Gears: " + currentGears;
        }
    }

    // Call this when the player dies
    public void PlayerDied()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (currentGears >= 1000)
        {
            // ✅ Show continue UI instead of loading scene
            if (tryAgainScreen != null)
            {
                tryAgainScreen.SetActive(true);
                Time.timeScale = 0f; // pause game
            }
        }
        else
        {
            Debug.Log("Not enough gears to continue, loading Game Over scene.");
            SceneManager.LoadScene(startScreenScene);
        }
    }
    // Called by Continue button
    public void ContinueGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (currentGears >= 1000)
        {
            currentGears -= 1000; // cost to continue
            UpdateUI();
            tryAgainScreen.SetActive(false);
            Time.timeScale = 1f;


            FindObjectOfType<PlayerHealth>().HealToFull();
        }
    }

    // Called by Quit button
    public void QuitToGameOver()
    {
        Time.timeScale = 1f; // unpause just in case
        SceneManager.LoadScene(startScreenScene);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Continue != null)
        {
            Continue.onClick.AddListener(ContinueGame);
        }

        if (Quit != null)
        {
            Quit.onClick.AddListener(QuitToGameOver);
        }


    }

    // Update is called once per frame
    void Update()
    {
            
        if (tryAgainScreen != null && tryAgainScreen.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                QuitToGameOver();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ContinueGame();
            }
        }

    }

}
