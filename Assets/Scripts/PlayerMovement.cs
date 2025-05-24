using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float hideSpeed = 2f;
    public float runSpeed = 8f;
    private float currentSpeed;
    public float rotationSpeed = 200f;
    private CharacterController controller;
    private Animator animator;
    private Vector3 moveDirection;


    public GameObject smokeGrenadePrefab; 
    public Transform throwPoint; 
    public float throwForce = 3f;
    public static PlayerMovement instance;
    public GameObject player;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentSpeed = speed;
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");

    }

    void Update()
    {
        float moveZ = Input.GetAxis("Vertical");  // W/S - Înainte/Înapoi
        float rotateY = Input.GetAxis("Horizontal"); // A/D - Rota?ie

        // Rotim juc?torul în jurul axei Y (stânga/dreapta)
        transform.Rotate(0, rotateY * rotationSpeed * Time.deltaTime, 0);

        // Calcul?m direc?ia de deplasare pe baza privirii actuale
        moveDirection = transform.forward * moveZ;

        // Dac? juc?torul apas? W/S, se deplaseaz?
        if (moveZ != 0)
        {
            controller.Move(moveDirection * currentSpeed * Time.deltaTime);
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Ascundere
            currentSpeed = hideSpeed;
            animator.SetBool("isHiding", true);
            animator.SetBool("isRunning", false);
        }
        else if (Input.GetKey(KeyCode.RightShift) && moveZ != 0)
        {
            // Alergare
            currentSpeed = runSpeed;
            animator.SetBool("isRunning", true);
            animator.SetBool("isHiding", false);
        }
        else
        {
            // Mers normal
            currentSpeed = speed;
            animator.SetBool("isRunning", false);
            animator.SetBool("isHiding", false);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowGrenade();
        }

    }


    void ThrowGrenade()
    {
        animator.SetTrigger("throw"); // Animatie de aruncare

        // Creaza grenada de fum
        GameObject grenade = Instantiate(smokeGrenadePrefab, throwPoint.position, throwPoint.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        if (rb != null)
        {
            //rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
            rb.AddForce((transform.forward + transform.up * 0.5f) * throwForce, ForceMode.VelocityChange);

        }
    }
}
