using UnityEngine;

public class PatrolState : IEnemyState
{
    private int currentPoint = 0;
    private float idleTimer = 0f;
    private float idleDuration = 2f;
    private bool isIdling = false;

    public void OnEnter(Enemy enemy)
    {
        enemy.animator.SetTrigger("Patrol");
    }

    // Update is called once per frame
    public void Update(Enemy enemy)
    {
        if (enemy.patrolPoints.Length == 0) return;

        // Check if player is within detection range
        float playerDistance = Vector2.Distance(enemy.transform.position, enemy.player.position);

        //Debug.Log("PatrolState Updating");

        // Log the result of CanSeePlayer
       // Debug.Log("CanSeePlayer(): " + enemy.CanSeePlayer());

        if (enemy.CanSeePlayer())
        {
            Debug.Log("Player detected — switching to ChaseState");
            enemy.ChangeState(new ChaseState());
            isIdling = false;
            return;
        }

        Transform target = enemy.patrolPoints[currentPoint];
        float distance = Vector2.Distance(enemy.transform.position, target.position);

        if (isIdling)
        {
            // Stay idle
            enemy.rb.linearVelocity = Vector2.zero;
            enemy.animator.SetFloat("EnemySpeed", 0f);

            idleTimer += Time.deltaTime;
            if (idleTimer >= idleDuration)
            {
                idleTimer = 0f;
                isIdling = false;
                currentPoint = (currentPoint + 1) % enemy.patrolPoints.Length;

                // Start moving to the next patrol point
                enemy.animator.SetTrigger("Patrol");
                Vector2 direction = (target.position - enemy.transform.position).normalized;
                enemy.rb.linearVelocity = direction * enemy.moveSpeed;
                enemy.UpdateLookDirection(direction); // Update look direction for animation

                // Set animation parameters
                enemy.animator.SetFloat("EnemyMoveX", direction.x);
                enemy.animator.SetFloat("EnemyMoveY", direction.y);
                enemy.animator.SetFloat("EnemySpeed", direction.sqrMagnitude);
                return;
            }
        }
        else if (distance < 0.1f)
        {
            // Reached patrol point, start idling
            isIdling = true;
            idleTimer = 0f;
            enemy.rb.linearVelocity = Vector2.zero;
            enemy.animator.SetFloat("EnemySpeed", 0f);
        }
        else
        {
            // Move toward patrol point
            Vector2 direction = (target.position - enemy.transform.position).normalized;
            enemy.rb.linearVelocity = direction * enemy.moveSpeed;
            enemy.UpdateLookDirection(direction); // Update look direction for animation

            // Set animation parameters
            enemy.animator.SetFloat("EnemyMoveX", direction.x);
            enemy.animator.SetFloat("EnemyMoveY", direction.y);
            enemy.animator.SetFloat("EnemySpeed", direction.sqrMagnitude);

        }
    }
    public void FixedUpdate(Enemy enemy)
    {
    }
    public void OnExit(Enemy enemy)
    {
        enemy.patrolTimer = 0f;
    }
}
