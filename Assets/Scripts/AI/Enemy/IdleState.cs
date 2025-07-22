using UnityEngine;

public class IdleState : IEnemyState
{
    public void OnEnter(Enemy enemy)
    {
        enemy.rb.linearVelocity = Vector2.zero;
    }

    public void Update(Enemy enemy)
    {
        float distance = Vector2.Distance(enemy.transform.position, enemy.player.position);
        if (distance < enemy.detectionRange)
        {
            enemy.ChangeState(new ChaseState());
        }
    }

    public void FixedUpdate(Enemy enemy) { }

    public void OnExit(Enemy enemy) { }
}
