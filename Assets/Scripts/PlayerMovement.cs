using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float hideSpeed = 2f;
    private float currentSpeed;
    public float rotationSpeed = 200f;
    private CharacterController controller;
    private Animator animator;
    private Vector3 moveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentSpeed = speed;
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

        // Getting Down (Shift)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = hideSpeed;
            animator.SetBool("isHiding", true);
        }
        else
        {
            currentSpeed = speed;
            animator.SetBool("isHiding", false);
        }
    }
}
