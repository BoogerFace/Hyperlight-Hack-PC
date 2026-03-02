using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CommunicationManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI incomingText;
    public Image playerPortrait;
    public Image missionGiverPortrait;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip incomingCallSound;
    public AudioClip dialogueBeepSound;
    public AudioClip advanceBeepSound;

    [Header("Dialogue Settings")]
    [TextArea(2, 6)]
    public string[] dialogueLines;   // Dialogue lines to show
    public float textSpeed = 0.03f;  // Speed of typewriter effect

    [Header("Next Level Loader (drag GameObject that loads gameplay scene)")]
    public string nextSceneName; // used at runtime

    private int currentLine = 0;
    private bool isTyping = false;
    private bool callStarted = false;


    private void Start()
    {
        // Ensure AudioSource exists
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("No AudioSource found! Please assign one to CommunicationManager.");
                return;
            }
        }

        // Initialize UI
        dialogueText.text = "";
        playerPortrait.gameObject.SetActive(false);
        missionGiverPortrait.gameObject.SetActive(false);
        incomingText.gameObject.SetActive(true);

        StartCoroutine(IncomingCallSequence());
    }

    private IEnumerator IncomingCallSequence()
    {
        if (incomingCallSound != null)
            audioSource.PlayOneShot(incomingCallSound);

        incomingText.text = "Incoming transmission... (Press [E] to answer)";
        callStarted = false;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        incomingText.gameObject.SetActive(false);
        playerPortrait.gameObject.SetActive(true);
        missionGiverPortrait.gameObject.SetActive(true);
        callStarted = true;

        StartCoroutine(DisplayNextLine());
    }

    private IEnumerator DisplayNextLine()
    {
        if (currentLine >= dialogueLines.Length)
        {
            EndCommunication();
            yield break;
        }

        if (advanceBeepSound != null)
            audioSource.PlayOneShot(advanceBeepSound);

        isTyping = true;
        dialogueText.text = "";

        string line = dialogueLines[currentLine];
        foreach (char c in line)
        {
            dialogueText.text += c;

            if (dialogueBeepSound != null)
                audioSource.PlayOneShot(dialogueBeepSound);

            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    private void Update()
    {
        if (!callStarted) return;

        if (Input.GetKeyDown(KeyCode.E) && !isTyping)
        {
            currentLine++;
            StartCoroutine(DisplayNextLine());
        }
    }

    private void EndCommunication()
    {
        dialogueText.text = "Transmission ended.";
        StartCoroutine(EndDelay());
    }

    private IEnumerator EndDelay()
    {
        yield return new WaitForSeconds(2f);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("No next scene assigned! Please drag a scene into the 'Next Level Loader' field.");
        }
    }
}
