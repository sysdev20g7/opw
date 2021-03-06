using UnityEngine;

/// <summary>
/// Represents the health of an object.
/// The object also needs a ZeroHealthBehavior component to function correctly.
/// </summary>
public class Health : MonoBehaviour
{
    //Allows for different noHealthBehavior for when health reaches zero.
    [SerializeField] private ZeroHealthBehavior noHealthBehavior;

    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    //default non-zero values for when max- and current health
    //not set in Unity Inspector.
    private readonly int defaultMaxHealth = 8;
    private readonly int defaultCurrentHealth = 8;

    //Events for subscribers, such as healthbar to Subscribe to.
    public delegate void HealDelegate(int amount);
    public event HealDelegate HealEvent;

    public delegate void DamageDelegate(int amount);
    public event DamageDelegate DamageEvent;

    void Start()
    {
        noHealthBehavior = GetComponent<ZeroHealthBehavior>();
        if (noHealthBehavior == null) Debug.Log("No Health behavior is missing from " + this.gameObject);
        if (maxHealth == 0) maxHealth = defaultMaxHealth;
        if (currentHealth == 0) currentHealth = defaultCurrentHealth;
    }

    /// <summary>
    /// Checks if current health reaches 0
    /// then calls destroys object behavior.
    /// </summary>
    void Update()
    {
        if (currentHealth == 0 && noHealthBehavior != null) {
            noHealthBehavior.ZeroHealthAction();
        }
        else if (noHealthBehavior == null) {
            Debug.Log(this + " has a ZeroHealthBehavior missing, won't be destroyed.");
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
        DamageEvent?.Invoke(currentHealth - newHealth);
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
        HealEvent?.Invoke(newHealth - currentHealth);
        currentHealth = newHealth;
        Debug.Log("Healing current health with " +
            amount + " to " + currentHealth);

    }

    /// <summary>
    /// Returns the current health of the object.
    /// </summary>
    /// <returns>currentHealth</returns>
    public int GetCurrentHealth() {
        return this.currentHealth;
    }

    /// <summary>
    /// Returns the max health of the object.
    /// </summary>
    /// <returns>maxHealth</returns>
    public int GetMaxHealth() {
        return this.maxHealth;
    }

    /// <summary>
    /// Sets the health of the object.
    /// Does not allow negative values or zero.
    /// </summary>
    /// <param name="newHealth"></param>
    public void SetHealth(int newHealth) {
        if (newHealth <= 0) return;
        this.currentHealth = Mathf.Min(this.maxHealth,newHealth);
    }

    /// <summary>
    /// Sets the new max health of the object.
    /// Does not allow negative values or zero.
    /// </summary>
    /// <param name="newMaxHealth"></param>
    public void SetMaxHealth(int newMaxHealth) {
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
    public void ChangeMaxHealthByValue(int maxHealthChange) {
        if (maxHealth + maxHealthChange <= 0) return;
        this.maxHealth += maxHealthChange;
        if (maxHealth < currentHealth) this.currentHealth = maxHealth;
    }
}
