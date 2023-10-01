using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] private float _TimeToKeepChasing = .5f;
    [SerializeField] private Material _GhostMaterial = null;
    
    private GameObject[] _waypoints = null;

    private EnemyBaseState _currentState = null;
    public EnemyChaseState _chaseState = new EnemyChaseState();
    public EnemyPatrolState _patrolState = new EnemyPatrolState();
    public EnemySearchState _investigateState = new EnemySearchState();
    public EnemySearchState _searchState = new EnemySearchState();

    private GameObject _player = null;
    private GameObject _PlayerGhostMesh = null;
    private NavMeshAgent _agent = null;
    private Vector3 _lastKnownLocation = Vector3.zero;
    
    // This variable is used to play the alert sound only when EnterState is called by patrol state
    // This is to prevent it from playing a lot
    private bool _playAlertSound = true;

    // Getters and setters
    public bool PlayAlertSound { get => _playAlertSound; set => _playAlertSound = value; }
    public EnemyBaseState CurrentState { get => _currentState; }
    public float TimeToKeepChasing { get => _TimeToKeepChasing; }
    public GameObject Player { get => _player; set => _player = value; }
    public GameObject PlayerGhostMesh { get => _PlayerGhostMesh; }
    public GameObject[] Waypoints { get => _waypoints; set => _waypoints = value; }
    public Material GhostMaterial { get => _GhostMaterial; }
    public NavMeshAgent Agent { get => _agent; set => _agent = value; }
    public Vector3 LastKnownLocation { get => _lastKnownLocation; set => _lastKnownLocation = value; }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        // Waypoints are tagged with: "Waypoint"
        _waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        // Nullchecks
        if (_player == null)
        {
            Debug.LogError("EnemyStateManager: _player is null!");
            return;
        }

        if (_agent == null)
        {
            Debug.LogError("EnemyStateManager: _agent is null!");
            return;
        }

        if (_waypoints == null)
        {
            Debug.LogError("EnemyStateManager: _waypoints is null!");
            return;
        }

        _PlayerGhostMesh = new GameObject();
        _PlayerGhostMesh.AddComponent<MeshFilter>().mesh = _player.GetComponentInChildren<MeshFilter>().mesh;
        _PlayerGhostMesh.AddComponent<MeshRenderer>().material = _GhostMaterial;
        _PlayerGhostMesh.transform.localScale = _player.transform.localScale;
        _PlayerGhostMesh.transform.rotation = _player.transform.rotation;
        _PlayerGhostMesh.SetActive(false);

        // Disable rotation
        _agent.updateRotation = false;

        // Set initial state
        _currentState = _patrolState;
        _currentState.EnterState(this);
    }

    private void Update()
    {
        _currentState.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState state)
    {
        _currentState = state;
        _currentState.EnterState(this);
    }

    public bool PlayerInSight()
    {
        if (_player == null) return false;

        RaycastHit hit;
        return Physics.Raycast(transform.position, Player.transform.position - transform.position, out hit) && hit.collider.gameObject.tag == "Player";
    }

    public void FadeMesh(MeshRenderer mesh, float duration, float targetAlpha)
    {
        while (mesh.material.color.a != targetAlpha)
        {
            mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, Mathf.MoveTowards(mesh.material.color.a, targetAlpha, Time.deltaTime / duration));
        }
    }
}