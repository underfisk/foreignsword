using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class AI_Movement : MonoBehaviour
{
    public Transform target { get; private set; }
    public NavMeshAgent agent { get; private set; }
    private Animator animator;

    public void SetTarget(Transform n_target)
    {
        this.target = n_target;
    }

    #region Monster State Machine
    private bool isIdle, isWalking, isAttacking, isDead;

    private void SetIdle(bool v)
    {
        isIdle = v;
        animator.SetBool("isIdle", v);
    }

    private void SetWalking(bool v)
    {
        isWalking = v;
        animator.SetBool("isWalking", v);
    }

    private void SetAttacking(bool v)
    {
        isAttacking = v;
        animator.SetBool("isAttacking", v);
    }

    public void SetDead(bool v)
    {
        isDead = v;
    }
    #endregion

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updatePosition = true;
        animator = GetComponent<Animator>();
        SetAttacking(false);
        SetWalking(false);
        SetIdle(true);
        SetDead(false);
    }

    private void Update()
    {
        if (target != null && !isDead)
        {
            agent.destination = target.transform.position;
            agent.angularSpeed = 1200;
            agent.acceleration = 1000;
            transform.LookAt(target);

            //Check if the monster remaining distance is less than the stopping, if is near or not to the click end point
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                //here soon we will maake him to return to his routine
                SetIdle(true);
                SetWalking(false);
            }
            else
            {
                SetIdle(false);
                SetWalking(true);
            }
        }

    
    }
}
