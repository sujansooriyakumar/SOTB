using System;
using UnityEngine;

public abstract class RequirementData
{
    public RequirementSO Config { get; protected set; }
    public abstract void Reset();
    public abstract bool IsMet();
    public event Action<RequirementData> onRequirementUpdated;

    protected void RaiseRequirementChanged()
    {
        onRequirementUpdated?.Invoke(this);
    }

}
