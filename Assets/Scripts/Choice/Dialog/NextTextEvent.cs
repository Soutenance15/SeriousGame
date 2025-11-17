using UnityEngine;
using UnityEngine.Events;

// Pour récuperer dans le menu unity
[CreateAssetMenu(fileName = "NewNextTextEvent", menuName = "Events/NextTextEvent")]
public class NextTextEvent : ScriptableObject
{
    UnityAction<Text> listeners;

    public void Raise(Text text)
    {
        if (null != listeners)
        {
            listeners.Invoke(text);
        }
    }

    public void RegisterListener(UnityAction<Text> listener)
    {
        listeners += listener;
    }

    public void UnregisterListener(UnityAction<Text> listener)
    {
        listeners -= listener;
    }
}
