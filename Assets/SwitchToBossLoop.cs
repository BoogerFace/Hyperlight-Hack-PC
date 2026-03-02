using UnityEngine;
using Cinemachine;

public class SwitchToBossLoop : MonoBehaviour
{
    [Header("Camera References")]
    public CinemachineVirtualCamera chuteCam;
    public CinemachineVirtualCamera bossCam;

    [Header("Track References")]
    public Transform player;          // The Player root object with Rigidbody + Collider
    public Transform bossDollyCart;   // The Dolly Cart for the boss loop track

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger by: " + other.name);

        // Check if the object entering is the Player
        if (other.CompareTag("Player"))
        {
            SwitchToBoss();   // do all actions in one method
        }
    }

    private void Update()
    {
        // Optional manual test key
        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchToBoss();
        }
    }

    private void SwitchToBoss()
    {
        // --- Camera Switch ---
        bossCam.Priority = 30;    // Higher number = active camera
        chuteCam.Priority = 10;

        // --- Player Re-Parenting ---
        if (player != null && bossDollyCart != null)
        {
            player.SetParent(bossDollyCart);
            player.localPosition = Vector3.zero;   // Snap to the cart's origin
        }

        Debug.Log("Camera switched to BossCam and Player attached to Boss Dolly Cart");
    }
}
