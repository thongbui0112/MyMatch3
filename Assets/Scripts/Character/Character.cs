using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character: MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] protected GameObject targetAttack;

    public Animator Animator { get => animator; set => animator = value; }

    public virtual void Attacking() {

    }

    public virtual void GetAttacking() {

    }

    public virtual void Dieing() {

    }

}
