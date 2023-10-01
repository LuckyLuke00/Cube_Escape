using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class GateBehaviour : MonoBehaviour
{
    [Tooltip("Makes it so that the gate closes after another key is collected (if the gate has been opened), and stays closed")]
    [SerializeField] private bool _closeAfterCollect = false;
    
    [Tooltip("Makes it so the gate ignores the enemy's state, so it can open even if the enemy is chasing the player")]
    [SerializeField] private bool _ignoreEnemyState = false;
    
    [Tooltip("Makes it so the gate only opens when all collectibles in the current level are collected")]
    [SerializeField] private bool _requireAllCollectiblesToOpen = false;
    
    [Tooltip("Makes it so the gate can only be opened once, and stays open after that")]
    [SerializeField] private bool _stayOpen = false;

    [Tooltip("The speed at which the gate opens and closes")]
    [SerializeField] private float _gateSpeed = 1f;
    
    [Tooltip("The number of collectibles needed to open the gate")]
    [SerializeField] private int _collectiblesToOpen = 0;

    private BoxCollider _collider = null;
    private EnemyStateManager _enemy = null;
    private GameManager _gameManager = null;
    private bool _isOpen = false;
    private bool _overrideClose = false;
    private bool _overrideOpen = false;
    private float _gateHeight = 0.0f; // y-Scale

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GateBehaviour: _gameManager is null!");
            return;
        }

        // Get the navmesh agent and check if it exists
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyStateManager>();
        if (_enemy == null)
        {
            Debug.LogError("EnemyStateManager is null");
            return;
        }

        _collider = GetComponent<BoxCollider>();
        if (_collider == null)
        {
            Debug.LogError("BoxCollider is null");
            return;
        }

        _collider.enabled = true;

        // Get the gate height: y-Scale
        _gateHeight = transform.localScale.y;
    }

    private void Start()
    {
        if (_requireAllCollectiblesToOpen)
        {
            _collectiblesToOpen = Collectible.Total;
            return;
        }

        if (_collectiblesToOpen > Collectible.Total)
        {
            Debug.LogWarning("GateBehaviour: _collectiblesToOpen is greater than the total number of collectibles!");
            _collectiblesToOpen = Collectible.Total;
        }
    }

    private void Update()
    {
        if (_overrideOpen)
        {
            OpenGate();
            return;
        }

        if (_overrideClose)
        {
            CloseGate();
            return;
        }

        if (_isOpen && _stayOpen) return;

        if (_isOpen && (!_ignoreEnemyState && _enemy.CurrentState == _enemy._chaseState) || (_closeAfterCollect && _collectiblesToOpen + 1 <= _gameManager.Collectibles))
        {
            CloseGate();
            return;
        }

        if ((!_isOpen && _collectiblesToOpen <= _gameManager.Collectibles) && (_ignoreEnemyState || _enemy.CurrentState != _enemy._chaseState))
        {
            OpenGate();
            return;
        }
    }

    private void OpenGate()
    {
        // When the gate is at half the height of the gate, stop moving the gate
        if (transform.position.y > -_gateHeight / 2)
        {
            transform.Translate(Vector3.down * _gateSpeed * Time.deltaTime);
            return;
        }

        // Set gate's height to half the gate's height (to prevent floating point errors)
        transform.position = new Vector3(transform.position.x, -_gateHeight / 2, transform.position.z);

        // Disable box collider when the gate is down
        _collider.enabled = false;
        _isOpen = true;
    }
    
    private void CloseGate()
    {
        // Enable box collider when the gate is closing
        _collider.enabled = true;

        if (transform.position.y < _gateHeight / 2)
        {
            transform.Translate(Vector3.up * _gateSpeed * Time.deltaTime);
            return;
        }
        
        transform.position = new Vector3(transform.position.x, _gateHeight / 2, transform.position.z);
        _isOpen = false;
    }
    public void OverrideGate(bool open)
    {
        if (open)
        {
            _overrideOpen = true;
        }
        else
        {
            _overrideClose = true;
        }
    }
}