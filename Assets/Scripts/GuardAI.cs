using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float patrolSpeed = 2f;
    public float waitTime = 3f; // Wait time at each waypoint

    private NavMeshAgent agent;
    private Animator animator;
    private int currentWaypoint = 0;
    private bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = patrolSpeed;
        MoveToNextWaypoint();
    }

    void Update()
    {
        if (!isWaiting && !agent.pathPending && agent.remainingDistance < 0.1f)
        {
            StartCoroutine(IdlePause());
        }
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        agent.SetDestination(waypoints[currentWaypoint].position);
        animator.SetBool("isWalking", true);
        animator.SetBool("isLooking", false);

        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    }

    IEnumerator IdlePause()
    {
        isWaiting = true;

        agent.isStopped = true; // Stop the agent completely
        animator.SetBool("isWalking", false);
        animator.SetBool("isLooking", true);

        yield return new WaitForSeconds(waitTime); // Wait for 3 seconds

        animator.SetBool("isLooking", false);
        agent.isStopped = false; // Resume movement
        isWaiting = false;

        MoveToNextWaypoint();
    }
}
