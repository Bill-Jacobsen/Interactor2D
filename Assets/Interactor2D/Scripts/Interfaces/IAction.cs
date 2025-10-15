
using UnityEngine;

public interface IAction
{
    void Execute(IInteractor interactor, GameObject target);
}
