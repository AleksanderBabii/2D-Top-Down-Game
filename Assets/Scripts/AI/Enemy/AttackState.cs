using UnityEngine;

public class AttackState : IEnemyState
{
    private float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;

    public void OnEnter(Enemy enemy)
    {
        lastAttackTime = Time.time;
        enemy.rb.linearVelocity = Vector2.zero;
        // Trigger attack animation
        Debug.Log("Enemy enters attack state");
        enemy.animator.SetTrigger("IsAttacking?");
    }

    public void Update(Enemy enemy)
    {
        float distance = Vector2.Distance(enemy.transform.position, enemy.player.position);
        if (distance > enemy.attackRange)
        {
            enemy.ChangeState(new ChaseState());
            return;
        }

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("Enemy attacks!");
            lastAttackTime = Time.time;
            // Deal damage to player
        }
    }

    public void FixedUpdate(Enemy enemy) { }

    public void OnExit(Enemy enemy) { }

    private void PerformAttack(Enemy enemy)
    {
        // Logic to deal damage to the player
        Debug.Log("Performing attack on player");
        // Example: player.GetComponent<Player>().TakeDamage(enemy.attackDamage);
    }
}