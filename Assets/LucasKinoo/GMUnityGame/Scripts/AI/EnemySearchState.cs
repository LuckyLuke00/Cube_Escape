using UnityEngine;
using UnityEngine.AI;

public class EnemySearchState : EnemyBaseState
{
    private float _lastKnownLocationDistance = 0f;
    private const float _reachDistance = 1f;

    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Entering Search State");
        SpawnTransparentPlayerMesh(enemy);
        enemy.PlayAlertSound = false;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        // Keep chasing the player for 5 seconds If the player is not in sight during that time,
        // switch to investigate state
        if (enemy.PlayerInSight())
        {
            enemy.PlayerGhostMesh.SetActive(false);

            enemy.SwitchState(enemy._chaseState);
        }

        _lastKnownLocationDistance = Vector3.Distance(enemy.transform.position, enemy.LastKnownLocation);

        if (_lastKnownLocationDistance < _reachDistance)
        {
            enemy.PlayerGhostMesh.SetActive(false);

            enemy.SwitchState(enemy._patrolState);
        }

        enemy.Agent.destination = enemy.LastKnownLocation;

        if (enemy.Agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            enemy.Agent.destination = enemy.Agent.pathEndPosition;
        }
    }

    private void SpawnTransparentPlayerMesh(EnemyStateManager enemy)
    {
        // Spawn a semi transparent player mesh at the last known location of the player This will
        // be used to show the player where the enemy last saw the player
        enemy.PlayerGhostMesh.transform.position = enemy.LastKnownLocation;
        enemy.PlayerGhostMesh.SetActive(true);
    }
}