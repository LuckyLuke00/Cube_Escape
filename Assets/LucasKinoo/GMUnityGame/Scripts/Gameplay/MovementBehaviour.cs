using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] protected float _movementSpeed = 10f;

    // Declare reference variables
    private CharacterController _characterController = null;

    // Variables to store player input values
    private Vector3 _currentMovement = Vector3.zero;

    public Vector3 CurrentMovement
    {
        get { return _currentMovement; }
        set { _currentMovement = value; }
    }

    protected GameObject _target = null;

    public GameObject Target
    {
        get { return _target; }
        set { _target = value; }
    }

    protected virtual void Awake()
    {
        // Initially set reference variables
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
    }

    protected virtual void HandleMovement()
    {
        _characterController.Move(_currentMovement * _movementSpeed * Time.deltaTime);
    }
}