using UnityEngine;

/// <summary>
/// Script for knockback effect.
/// Adds a force to an enemy to give a knockback effect
/// </summary>
public class Knockback : MonoBehaviour
{
    public float thrust;

    /// <summary>
    /// This function adds a force to an enemy
    /// to knock them back when they are attacked.
    /// </summary>
    /// <param name="other">The game object with a RigidBody2D component 
    /// that will get knocked back</param>
    public void DoKnockback(GameObject other) {
        Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
        if (enemy != null) {
            Vector2 difference = (Vector2)enemy.transform.position - (Vector2)transform.position;
            difference = difference.normalized * thrust;
            enemy.AddForce(difference, ForceMode2D.Impulse);
        }
        
    }
}
