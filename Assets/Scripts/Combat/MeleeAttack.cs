using UnityEngine;

/// <summary>
/// Represents the melee attack of attack capable Game Object.
/// The attack capable object can have a reference to Animator
/// to notify the attack state for animations. 
/// </summary>
public abstract class MeleeAttack : MonoBehaviour {
    [SerializeField] protected bool IsAttacking;
    [SerializeField] protected int attackDmg;
    //Default non-zero values for when max- and current health
    //not set in Unity Inspector.
    protected readonly int defaultAttackDmg = 1;
    [SerializeField] protected float attacksPerSecond = 1f;
    protected float nextAttackTime = 0f;

    protected Animator anim;

    /// <summary>
    /// Returns wether Game Object is attacking or not.
    /// Added as get method, as C# properties not exposed to Unity.
    /// </summary>
    /// <returns>True, if attacking.</returns>
    public bool GetIsAttacking() {
        return this.IsAttacking;
    }

    /// <summary>
    /// Returns the attack damage of the Game Object.
    /// Added as get method, as C# properties not exposed to Unity.
    /// </summary>
    /// <returns>The attack damage of the Game Object.</returns>
    public int GetAttackDmg() {
        return this.attackDmg;
    }

    /// <summary>
    /// Sets a new melee attack damage for the Game Object.
    /// Will not allow negative attack damage.
    /// </summary>
    /// <param name="newAttackDmg"></param>
    public void SetAttackDmg(int newAttackDmg) {
        if (newAttackDmg < 0) return;
        this.attackDmg = newAttackDmg;
    }

    /// <summary>
    /// Attacks opposing Game Object(s).
    /// </summary>
    protected abstract void Attack();
}