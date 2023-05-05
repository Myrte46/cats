using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkingSpeed;
    public float runningSpeed;
    public float jumpStrength;

    private Rigidbody2D rb;
    private PlayerControls controls;

    private Animator animator;

    private float movement;
    private bool isRunning;

    private Vector3 localScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        localScale = transform.localScale;

        controls = new PlayerControls();

        controls.Player.Movement.performed += ctx => movement = ctx.ReadValue<float>();
        controls.Player.Movement.canceled += _ => movement = 0;

        controls.Player.Jump.started += _ => Jump();

        controls.Player.Sprinting.performed += ctx => isRunning = true;
        controls.Player.Sprinting.canceled += _ => isRunning = false;
    }

    private void OnEnable() => controls.Enable();
    private void onDisable() => controls.Disable();

    private void Update()
    {
        if (movement < 0) //if we're moving left
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        if (movement > 0) //if we're moving right
            transform.localScale = new Vector3(localScale.x, localScale.y, localScale.z);

        animator.SetFloat("Movement", Mathf.Abs(movement));
        animator.SetBool("isRunning", isRunning);
    }

    private void FixedUpdate()
    {
        if(!isRunning)
            rb.velocity = new Vector2(movement * walkingSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(movement * runningSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
    }
}
