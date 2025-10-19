using UnityEngine;

[CreateAssetMenu(fileName = "DoorOpenAction", menuName = "Interactor2D/Actions/Open Door")]
public class DoorOpenAction : InteractionAction
{
    public override void Execute(IInteractor interactor, GameObject target)
    {
        var door = target.GetComponent<IDoor>();
        if (!door.IsOpen)
        {
            door.Open();
        }
        else
        {
            door.Close();
        }
    }
}