using UnityEngine;

[CreateAssetMenu(menuName = "Events/Vector2 Event Channel")]
public class Vector2EventChannel : GenericEventChannel<Vector2Event>
{
}

[System.Serializable]
public struct Vector2Event
{
    public Vector2 Value;

    public Vector2Event(Vector2 value)
    {
        Value = value;
    }
}