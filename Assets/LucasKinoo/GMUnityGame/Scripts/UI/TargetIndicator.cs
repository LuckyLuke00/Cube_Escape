using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] private float _hideDistance = 15f;

    private float _sqrMagnitude = 0f;
    private Renderer _arrowRenderer = null;
    private Transform _arrowTransform = null;
    private Transform _target = null;
    private Vector3 _arrowDefaultScale = Vector3.one;

    private void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Enemy").transform;
        if (_target == null)
        {
            Debug.LogError("TargetIndicator: Target is null");
        }
        // Get the transform of the arrow component in the child object
        _arrowTransform = transform.GetChild(0);
        if (_arrowTransform == null)
        {
            Debug.LogError("TargetIndicator: ArrowTranform is null");
        }
        _arrowDefaultScale = _arrowTransform.localScale;

        // From the arrow transform we get the renderer
        _arrowRenderer = GetComponentInChildren<Renderer>();
        if (_arrowRenderer == null)
        {
            Debug.LogError("TargetIndicator: ArrowRenderer is null");
        }

    }

    private void Update()
    {
        // Hide the indicator if the target is on screen or too far away
        if (_target == null)
        {
            return;
        }

        _sqrMagnitude = (_target.position - transform.position).sqrMagnitude;

        if (IsTargetOnScreen() || IsTargetTooFarAway())
        {
            // Hide the indicator
            _arrowRenderer.enabled = false;
            return;
        }
        
        // Show the indicator
        _arrowRenderer.enabled = true;

        ScaleIndicator();

        Vector3 targetPosition = _target.position;
        transform.LookAt(targetPosition);

    }

    private bool IsTargetOnScreen()
    {
        Vector3 targetPosition = _target.position;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetPosition);

        return screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height;
    }

    private bool IsTargetTooFarAway()
    {
        // Check if the target is too far away by using SqrMagnitude and comparing it to the hide distance
        return _sqrMagnitude > _hideDistance * _hideDistance;
    }

    private void ScaleIndicator()
    {
        // Scale the _arrowTransform based on the distance to the target
        // The closer the target the bigger the scale

        // This is a simple formula to get a value between 0 and 1
        float scale = 1 - (_sqrMagnitude / (_hideDistance * _hideDistance));

        // Set the scale of the arrow transform
        _arrowTransform.localScale = new Vector3(scale, scale, scale);

        // Clamp the scale to the default scale
        _arrowTransform.localScale = Vector3.Min(_arrowTransform.localScale, _arrowDefaultScale);

    }
}
