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
        
        //if condition passed, attack again 
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("Enemy attacks!");
            lastAttackTime = Time.time;
            // Deal damage to player
            // Trigger attack logic
            enemy.animator.SetTrigger("Attack");
            PerformAttack(enemy);
        }
    }

    public void FixedUpdate(Enemy enemy) { }

    public void OnExit(Enemy enemy) { }

    public void PerformAttack(Enemy enemy)
    {
        //Use animation events or direct logic here
        Debug.Log("Enemy attacks player!");

        //Example: Deal damage to player
        if (Vector2.Distance(enemy.transform.position, enemy.player.position) < enemy.attackRange)
        {
           enemy.player.GetComponent<HealthSystem>()?.TakeDamage(1); // Optional script
        }
    }
}