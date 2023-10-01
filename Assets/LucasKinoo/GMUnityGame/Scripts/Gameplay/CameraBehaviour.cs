using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 5f;
    [SerializeField] private float _cameraHeight = 5f;

    private GameObject _player = null;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        if (_player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }
        transform.position = CalculateCameraPosition();
    }

    private void LateUpdate()
    {
        if (_player != null)
        {
            // Smoothly move the camera, and prevent the camera from jittering
            transform.position = Vector3.Lerp(transform.position, CalculateCameraPosition(), Time.deltaTime * _followSpeed);
        }
    }

    private Vector3 CalculateCameraPosition()
    {
        Vector3 position = _player.transform.position;
        position.y += _cameraHeight;
        return position;
    }
}