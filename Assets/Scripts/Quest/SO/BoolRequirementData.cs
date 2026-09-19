using UnityEngine;

public class BoolRequirementData: RequirementData
{
    private bool defaultValue;
    private bool _value;
    public bool value
    {
        get => _value;
        set
        {
            _value = value;
            RaiseRequirementChanged();
        }
    }
    public BoolRequirementData(BoolRequirementSO config)
    {
        Config = config;
        value = config.defaultValue;
        defaultValue = config.defaultValue;
    }

    public override void Reset()
    {
        value = defaultValue;
    }

    public override bool IsMet() => value;
}
