using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.takeDamage(damage);
            }
            Destroy(gameObject);
        }

        // ✅ If it hits player's melee weapon, just destroy
        else if (other.CompareTag("PlayerMelee"))
        {
            Destroy(gameObject);
        }
    }

}
