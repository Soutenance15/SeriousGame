using UnityEngine;
using TMPro;

public class DayDataBinder : MonoBehaviour
{
    [Header("Data du jour")]
    [SerializeField] private DayData dayData;

    [System.Serializable]
    public struct QuestionUI
    {
        [Header("UI pour une question")]
        public TextMeshProUGUI questionText;   // Texte de la question dans le Dialog_X
        public Choice[] choiceButtons;         // Les 3 boutons Choice de Choices_X
    }

    [Header("Mapping QuestionStep -> UI")]
    [SerializeField] private QuestionUI[] questionsUI;

    private void Awake()
    {
        if (dayData == null)
        {
            Debug.LogError("DayData non assigné dans DayDataBinder", this);
            return;
        }

        // ⚠ ICI : on utilise dayData.steps (et plus dayData.questions)
        if (dayData.steps == null || dayData.steps.Length == 0)
        {
            Debug.LogError($"DayData {dayData.name} ne contient aucune step (QuestionStep[] steps vide)", this);
            return;
        }

        int count = Mathf.Min(dayData.steps.Length, questionsUI.Length);

        for (int i = 0; i < count; i++)
        {
            QuestionStep stepData = dayData.steps[i];
            QuestionUI ui = questionsUI[i];

            // 1) Texte de la question dans le Dialog_i
            if (ui.questionText != null)
            {
                // ⚠ On suppose que QuestionStep a un champ "questionText"
                ui.questionText.text = stepData.questionText;
            }
            else
            {
                Debug.LogWarning($"QuestionUI[{i}] n'a pas de questionText assigné dans l'inspector", this);
            }

            // 2) Les choix A/B/C
            if (stepData.choices == null || stepData.choices.Length == 0)
            {
                Debug.LogWarning($"Step {i} de {dayData.name} n'a pas de ChoiceData (choices[] vide)", this);
                continue;
            }

            if (ui.choiceButtons == null || ui.choiceButtons.Length == 0)
            {
                Debug.LogWarning($"QuestionUI[{i}] n'a pas de choiceButtons assignés dans l'inspector", this);
                continue;
            }

            int choiceCount = Mathf.Min(ui.choiceButtons.Length, stepData.choices.Length);

            for (int c = 0; c < choiceCount; c++)
            {
                Choice choiceComp = ui.choiceButtons[c];
                ChoiceData choiceData = stepData.choices[c];

                if (choiceComp == null || choiceData == null)
                    continue;

                // ⚠ Ici on utilise les vrais noms : deltaGlycemie, deltaEnergie, deltaPlaisir
                choiceComp.SetDataAndLabel(
                    choiceData.label,
                    choiceData.feedback,
                    choiceData.deltaGlycemie,
                    choiceData.deltaEnergie,   // int -> float (conversion implicite ok)
                    choiceData.deltaPlaisir    // int -> float
                );
            }
        }
    }
}
