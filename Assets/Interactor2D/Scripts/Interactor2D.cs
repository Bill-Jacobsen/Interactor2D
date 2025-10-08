using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu( "Interaction/Interactor 2D" )]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Interactor2D : MonoBehaviour, IInteractor
{
    [Header("Interact Input")]
    [Tooltip("Assign a Button-type InputAction (e.g., Player/Interact).")]
    [SerializeField] private InputActionReference interactAction;
    
    // Interactable we're currently overlapping (null if none).
    private Interactable2D _currentInteractable;
    public Transform Transform => transform;
    
    private void Awake()
    {
        // Ensure the player's collider is not a trigger so we receive trigger callbacks.
        var col = GetComponent<Collider2D>();
        if (col && col.isTrigger)
        {
            col.isTrigger = false;
        }
    }
    
    // Enable the assigned action when active.
    private void OnEnable()
    {
        if (interactAction?.action != null)
        {
            interactAction.action.Enable();
        }
        else
        {
            Debug.LogWarning("[Interactor2D] No interact action assigned.", this);
        }
    }
    
    private void Update()
    {
        if (interactAction?.action == null) return;  // No action assigned.
        
        if (interactAction.action.WasPressedThisFrame())
        {
            if (_currentInteractable != null)
            {
                // Interact goes here
            }
            else
            {
                Debug.Log("[Interactor2D] Interact pressed, but no interactable found.");
            }
        }

    }
    
    // Disable the action and clear _currentInteractable when inactive.
    private void OnDisable()
    {
        if (interactAction?.action != null)
        {
            interactAction.action.Disable();
        }

        _currentInteractable = null;
    }
    
    // Called when the component is first added, or you click reset in the inspector.
    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = false;  // Ensure the collider is not a trigger for trigger callbacks.
    }
    
    // Editor-time guard: keep the player's collider a NON-trigger if someone toggles it on.
    private void OnValidate()
    {
        var col = GetComponent<Collider2D>();
        if (col && col.isTrigger) col.isTrigger = false;
    }
    
    // Cache the interactable if we overlap it.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Interactable2D>(out Interactable2D interactable))
        {
            _currentInteractable = interactable;
        }

    }
    
    // Clear it when we leave that same interactable.
    private void OnTriggerExit2D(Collider2D other)
    {
        // If we don't currently have a target, nothing to clear.
        if (_currentInteractable == null) return;
        // Did the thing we exited even have an Interactable2D on it?
        if (!other.TryGetComponent<Interactable2D>(out var interactable)) return;
        // Is the thing we exited the same as the thing we had cached?
        if (interactable != _currentInteractable) return;
        
        _currentInteractable = null;
    }
}
