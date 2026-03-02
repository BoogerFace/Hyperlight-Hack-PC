using UnityEngine;
using Cinemachine;

public class TrackSwitcher : MonoBehaviour
{
    [Header("Track Settings")]
    public CinemachineDollyCart playerCart;        // The ONE cart that moves the player
    public CinemachineSmoothPath chuteTrack;       // First path
    public CinemachineSmoothPath bossTrack;        // Boss loop path
    public float bossSpeed = 5f;                   // Speed on the boss path

    [Header("Camera Settings")]
    public CinemachineVirtualCamera bossCam;       // Virtual camera inside PlayerCart
    public Transform bossTarget;                   // Boss robot to look at

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchToBossTrack();
        }
    }

    private void SwitchToBossTrack()
    {
        // Stop cart briefly (optional smoothness)
        playerCart.m_Speed = 0f;

        // Change to boss path and restart at the beginning
        playerCart.m_Path = bossTrack;
        playerCart.m_Position = 0f;

        // Resume with new speed
        playerCart.m_Speed = bossSpeed;

        // ✅ Activate camera LookAt now
        if (bossCam != null && bossTarget != null)
        {
            bossCam.LookAt = bossTarget;
        }

        Debug.Log("✅ Switched to Boss Track & Camera now looks at Boss");
    }
}
