using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class UserInput : MonoBehaviour
{
    public static UserInput Instance { get; private set; }
    
    private Controls controls;
    
    public Vector2 Move { get; private set; }
    public bool InteractPressedThisFrame => controls.Player.Interact.WasPressedThisFrame();
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        controls = new Controls();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();
    
    private void Update()
    {
        Move = controls.Player.Move.ReadValue<Vector2>();
    }
    


    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        controls?.Dispose();
    }
}
