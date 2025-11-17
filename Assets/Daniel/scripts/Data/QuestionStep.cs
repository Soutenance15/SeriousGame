using UnityEngine;

[System.Serializable]
public class QuestionStep
{
    [Header("Texte affiché dans le dialogue")]
    [TextArea(2, 5)]
    public string questionText;

    [Header("3 choix possibles (A / B / C)")]
    public ChoiceData[] choices = new ChoiceData[3];
}
