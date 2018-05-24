using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : MonoBehaviour
{

    /// <summary>
    /// NPC state, this script is binded to each monster to each one is different
    /// </summary>
    private bool isDeadState, isIdleState, isAttackingState, isWalkingState;
    [SerializeField] Animator mAnimator;
    public bool isDead { get { return isDeadState; } }
    public bool isIdle { get { return isIdleState; } }
    public bool isAttacking { get { return isAttackingState; } }
    public bool isWalking { get { return isWalkingState; } }

    private void Start()
    {
        mAnimator = GetComponent<Animator>();
    }

    public void SetDead(bool v)
    {
        this.isDeadState = v;
        mAnimator.SetBool("isDead", v);
    }

    public void SetIdle(bool v)
    {
        this.isIdleState = v;
        mAnimator.SetBool("isIdle", v);
    }

    public void SetAttacking(bool v)
    {
        this.isAttackingState = v;
        mAnimator.SetBool("isAttacking", v);
    }

    public void SetWalking(bool v)
    {
        this.isWalkingState = v;
        mAnimator.SetBool("isWalking", v);
    }
}
