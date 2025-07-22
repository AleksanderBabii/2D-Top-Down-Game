using UnityEngine;

public interface IEnemyState
{
    void OnEnter(Enemy enemy);
    void OnExit(Enemy enemy);
    void Update(Enemy enemy);
    void FixedUpdate(Enemy enemy);
}

