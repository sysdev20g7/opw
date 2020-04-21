using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the health of an object.
/// </summary>
public class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int currentHealth;

    void Start()
    {
        //Want object to start with max health.
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Lets object be damaged.
    /// </summary>
    /// <param name="amount"></param>
    private void TakeDamage(int amount) {
        if (amount <0) {
            return;
        }
        if (!((currentHealth - amount) < 0)) {
            currentHealth -= amount;
        }
    }

    private void Heal(int amount) {
        if (amount < 0) {
            return;
        }
        if ((currentHealth + amount) > maxHealth) {
            currentHealth = maxHealth;
        }
        else {
            currentHealth += amount;
        }
    }

}
