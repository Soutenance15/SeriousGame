using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    // ---------------------------------------------------
    //  AJOUT : Setters pour remplir Choice depuis DayData
    // ---------------------------------------------------

    /// <summary>
    /// Remplit les données internes du Choice
    /// (appelé par DayDataBinder)
    /// </summary>
    public void SetData(string textChoice, string messageFinal,
                        float glycemie, float energy, float pleasure)
    {
        this.textChoice = textChoice;
        this.messageFinal = messageFinal;
        this.glycemie = glycemie;
        this.energy = energy;
        this.pleasure = pleasure;
    }

    /// <summary>
    /// Remplit les données internes + met à jour le texte du bouton
    /// (si un TextMeshPro existe dans les enfants du bouton)
    /// </summary>
    public void SetDataAndLabel(string textChoice, string messageFinal,
                                float glycemie, float energy, float pleasure)
    {
        SetData(textChoice, messageFinal, glycemie, energy, pleasure);

        // Mettre à jour le label visuel du bouton
        TextMeshProUGUI tmp = GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
        {
            tmp.text = textChoice;
        }
    }

    // ----------------------
    // Getters existants
    // ----------------------
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
