using System;
using UnityEngine;
using UnityEngine.InputSystem;

[AddComponentMenu( "Interaction/Interactor 2D" )]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Interactor2D : MonoBehaviour, IInteractor
{
    public event Action<IInteractor, IInteractable> Interacted;
    
    [Header("Interact Input")]
    [Tooltip("Assign a Button-type InputAction (e.g., Player/Interact).")]
    [SerializeField] private InputActionReference interactAction;
    
    // Interactable we're currently overlapping (null if none).
    private IInteractable _currentInteractable;
    public IInteractable CurrentInteractable => _currentInteractable;
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
        var action = interactAction?.action;
        if (action == null)
        {
            Debug.LogWarning("[Interactor2D] No interact action assigned.", this);
            return;
        }

        action.Enable();
        action.performed += OnInteractPerformed;

    }
    
    // Disable the action and clear _currentInteractable when inactive.
    private void OnDisable()
    {
        var action = interactAction?.action;
        if (action != null)
        {
            action.performed -= OnInteractPerformed;
            action.Disable();
        }
        _currentInteractable = null;

    }
    
    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        if (_currentInteractable == null)
        {
            Debug.Log("[Interactor2D] Interact performed, but no interactable found.");
            return;
        }
        
        // Do the thing
        _currentInteractable.Interact(this);
        
        Interacted?.Invoke(this, _currentInteractable);
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
        var i = other.GetComponentInParent<IInteractable>();
        if (i != null) _currentInteractable = i;
    }
    
    // Clear it when we leave that same interactable.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (_currentInteractable == null) return;
        var i = other.GetComponentInParent<IInteractable>();
        if (i != _currentInteractable) return;
        _currentInteractable = null;
    }
}
