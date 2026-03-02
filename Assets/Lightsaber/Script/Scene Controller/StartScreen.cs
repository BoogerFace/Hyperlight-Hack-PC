using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [Header("UI References")]
    public GameObject levelPanel; // assign your level selection panel in Inspector
    public GameObject startPanel;
    public Button startButton;
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;

    // these store the actual scene names for runtime (including builds)
    public string level1Scene;
    public string level2Scene;
    public string level3Scene;

    void Awake()
    {
        // Get names from dragged SceneAssets in editor
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (levelPanel != null)
            levelPanel.SetActive(false);

        if (startButton != null)
            startButton.onClick.AddListener(ShowLevelPanel);

        if (level1Button != null)
            level1Button.onClick.AddListener(() => LoadLevel(level1Scene));

        if (level2Button != null)
            level2Button.onClick.AddListener(() => LoadLevel(level2Scene));

        if (level3Button != null)
            level3Button.onClick.AddListener(() => LoadLevel(level3Scene));
    }

    private void ShowLevelPanel()
    {
        if (levelPanel != null)
            levelPanel.SetActive(true);
        if (startPanel != null)
            startPanel.SetActive(false);
    }

    private void LoadLevel(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("No scene name assigned for this button!");
        }
    }
}
