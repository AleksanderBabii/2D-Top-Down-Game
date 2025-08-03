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
    public float attackRange = 1f;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float patrolWaitTime = 1.5f;

    [HideInInspector] public int currentPatrolIndex;
    [HideInInspector] public float patrolTimer = 1f;

    [Header("Vision Settings")]
    public float detectionRadius = 5f;
    [Range(0, 360)]
    public float fieldOfView = 90f;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    public Vector2 lookDirection { get; private set; } = Vector2.right;

    [HideInInspector] public bool canSeePlayer;

    private float currentHealth;
    private IEnemyState currentState;
    private Enemy enemy;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        ChangeState(new IdleState());

       // Debug.Log("Enemy Start called"); //Check if enemy even exists
        currentState = new PatrolState();
        currentState.OnEnter(this);
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

    public bool CanSeePlayer()
    {
        

        if (player == null || enemy == null) return false;

        Vector2 origin = transform.position;
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(origin, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            float angle = Vector2.Angle(enemy.lookDirection, directionToPlayer);

            if (angle < fieldOfView / 2f)
            {
                /*RaycastHit2D hit = Physics2D.Raycast(origin, directionToPlayer, detectionRadius, obstacleLayer | playerLayer);
                Debug.DrawRay(origin, directionToPlayer * detectionRadius, Color.red);

                if (hit.collider != null && hit.transform == player)
                    return true;*/

                Debug.DrawRay(transform.position, directionToPlayer * detectionRadius, Color.red);

                // TEMPORARY: skip angle check
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRadius, obstacleLayer | playerLayer);
                Debug.DrawRay(transform.position, directionToPlayer * detectionRadius, Color.green);
                if (hit && hit.transform == player)
                {
                    Debug.Log("Raycast hit the player!");
                    return true;
                }
            }
        }
        return false;
    }
    
    public void UpdateLookDirection(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0.01f)
            lookDirection = direction.normalized;
    }

}
