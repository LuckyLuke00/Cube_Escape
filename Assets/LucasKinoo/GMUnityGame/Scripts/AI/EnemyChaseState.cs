using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{
    private float _timer = 0f;

    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Entering Chase State");
        
        if (enemy.PlayAlertSound)
            SoundManager._instance.PlaySound(SoundManager._instance.AlertSound);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.Player == null) return;

        _timer += Time.deltaTime;

        if (_timer > enemy.TimeToKeepChasing && !enemy.PlayerInSight())
        {
            _timer = 0f;

            enemy.SwitchState(enemy._searchState);
        }

        enemy.LastKnownLocation = enemy.Player.transform.position;
        enemy.Agent.destination = enemy.LastKnownLocation;

        if (enemy.Agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            enemy.Agent.destination = enemy.Agent.pathEndPosition;
        }
    }
}