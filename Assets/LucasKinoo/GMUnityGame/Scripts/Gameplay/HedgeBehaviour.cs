using UnityEngine;
using UnityEngine.AI;

public class HedgeBehaviour : MonoBehaviour
{
    private NavMeshAgent _agent = null;
    private BoxCollider _collider = null;
    private EnemyStateManager _enemy = null;

    private void Awake()
    {
        // Get the navmesh agent and check if it exists
        _agent = GameObject.FindGameObjectWithTag("Enemy").GetComponent<NavMeshAgent>();
        if (_agent == null)
        {
            Debug.LogError("NavMeshAgent is null");
            return;
        }

        // Get the boxcollider and check if it exists
        _collider = GetComponent<BoxCollider>();
        if (_collider == null)
        {
            Debug.LogError("BoxCollider is null");
            return;
        }

        // Get the enemy state manager from the agent
        _enemy = _agent.GetComponent<EnemyStateManager>();
        if (_enemy == null)
        {
            Debug.LogError("EnemyStateManager is null");
            return;
        }
    }

    // When the navmesh agent is in chase mode, disable the box collider else enable it
    private void Update()
    {
        if (_enemy.CurrentState == _enemy._chaseState)
        {
            _collider.enabled = false;
            return;
        }
        _collider.enabled = true;
    }
}