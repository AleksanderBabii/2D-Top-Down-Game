using UnityEngine;


[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from the player
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize the movement vector to prevent faster diagonal movement
        movement = movement.normalized;

        //Animate
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        /*        animator.SetFloat("MoveX", 1f);
                animator.SetFloat("MoveY", 0);
                animator.SetFloat("Speed", 1f);*/

        Debug.Log($"X: {movement.x}, Y: {movement.y}, Speed: {movement.sqrMagnitude}5");


    }
    private void FixedUpdate()
    {
        // Move the player
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        //Debug.Log("RB Position: " + rb.position + " | Move Vector: " + movement.normalized);
    }
}

