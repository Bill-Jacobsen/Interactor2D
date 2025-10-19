using UnityEngine;

public class DoorController : MonoBehaviour, IDoor
{
    [Header("Motion")]
    [Tooltip("Local offset from the closed position to the open position.")]
    [SerializeField] private Vector3 openOffset = new Vector3(0, 3, 0);
    
    [Tooltip("Duration of the open animation (in seconds).")]
    [Min(0.01f)]
    [SerializeField] private float openTime = 0.25f;
    
    private Vector3 _closedLocalPos;
    private Vector3 _startLocalPos;
    private Vector3 _endLocalPos;
    
    private float _doorMovingTime;
    private bool _isMoving;
    
    public bool IsOpen { get; private set; }
    
    private void Awake()
    {
        _closedLocalPos = transform.localPosition;
        IsOpen = false;
    }

    private void Update()
    {
        if (!_isMoving) return;
        
        // Advance the animation progress and calculate the alpha value.
        float duration = Mathf.Max(0.0001f, openTime);
        _doorMovingTime += Time.deltaTime;
        float alpha = Mathf.Clamp01(_doorMovingTime / duration);
        
        // Move the door between the start and end positions linearly in local space.
        transform.localPosition = Vector3.Lerp(_startLocalPos, _endLocalPos, alpha);
        
        // If the animation is complete, set the door to the end position and stop moving.
        if (_doorMovingTime >= duration)
        {
            transform.localPosition = _endLocalPos;
            _isMoving = false;
        }
    }

    private void BeginMove(bool toOpen)
    {
        // Start from wherever the door currently is.
        _startLocalPos = transform.localPosition;
        
        // Choose the target: closed pos OR closed pos + open offset (which is the open pose).
        _endLocalPos = toOpen ? _closedLocalPos + openOffset : _closedLocalPos;
        
        // Reset the animation progress and mark as moving.
        _doorMovingTime = 0f;
        _isMoving = true;
    }
    
    public void Open()
    {
        if (IsOpen && !_isMoving) return;
        IsOpen = true;

        BeginMove(true);
        Debug.Log($"[Door] Opening door {name}");
    }
    
    public void Close()
    {
        if (!IsOpen && !_isMoving) return;
        IsOpen = false;
        BeginMove(false);
        Debug.Log($"[Door] Closing door {name}");
    }
}
