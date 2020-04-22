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
    //default non-zero values for when max- and current health
    //not set in Unity Inspector.
    private int defaultMaxHealth = 8;
    private int defaultCurrentHealth = 8;

    //Events for subscribers, such as healthbar to subscribe to.
    public delegate int HealDelegate(int amount);
    public event HealDelegate healEvent;

    public delegate int DamageDelegate(int amount);
    public event DamageDelegate damageEvent;

    //Allows for different destroyBehavior for when health reaches zero.
    private DestroyBehavior destroyBehavior;

    void Start()
    {
        destroyBehavior = GetComponent<DestroyBehavior>();
        if (destroyBehavior == null) Debug.Log("Destroy behavior is missing from " + this.gameObject);
        if (maxHealth == 0) maxHealth = defaultMaxHealth;
        if (currentHealth == 0) currentHealth = defaultCurrentHealth;
    }

    /// <summary>
    /// Checks if current health reaches 0
    /// then calls destroys object behavior.
    /// </summary>
    void Update() {
        if (currentHealth == 0) {
            destroyBehavior.destroyObject();
        }    
    }

    /// <summary>
    /// Lets object be damaged. Negative values not allowed.
    /// Raises an damageEvent when object is damaged.
    /// </summary>
    /// <param name="amount"></param> The amount damaged.
    public void TakeDamage(int amount) {
        if (amount <0) return;

        int newHealth = Mathf.Max((currentHealth - amount), 0);
        if (damageEvent != null) {
            damageEvent(newHealth - currentHealth);
        }
        currentHealth = newHealth;
        Debug.Log("Damaging current health with " +
                    amount + " to " + currentHealth);
    }

    /// <summary>
    /// Lets object be healed. Negative values not allowed.
    /// Raises and healEvent when object is healed.
    /// </summary>
    /// <param name="amount"></param> The amount healed.
    public void Heal(int amount) {
        if (amount < 0) return;

        int newHealth = Mathf.Min((currentHealth + amount), maxHealth);
        if (healEvent != null) {
            healEvent(currentHealth - newHealth);
        }
        currentHealth = newHealth;
        Debug.Log("Healing current health with " +
            amount + " to " + currentHealth);

    }

    /// <summary>
    /// Returns the current health of the object.
    /// </summary>
    /// <returns>currentHealth</returns>
    public int getCurrentHealth() {
        return this.currentHealth;
    }

    /// <summary>
    /// Returns the max health of the object.
    /// </summary>
    /// <returns>maxHealth</returns>
    public int getMaxHealth() {
        return this.maxHealth;
    }

    /// <summary>
    /// Sets the new max health of the object.
    /// Does not allow negative values or zero.
    /// </summary>
    /// <param name="newMaxHealth"></param>
    public void setMaxHealth(int newMaxHealth) {
        if (newMaxHealth <= 0) return;
        this.maxHealth = newMaxHealth;
    }

    /// <summary>
    /// Changes the new max health of the object by said given amount.
    /// Allows for negative values.
    /// Does not allow to change max health to, or below, zero.
    /// If max health set to below current health of object
    /// current health is set to max health.
    /// </summary>
    /// <param name="maxHealthChange"></param>
    public void changeMaxHealthByValue(int maxHealthChange) {
        if (maxHealth + maxHealthChange <= 0) return;
        this.maxHealth += maxHealthChange;
        if (maxHealth < currentHealth) this.currentHealth = maxHealth;
    }
}
