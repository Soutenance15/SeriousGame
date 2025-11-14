using UnityEngine;
using UnityEngine.UI;

public class Text : MonoBehaviour
{
    [SerializeField]
    Button nextTextButton;

    [SerializeField]
    NextTextEvent nextTextEvent;

    void Awake()
    {
        nextTextButton = GetComponent<Button>();
        nextTextButton.onClick.AddListener(() => PublishChoiceEventClick());
    }

    void PublishChoiceEventClick()
    {
        if (nextTextEvent != null)
        {
            nextTextEvent.Raise(this);
        }
    }

    public Button GetNextButton()
    {
        return nextTextButton;
    }
}
