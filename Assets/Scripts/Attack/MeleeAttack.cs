using UnityEngine;

/// <summary>
/// Represents the melee attack of attack capable Game Object.
/// The attack capable object can have a reference to Animator
/// to notify the attack state for animations. 
/// </summary>
public abstract class MeleeAttack : MonoBehaviour {
    [SerializeField] public bool IsAttacking { get; set; }
    [SerializeField] public int attackDmg { get; set; }
    //Default non-zero values for when max- and current health
    //not set in Unity Inspector.
    protected readonly int defaultAttackDmg = 1;

    [SerializeField] protected float attacksPerSecond = 1f;
    protected float nextAttackTime = 0f;

    protected Animator anim;
}