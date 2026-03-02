using UnityEngine;
using TMPro;   // <-- Needed for TextMeshPro

public class BombThrower : MonoBehaviour
{
    [Header("Bomb Settings")]
    public GameObject bombPrefab;
    public Transform throwPoint;        // Where bombs are spawned
    public int bombsPerLevel = 2;
    public float throwForce = 10f;
    public float upwardForce = 3f;      // Adds arc to throw

    [Header("UI Reference")]
    public TextMeshProUGUI grenadeText;  // Reference to TMP UI text

    private int bombsLeft;

    private void Start()
    {
        bombsLeft = bombsPerLevel;
        UpdateGrenadeUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && bombsLeft > 0)
        {
            ThrowBomb();
        }
    }

    private void ThrowBomb()
    {
        GameObject bombInstance = Instantiate(bombPrefab, throwPoint.position, Quaternion.identity);
        Rigidbody rb = bombInstance.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 force = transform.forward * throwForce + Vector3.up * upwardForce;
            rb.AddForce(force, ForceMode.Impulse);
        }

        bombsLeft--;
        UpdateGrenadeUI();
        Debug.Log("Bomb thrown! Bombs left: " + bombsLeft);
    }

    private void UpdateGrenadeUI()
    {
        if (grenadeText != null)
        {
            grenadeText.text = "x" + bombsLeft.ToString();
        }
        else
        {
            Debug.LogWarning("Grenade Text reference not assigned in inspector!");
        }
    }

    // Optional method if you ever want to add bombs back
    public void AddBombs(int amount)
    {
        bombsLeft += amount;
        UpdateGrenadeUI();
    }
}
