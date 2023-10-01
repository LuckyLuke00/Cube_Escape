using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    // Use waypoints to patrol and randomly choose a waypoint to go to
    private int _destination = 0;

    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Entering Patrol State");
        // Go to closest waypoint
        _destination = GetClosestWaypointTo(enemy, enemy.Player.transform.position);
        enemy.PlayAlertSound = true;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        // If there are no waypoints, do nothing
        if (enemy.Waypoints.Length < 1)
        {
            Debug.LogError("EnemyPatrolState: No waypoints!");
            return;
        }

        // Draw a line to the waypoint
        Debug.DrawLine(enemy.transform.position, enemy.Waypoints[_destination].transform.position, Color.red);

        if (enemy.PlayerInSight())
        {
            enemy.SwitchState(enemy._chaseState);
        }

        // Randomly move around the map
        if (Vector3.Distance(enemy.transform.position, enemy.Waypoints[_destination].transform.position) < .5f && enemy.Waypoints.Length > 1)
        {
            _destination = GetRandomWaypoint(enemy);
        }

        enemy.Agent.destination = enemy.Waypoints[_destination].transform.position;
    }

    private int GetClosestWaypointTo(EnemyStateManager enemy, Vector3 target)
    {
        int closestWaypoint = 0;
        float closestDistance = Mathf.Infinity;

        if (enemy.Waypoints.Length < 1)
        {
            Debug.LogError("No waypoints found!");
            return closestWaypoint;
        }

        for (int i = 0; i < enemy.Waypoints.Length; ++i)
        {
            // Use square distance
            float distance = (enemy.Waypoints[i].transform.position - target).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestWaypoint = i;
            }
        }
        return closestWaypoint;
    }

    private int GetRandomWaypoint(EnemyStateManager enemy)
    {
        return Random.Range(0, enemy.Waypoints.Length);
    }
}