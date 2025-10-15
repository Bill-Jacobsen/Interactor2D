using UnityEngine;

public interface IDoor
{
    void Open();
    void Close();
    bool IsOpen { get; }
}
