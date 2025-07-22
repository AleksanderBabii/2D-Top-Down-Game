using UnityEngine;

public class ChaseState : IEnemyState
{
    public void OnEnter(Enemy enemy) {
        enemy.animator.SetTrigger("Chase");
    }

    public void Update(Enemy enemy)
    {
        float distance = Vector2.Distance(enemy.transform.position, enemy.player.position);

        if (distance > enemy.detectionRange)
        {
            enemy.ChangeState(new IdleState());
            return;
        }
        else if (distance < enemy.attackRange)
        {
            enemy.ChangeState(new AttackState());
            return;
        }

        // Direction logic duplicated here so animation has access to it
        Vector2 direction = (enemy.player.position - enemy.transform.position).normalized;

        // Set animation parameters
        enemy.animator.SetFloat("EnemyMoveX", direction.x);
        enemy.animator.SetFloat("EnemyMoveY", direction.y);
        enemy.animator.SetFloat("EnemySpeed", direction.sqrMagnitude);
    }

    public void FixedUpdate(Enemy enemy)
    {
        if (enemy.player == null)
            return;

        Vector2 direction = (enemy.player.position - enemy.transform.position).normalized;
        enemy.rb.velocity = direction * enemy.moveSpeed;

    }

    public void OnExit(Enemy enemy)
    {
        enemy.rb.linearVelocity = Vector2.zero;
        enemy.animator.SetFloat("EnemySpeed", 0f);
    }
}
