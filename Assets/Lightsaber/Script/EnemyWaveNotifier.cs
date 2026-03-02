using UnityEngine;

public class EnemyWaveNotifier : MonoBehaviour
{
    public StopZone stopZone;

    private void OnDestroy()
    {
        // Notify stop zone that this enemy is dead
        if (stopZone != null)
        {
            stopZone.EnemyDied();
        }
    }
}
