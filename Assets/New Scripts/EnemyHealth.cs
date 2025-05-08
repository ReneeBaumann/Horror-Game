using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the enemy.
    private int currentHealth; // Current health of the enemy.

    private void Start()
    {
        currentHealth = maxHealth; // Set the initial health to the maximum health.
    }

    // Function to apply damage to the enemy.
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die(); 
        }
    }

    // Function to handle the enemy's death.
    private void Die()
    {
        gameObject.GetComponent<Animator>().SetBool("Death", true);
    }
}