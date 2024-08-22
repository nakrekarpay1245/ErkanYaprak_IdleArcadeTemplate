using UnityEngine;
using UnityEngine.Events;

public class TopDownCharacterCollector : MonoBehaviour, ICollector
{
    [Header("Collector Settings")]
    [SerializeField, Tooltip("The radius within which collectable items can be detected.")]
    private float collectRadius = 5f;

    [Header("Events")]
    [SerializeField, Tooltip("Action triggered when an item is collected.")]
    private UnityAction<ICollectable> OnCollect;

    private Collider[] collidersInRange;

    private void Awake()
    {
        // Initialize the collider array based on expected size
        collidersInRange = new Collider[10]; // Adjust size based on expected number of items
    }

    private void Update()
    {
        // Check for collectable items within the specified radius
        DetectAndCollectItems();
    }

    /// <summary>
    /// Detects and collects items within the collect radius.
    /// </summary>
    private void DetectAndCollectItems()
    {
        // Use Physics.OverlapSphere to find colliders within the collect radius
        int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, collectRadius, collidersInRange);

        for (int i = 0; i < colliderCount; i++)
        {
            Collider collider = collidersInRange[i];
            if (collider != null && collider.TryGetComponent(out ICollectable collectable))
            {
                Collect(collectable);
            }
        }
    }

    /// <summary>
    /// Collects a given collectable item and triggers the onCollect event.
    /// </summary>
    /// <param name="collectable">The collectable item to collect.</param>
    public void Collect(ICollectable collectable)
    {
        collectable.OnCollect(this);
        OnCollect?.Invoke(collectable);
    }

    /// <summary>
    /// Draws the collect radius in the Scene view for debugging purposes.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, collectRadius);
    }
}