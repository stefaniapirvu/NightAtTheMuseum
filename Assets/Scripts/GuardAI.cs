using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    [SerializeField] float radius = 10.0f;
    [SerializeField] List<GameObject> waypoint;
    [SerializeField] int currentWaypoint = 0;

    [SerializeField] float walkSpeed = 3.5f;  
    [SerializeField] float runSpeed = 6.0f;   

    NavMeshAgent agent;
    Animator animator;
    private bool isInvestigatingGrenade = false;
    private Vector3 grenadePosition;
    private float investigationTimer = 0.0f;
    private const float investigationDuration = 3.0f;  
    private bool hasArrivedAtGrenade = false;
    private GameObject grenadeObject;  

    public GridManager gridManager;
    private bool hasDetectedPlayer = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = walkSpeed;
    }

    void Update()
    {
        if (isInvestigatingGrenade)
        {
            if (!hasArrivedAtGrenade)
            {
                if (Vector3.Distance(transform.position, grenadePosition) < 1.0f) 
                {
                    agent.isStopped = true;
                    hasArrivedAtGrenade = true;
                    investigationTimer = 0.0f;
                    gridManager.UpdateQValue(grenadePosition, -1f);
                }
                animator.SetBool("isLooking", false);

            }
            else
            {
                Destroy(grenadeObject, investigationDuration);
                investigationTimer += Time.deltaTime;

                if (investigationTimer < investigationDuration)
                {
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isLooking", true);

                    agent.speed = walkSpeed;
                }
                else
                {
                    isInvestigatingGrenade = false;
                    hasArrivedAtGrenade = false;
                    investigationTimer = 0.0f;
                    agent.isStopped = false;  
                    animator.SetBool("isLooking", false);


                    if (Vector3.Distance(transform.position, PlayerMovement.instance.player.transform.position) < radius)
                    {
                        agent.SetDestination(PlayerMovement.instance.player.transform.position);
                        animator.SetBool("isRunning", true);
                        agent.speed = runSpeed;
                    }
                    else
                    {
                        agent.SetDestination(waypoint[currentWaypoint].transform.position);
                        animator.SetBool("isRunning", false);
                        agent.speed = walkSpeed;
                    }
                }
            }
        }
        else
        {
            animator.SetBool("isLooking", false);

            if (Vector3.Distance(transform.position, PlayerMovement.instance.player.transform.position) < radius)
            {
                if (!hasDetectedPlayer)
                {
                    gridManager.UpdateQValue(PlayerMovement.instance.player.transform.position, -1f);  // Update grid only once
                    hasDetectedPlayer = true;  // Set flag to true so it doesn't update the grid again
                }
                FaceTarget();
                agent.SetDestination(PlayerMovement.instance.player.transform.position);
                agent.speed = runSpeed;  
                animator.SetBool("isRunning", true);  
            }
            else
            {
                if (waypoint.Count == 0) return;

                agent.SetDestination(waypoint[currentWaypoint].transform.position);

                agent.speed = walkSpeed;  
                animator.SetBool("isRunning", false); 

                if (Vector3.Distance(transform.position, waypoint[currentWaypoint].transform.position) < 3.0f)
                {
                    currentWaypoint++;
                    if (currentWaypoint >= waypoint.Count)
                    {
                        currentWaypoint = 0;
                    }
                }
                if (hasDetectedPlayer)
                {
                    hasDetectedPlayer = false;  // Reset flag so that we can update the grid again if the player is detected later
                }
            }
        }
    }

    public void GoToGrenade(Vector3 grenadeLocation, GameObject grenade)
    {
        isInvestigatingGrenade = true;
        grenadePosition = grenadeLocation;
        grenadeObject = grenade;  
        agent.SetDestination(grenadePosition); 
        hasArrivedAtGrenade = false;  
    }

    void FaceTarget()
    {
        Vector3 direction = (PlayerMovement.instance.player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
