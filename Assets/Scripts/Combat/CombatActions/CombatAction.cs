using System;
using UnityEngine;

public abstract class CombatAction
{
    public abstract event Action OnActionComplete;
    public abstract void ExecuteAction();

}
