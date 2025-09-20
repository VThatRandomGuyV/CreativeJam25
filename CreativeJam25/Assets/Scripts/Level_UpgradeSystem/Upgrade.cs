using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    protected string upgradeName;
    protected string description;

    public string Name { get { return name; } set { name = value; } }

    public string Description { get { return description; } set { description = value; } }

    public abstract void ApplyUpgrade(GameObject player);
}
