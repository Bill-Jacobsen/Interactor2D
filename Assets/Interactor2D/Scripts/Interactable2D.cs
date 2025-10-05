using UnityEngine;

[AddComponentMenu( "Interaction/Interactable 2D" )]
[DisallowMultipleComponent]
public class Interactable2D : MonoBehaviour
{
    private enum GizmoMode { 
        Off, 
        SelectedOnly, 
        Always
    }
    
    [Header("Gizmos")]
    [Tooltip("Off = no gizmo, SelectedOnly = show when selected, Always = always show in Scene View.")]
    [SerializeField] private GizmoMode gizmoMode = GizmoMode.SelectedOnly;
    
    private void Awake()
    {
        var col = GetComponent<Collider2D>();
        if (!col)
        {
            Debug.LogError
                ("[Interactor2D] Missing Collider2D at runtime. Interaction will not work.", this);
            return;
        }

        if (!col.isTrigger)
        {
            col.isTrigger = true;
        }
    }
    
    // Temporary debug for testing. Replace it with real behavior later.
    public void Interact()
    {
        Debug.Log($"[Interactor2D] Interacted with: {name}");
    }
    
    // Get collider and set it to trigger.
    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col)
        {
            col.isTrigger = true;
        }
    }
    
    // Keep existing collider a trigger while editing.
    private void OnValidate()
    {
        var col = GetComponent<Collider2D>();
        if (col && !col.isTrigger)
        {
            col.isTrigger = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (gizmoMode != GizmoMode.SelectedOnly) return;
        DrawTriggerGizmo();
    }

    private void OnDrawGizmos()
    {
        if (gizmoMode != GizmoMode.Always) return;
        DrawTriggerGizmo();
    }
    
    // Scene gizmo: draw from whatever Collider2D exists (works for all shapes).
    private void DrawTriggerGizmo()
    {
        var col = GetComponent<Collider2D>();
        if (!col) return;

        var b = col.bounds; // world-space AABB for any 2D collider.
        var center = b.center;
        var size   = b.size;

        Gizmos.color = new Color(0f, 0.8f, 0f, 0.25f);
        Gizmos.DrawCube(center, new Vector3(size.x, size.y, 0.01f));

        Gizmos.color = new Color(0f, 0.8f, 0f, 1f);
        Gizmos.DrawWireCube(center, new Vector3(size.x, size.y, 0.01f));
    }
}
