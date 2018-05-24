using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMesh))]
[RequireComponent(typeof(Animator))]
public class NPC_Movement : MonoBehaviour
{
    [SerializeField,Tooltip("Set the cords/transform for the NPC move into, it will follow the order of index starting on 0")] Vector3[] Waypoints;
    [SerializeField] float TimeToWait; //time after he move to stay on that position
    [SerializeField] bool RandomWaypoints;
    private Animator animator;
    private NavMeshAgent agent;
    private int currentWaypointIndex;

    private bool isIdle, isWalking, isTalking;

    private void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        isIdle = true;
        isWalking = false;
        isTalking = false;
        animator.SetBool("isIdle", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isTalking", false);

        currentWaypointIndex = 0;
        if (agent == null) agent = GetComponent<NavMeshAgent>();

        if (Waypoints.Length >= 0)
        {
            if (!RandomWaypoints)
                StartCoroutine(Move(currentWaypointIndex, TimeToWait));
            else
                StartCoroutine(Move(Random.Range(0,Waypoints.Length), TimeToWait));
        }
    }

    private void Update()
    {
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            isIdle = false;
            isWalking = true;

            if (currentWaypointIndex >= Waypoints.Length)
                currentWaypointIndex = 0;

            if (currentWaypointIndex <= Waypoints.Length)
            {
                StartCoroutine(Move(currentWaypointIndex, TimeToWait));
                currentWaypointIndex++;
            }
        }
        else
        {
            isIdle = true;
            animator.SetBool("isIdle", true);
            isWalking = false;
            animator.SetBool("isWalking", true);
        }
    }

    IEnumerator Move(int index, float timeDelay)
    {

        yield return new WaitForSecondsRealtime(timeDelay);

        if (RandomWaypoints)
            agent.SetDestination(Waypoints[Random.Range(0, Waypoints.Length)]);
        else
            agent.SetDestination(Waypoints[index]);
    }
}
