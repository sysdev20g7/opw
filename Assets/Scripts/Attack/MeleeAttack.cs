using UnityEngine;

/// <summary>
/// Represents the attack of attack capable Game Object.
/// The attack capable game object needs a AttackPoint game object as child
/// to determine if a game object is in range. 
/// If this other game object is an opposable game object,
/// the attack capable game object can then attack this opposable game object.
/// 
/// The attack capable object needs an Animator and an AttackBehavior component
/// to function properly. 
/// </summary>
public abstract class MeleeAttack : MonoBehaviour {
    [SerializeField]
    public int attackDmg { get; set; }
    //Default non-zero values for when max- and current health
    //not set in Unity Inspector.
    protected readonly int defaultAttackDmg = 1;

    [SerializeField]
    public bool IsAttacking { get; set; }
    protected float attackTime = 0.25f;
    protected float attackCounter = 0.25f;

    //Attack rate per second
    [SerializeField]
    protected float attackRate = 1f;
    protected float nextAttackTime = 0f;

    protected Animator anim;
    protected Rigidbody2D rb;

}