

public interface IInteractable
{
    void Interact(IInteractor interactor);
    TriggerMode Mode { get;  }
}
