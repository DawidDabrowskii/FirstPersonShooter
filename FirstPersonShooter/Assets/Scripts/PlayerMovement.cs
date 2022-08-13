using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Settings")]
    [SerializeField] float speed = 15f;
    [SerializeField] float gravity = -10f;
    [SerializeField] float jumpHeight = 2f;

    private Vector3 dir;
    private Vector3 velocity;
    private bool isGrounded;



    private void Update()
    {
        BasicMovement();
        Jumping();
    }

    private void BasicMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        dir = transform.right * x + transform.forward * z;

        controller.Move(dir * speed * Time.deltaTime);

    }
    private void Jumping()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, groundLayer);

        if (isGrounded && velocity.y < 0)
            velocity.y = -1f;

        if (isGrounded)
        {
            if(Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * 2 * -gravity);
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }
}
