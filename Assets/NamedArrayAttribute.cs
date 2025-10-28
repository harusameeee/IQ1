using UnityEngine;

public sealed class NamedArrayAttribute : PropertyAttribute
{
    public NamedArrayAttribute(string displayName)
    {
        DisplayName = displayName;
    }
    public NamedArrayAttribute(string[] displayNames)
    {
        DisplayNames = displayNames;
    }
    public string DisplayName { get; }
    public string[] DisplayNames { get; }
}
