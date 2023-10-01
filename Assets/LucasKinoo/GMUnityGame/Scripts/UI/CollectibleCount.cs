using UnityEngine;

public class CollectibleCount : MonoBehaviour
{
    [SerializeField] private string _collectibleName = "Collectible";

    private int _count = 0;
    private TMPro.TMP_Text _text = null;

    private void Awake()
    {
        // The text component is a child of this object
        _text = GetComponentInChildren<TMPro.TMP_Text>();

        if (_text == null)
        {
            Debug.LogError("CollectibleCount: Text is null!");
        }
    }

    private void Start()
    {
        // Needs to be called in start because the collectibles are instantiated at runtime
        UpdateCount();
    }

    private void OnEnable()
    {
        Collectible.OnCollectibleCollected += OnCollectibleCollected;
    }

    private void OnDisable()
    {
        Collectible.OnCollectibleCollected -= OnCollectibleCollected;
    }

    private void OnCollectibleCollected()
    {
        ++_count;
        UpdateCount();
    }

    private void UpdateCount()
    {
        // Display string in "Collectible: [ 0 / 3 ]" format
        _text.text = $"{_collectibleName}: [ {_count} / {Collectible.Total} ]";
    }
}