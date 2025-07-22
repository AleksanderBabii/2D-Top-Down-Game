using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public Transform player { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }

    [Header("Stats")]
    public float maxHealth = 3f;
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1f;

    private float currentHealth;
    private IEnemyState currentState;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        ChangeState(new IdleState());
    }
   
        

    void Update()
    {
        currentState?.Update(this);

    }

    void FixedUpdate()
    {
        currentState?.FixedUpdate(this);

        
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState?.OnExit(this);
        currentState = newState;
        currentState?.OnEnter(this);
    }

    public void TakeDamage(float amount, Vector2 source)
    {
        currentHealth -= amount;

        Vector2 knockDir = (transform.position - (Vector3)source).normalized;
        rb.AddForce(knockDir * 4f, ForceMode2D.Impulse);

        if (currentHealth <= 0)
            Destroy(gameObject); // Or play death animation
    }

}
