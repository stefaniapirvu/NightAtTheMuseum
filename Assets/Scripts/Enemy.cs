//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.AI;

//public class Enemy : MonoBehaviour
//{
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public class GuardAI : MonoBehaviour
//{
//    [SerializeField] float radius = 10.0f;
//    [SerializeField] List<GameObject> waypoint;
//    [SerializeField] int currentWaypoint = 0;

//    [SerializeField] float walkSpeed = 3.5f;  // viteza normala
//    [SerializeField] float runSpeed = 5.0f;

//    NavMeshAgent agent;
//    Animator animator;


//    private bool isInvestigatingGrenade = false;
//    private Vector3 grenadePosition;
//    private float investigationTimer = 0.0f;
//    private const float investigationDuration = 3.0f;

//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();
//        animator = GetComponent<Animator>();
//        agent.speed = walkSpeed;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (isInvestigatingGrenade)
//        {
//            investigationTimer += Time.deltaTime;

//            if (investigationTimer < investigationDuration)
//            {
//                // Se duce la grenad?
//                agent.SetDestination(grenadePosition);
//                animator.SetBool("isRunning", false);
//                agent.speed = walkSpeed;  // Viteza de mers normal?
//            }
//            else
//            {
//                // Dup? ce investigheaz? grenada, se întoarce la patrulare sau urm?rire
//                isInvestigatingGrenade = false;
//                investigationTimer = 0.0f;

//                if (Vector3.Distance(transform.position, PlayerMovement.instance.player.transform.position) < radius)
//                {
//                    // Dac? juc?torul este înc? în raza de urm?rire
//                    agent.SetDestination(PlayerMovement.instance.player.transform.position);
//                    animator.SetBool("isRunning", true);
//                    agent.speed = runSpeed;
//                }
//                else
//                {
//                    // Dac? nu, continu? patrularea
//                    agent.SetDestination(waypoint[currentWaypoint].transform.position);
//                    animator.SetBool("isRunning", false);
//                    agent.speed = walkSpeed;
//                }
//            }
//        }
//        else
//        {
//            // Comportamentul normal de patrulare sau urm?rire
//            if (Vector3.Distance(transform.position, PlayerMovement.instance.player.transform.position) < radius)
//            {
//                FaceTarget();
//                agent.SetDestination(PlayerMovement.instance.player.transform.position);
//                agent.speed = runSpeed;  // cre?tem viteza
//                animator.SetBool("isRunning", true);  // comut?m pe animatia de running
//            }
//            else
//            {
//                if (waypoint.Count == 0) return;

//                agent.SetDestination(waypoint[currentWaypoint].transform.position);

//                agent.speed = walkSpeed;  // revenim la viteza de mers
//                animator.SetBool("isRunning", false); // revenim la animatia de walking

//                if (Vector3.Distance(transform.position, waypoint[currentWaypoint].transform.position) < 3.0f)
//                {
//                    currentWaypoint++;
//                    if (currentWaypoint >= waypoint.Count)
//                    {
//                        currentWaypoint = 0;
//                    }
//                }
//            }
//        }
//    }

//    public void GoToGrenade(Vector3 grenadeLocation)
//    {
//        // Activ?m comportamentul de investigare a grenadei
//        isInvestigatingGrenade = true;
//        grenadePosition = grenadeLocation;
//    }

//    void FaceTarget()
//    {
//        Vector3 direction = (PlayerMovement.instance.player.transform.position - transform.position).normalized;
//        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
//        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
//    }


//    void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(transform.position, radius);
//    }
//}
////using System.Collections;
////using System.Collections.Generic;
////using UnityEngine;
////using UnityEngine.AI;



////public class GuardAI : MonoBehaviour
////{
////    public Transform[] waypoints;
////    public float patrolSpeed = 4f;
////    public float waitTime = 1.5f; // Wait time at each waypoint
////    [SerializeField] float followRadius = 15.0f;

////    private NavMeshAgent agent;
////    private Animator animator;
////    private int currentWaypoint = 0;
////    private bool isWaiting = false;

////    void Start()
////    {
////        agent = GetComponent<NavMeshAgent>();
////        animator = GetComponent<Animator>();

////        agent.speed = patrolSpeed;
////        MoveToNextWaypoint();
////    }

////    void Update()
////    {
////        PatrolWaypoints();

////        float distanceToPlayer = Vector3.Distance(transform.position, PlayerMovement.instance.transform.position);
////        if (distanceToPlayer < followRadius)
////        {
////            FaceTarget();
////            agent.SetDestination(PlayerMovement.instance.transform.position);
////            animator.SetBool("isWalking", false);
////            agent.speed = 6.0f;
////            animator.SetBool("iRunning", true);



////        }
////        else
////        {
////            PatrolWaypoints();
////        }

////    }

////    void PatrolWaypoints()
////    {
////        if (!isWaiting && !agent.pathPending && agent.remainingDistance < 0.1f)
////        {
////            StartCoroutine(IdlePause());
////        }
////    }

////    void FaceTarget()
////    {
////        Vector3 direction = (PlayerMovement.instance.transform.position - transform.position).normalized;
////        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
////        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
////    }

////    void MoveToNextWaypoint()
////    {
////        if (waypoints.Length == 0) return;

////        agent.SetDestination(waypoints[currentWaypoint].position);
////        animator.SetBool("isWalking", true);
////        animator.SetBool("isLooking", false);

////        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
////    }

////    IEnumerator IdlePause()
////    {
////        isWaiting = true;

////        agent.isStopped = true; // Stop the agent completely
////        animator.SetBool("isWalking", false);
////        animator.SetBool("isLooking", true);

////        yield return new WaitForSeconds(waitTime); // Wait for 3 seconds

////        animator.SetBool("isLooking", false);
////        agent.isStopped = false; // Resume movement
////        isWaiting = false;

////        MoveToNextWaypoint();
////    }
////}

//}
