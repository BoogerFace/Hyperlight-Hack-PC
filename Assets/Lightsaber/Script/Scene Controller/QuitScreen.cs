using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuitScreen : MonoBehaviour
{
    private string previousScene;
    public Button quitBtn;
    public Button startOverBtn;

    private void Start()
    {
        // Store the previous scene name (the scene that was active before this one)
        previousScene = PlayerPrefs.GetString("PreviousScene", "");
    }

    // Call this when the player chooses "Start Over"
    public void StartOver()
    {
        if (!string.IsNullOrEmpty(previousScene))
        {
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.LogWarning("No previous scene stored! Reloading current scene instead.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Call this when the player chooses "Quit to Start Screen"
    public void QuitToStart()
    {
        SceneManager.LoadScene("StartScreen"); // ✅ replace with your actual start menu scene name
    }

    // Helper: Call this before loading quit screen to remember current scene
    public static void SavePreviousScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("PreviousScene", currentScene);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
