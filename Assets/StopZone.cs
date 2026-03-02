using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

public class StopZone : MonoBehaviour
{
    [Header("References")]
    public CinemachineDollyCart playerCart;    // Your player cart
    public float resumeSpeed = 5f;             // Speed after enemies cleared

    [Header("Enemies")]
    public List<GameObject> enemiesToActivate; // Drag your pre-placed enemies here

    [Header("Laser Barriers")]
    public List<GameObject> lasersToDeactivate; // Drag any laser or obstacle GameObjects here

    private bool waitingForClear = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!waitingForClear && other.CompareTag("Player"))
        {
            // Stop the cart
            if (playerCart != null)
            {
                playerCart.m_Speed = 0f;
                Debug.Log("🚦 Player stopped – activating enemies");
            }

            // Activate all enemies assigned to this stop zone
            foreach (GameObject enemy in enemiesToActivate)
            {
                if (enemy != null)
                {
                    enemy.SetActive(true);

                    // Add notifier if enemy dies
                    EnemyWaveNotifier notifier = enemy.GetComponent<EnemyWaveNotifier>();
                    if (notifier == null)
                    {
                        notifier = enemy.AddComponent<EnemyWaveNotifier>();
                        notifier.stopZone = this;
                    }
                }
            }

            waitingForClear = true;
        }
    }

    // Called by EnemyWaveNotifier when an enemy dies
    public void EnemyDied()
    {
        bool allDead = true;
        foreach (GameObject enemy in enemiesToActivate)
        {
            if (enemy != null && enemy.activeInHierarchy)
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            DeactivateLasers();
            ResumeMovement();
        }
    }

    private void DeactivateLasers()
    {
        foreach (GameObject laser in lasersToDeactivate)
        {
            if (laser != null && laser.activeInHierarchy)
            {
                laser.SetActive(false);
                Debug.Log("💥 Laser deactivated!");
            }
        }
    }

    private void ResumeMovement()
    {
        if (playerCart != null)
        {
            playerCart.m_Speed = resumeSpeed;
            Debug.Log("✅ Cart resumed – all enemies cleared");
        }
        waitingForClear = false;
    }
}
