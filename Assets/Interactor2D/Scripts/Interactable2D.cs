using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu( "Interaction/Interactable 2D" )]
[DisallowMultipleComponent]
public class Interactable2D : MonoBehaviour, IInteractable
{
    private enum GizmoMode { 
        Off, 
        SelectedOnly, 
        Always
    }

    
    [Header("Trigger Mode")]
    [Tooltip("How this interactable activates:\n" +
             "Input - press the assigned button.\n" +
             "Pickup - auto when the Interactor enters (coins, ignores input).\n" +
             "PressurePlate - auto on enter AND on exit (step-on/step-off).")]
    [SerializeField] private TriggerMode triggerMode = TriggerMode.Input;
    
    [Header("Actions (executed in order)")] [SerializeField]
    private InteractionAction action;
    
    [Header("Action Target (optional)")]
    [Tooltip("If set, the action will be executed on this GameObject instead of this Interactable.")]
    [SerializeField] private GameObject actionTargetOverride;
    
    [Header("On Interact")] [Tooltip("Invoked when this object is interacted with.")] [SerializeField]
    private UnityEvent onInteract = new ();
    
    [Header("Gizmos")]
    [Tooltip("Off = no gizmo, SelectedOnly = show when selected, Always = always show in Scene View.")]
    [SerializeField] private GizmoMode gizmoMode = GizmoMode.SelectedOnly;
    
    public TriggerMode Mode => triggerMode;
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

    public void Interact(IInteractor interactor)
    {
        var target = actionTargetOverride != null ? actionTargetOverride : gameObject;
        if (action != null)
        {
            action?.Execute(interactor, target);
        }
        onInteract?.Invoke();
        Debug.Log($"[Interactor2D] Interacting with {name}", this);
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
