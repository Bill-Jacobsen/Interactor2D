using UnityEngine;

public abstract class InteractionAction : ScriptableObject, IAction
{
    public abstract void Execute(IInteractor interactor, GameObject target);
}
