using UnityEngine;

public class ChaseState : IEnemyState
{
    private float lostPlayerTimer = 0f;
    private float lostPlayerDuration = 2f; // Time before returning to patrol if player is lost

    public void OnEnter(Enemy enemy) 
    {
        //Debug.Log("Entered ChaseState");
        enemy.animator.SetTrigger("Chase");
        lostPlayerTimer = 0f; // Reset lost player timer
    }

    public void Update(Enemy enemy)
    {
        float distance = Vector2.Distance(enemy.transform.position, enemy.player.position);

        // Player is within attack range
        if (distance < enemy.attackRange)
        {
            enemy.ChangeState(new AttackState());
            return;
        }

        // Player is outside detection range
        if (!enemy.CanSeePlayer())
        {
            lostPlayerTimer += Time.deltaTime;
            if (lostPlayerTimer >= lostPlayerDuration)
            {
                enemy.ChangeState(new PatrolState());
                return;
            }
        }
        else
        {
            // Player back in detection range, reset timer
            lostPlayerTimer = 0f;
            enemy.CanSeePlayer(); // Ensure player is still visible
        }

        // Direction logic duplicated here so animation has access to it
        Vector2 direction = (enemy.player.position - enemy.transform.position).normalized;
        enemy.rb.linearVelocity = direction * enemy.moveSpeed;
        enemy.UpdateLookDirection(direction); // Update look direction for animation

        // Set animation parameters
        enemy.animator.SetFloat("EnemyMoveX", direction.x);
        enemy.animator.SetFloat("EnemyMoveY", direction.y);
        enemy.animator.SetFloat("EnemySpeed", direction.sqrMagnitude);
        
        if (enemy.player == null)
            return;

    }

    public void FixedUpdate(Enemy enemy)
    {
 
    }

    public void OnExit(Enemy enemy)
    {
        enemy.rb.linearVelocity = Vector2.zero;
        enemy.animator.SetFloat("EnemySpeed", 0f);
    }

  
}
