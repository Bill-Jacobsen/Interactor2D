using UnityEngine;

public class DoorController : MonoBehaviour, IDoor
{
    [SerializeField] private Vector3 openOffset = new Vector3(0, 3, 0);
    [SerializeField] private float openTime = 0.25f;

    private Vector3 _closedPos;
    public bool IsOpen { get; private set; }
    private void Awake()
    {
        _closedPos = transform.position;
    }
    
    public void Open()
    {
        if (IsOpen) return;
        IsOpen = true;

        transform.position = _closedPos + openOffset;
        Debug.Log($"[Door] Opened door {name}");
    }
    
    public void Close()
    {
        if (!IsOpen) return;
        IsOpen = false;
        transform.position = _closedPos;
        Debug.Log($"[Door] Closed door {name}");
    }
}
