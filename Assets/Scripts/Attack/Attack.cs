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
public abstract class Attack : MonoBehaviour {
    [SerializeField]
    protected int attackDmg { get; set; }
    //Default non-zero values for when max- and current health
    //not set in Unity Inspector.
    private readonly int defaultAttackDmg = 1;

    protected bool IsAttacking { get; set; }
    [SerializeField]
    protected float attackTime;
    [SerializeField]
    protected float startTimeAttack;

    protected Animator anim;

}