using UnityEngine;

public abstract class AttackBehaviour : MonoBehaviour 
{
    protected bool attacking;

    public bool isAttacking() {
        return this.attacking;
    }

}