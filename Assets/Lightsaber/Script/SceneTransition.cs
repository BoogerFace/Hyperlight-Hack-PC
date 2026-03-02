using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Trigger Objects")]
    [Tooltip("Drag GameObjects with colliders here that will trigger scene transitions.")]
    public Collider[] triggerColliders;

    [Header("Scene Settings")]
    [Tooltip("Drag the scene asset you want to load here.")]
    public string sceneAsset;
    [HideInInspector] public string sceneToLoad; // Runtime name

    [Header("Tag Filter (optional)")]
    public string requiredTag = "Player";

    private void Awake()
    {
        // Ensure triggers are marked properly
        foreach (Collider col in triggerColliders)
        {
            if (col != null) col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to one of the triggers
        foreach (Collider col in triggerColliders)
        {
            if (other == col) return; // Prevent self-detection
        }

        // Tag filter
        if (!string.IsNullOrEmpty(requiredTag) && !other.CompareTag(requiredTag))
            return;

        if (!string.IsNullOrEmpty(sceneAsset))
        {
            Debug.Log("Loading scene: " + sceneAsset);
            SceneManager.LoadScene(sceneAsset);
        }
        else
        {
            Debug.LogWarning("⚠️ Scene not assigned in SceneTransitionManager!");
        }
    }
}
