using UnityEngine;
using UnityEngine.UI;

public class Choice : MonoBehaviour
{
    Button buttonChoice;

    [SerializeField]
    string textChoice;

    [SerializeField]
    string messageFinal;

    [SerializeField]
    float glycemie;

    [SerializeField]
    float pleasure;

    [SerializeField]
    float energy;

    [SerializeField]
    ChoiceEvent choiceEvent;

    void Awake()
    {
        buttonChoice = GetComponent<Button>();
        buttonChoice.onClick.AddListener(() => PublishChoiceEventClick());
    }

    void PublishChoiceEventClick()
    {
        if (choiceEvent != null)
        {
            choiceEvent.Raise(this);
        }
    }

    // Setter & Getter
    public string GetTextChoice()
    {
        return textChoice;
    }

    public string GetMessageFinal()
    {
        return messageFinal;
    }

    public float GetGlycemie()
    {
        return glycemie;
    }

    public float GetEnergy()
    {
        return energy;
    }

    public float GetPleasure()
    {
        return pleasure;
    }
}
