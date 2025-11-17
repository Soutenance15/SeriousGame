using UnityEngine;
using UnityEngine.Events;

// Pour récuperer dans le menu unity
[CreateAssetMenu(fileName = "NewChoiceEvent", menuName = "Events/ChoiceEvent")]
public class ChoiceEvent : ScriptableObject
{
    UnityAction<Choice> listeners;

    public void Raise(Choice choice)
    {
        if (null != listeners)
        {
            listeners.Invoke(choice);
        }
    }

    public void RegisterListener(UnityAction<Choice> listener)
    {
        listeners += listener;
    }

    public void UnregisterListener(UnityAction<Choice> listener)
    {
        listeners -= listener;
    }
}
