using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField]
    ChoiceEvent choiceEvent;

    private void OnEnable()
    {
        if (choiceEvent != null)
            choiceEvent.RegisterListener(OnChoiceClick);
    }

    private void OnDisable()
    {
        if (choiceEvent != null)
            choiceEvent.UnregisterListener(OnChoiceClick);
    }

    void OnChoiceClick(Choice choice)
    {
        Debug.Log(choice.GetTextChoice());
        Debug.Log(choice.GetMessageFinal());
    }
}
