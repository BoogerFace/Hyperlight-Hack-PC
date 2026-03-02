using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBoss : MonoBehaviour
{
    [Header("Scene to load after boss is destroyed")]
    public string startScreenScene = "StartScreen"; // Name of the Start Screen scene

    private void OnDestroy()
    {
        // Prevent accidental triggers when exiting play mode
        if (Application.isPlaying)
        {
            Debug.Log("Final Boss destroyed. Loading Start Screen...");
            SceneManager.LoadScene(startScreenScene);
        }
    }

        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
