using System.Collections.Generic;
using UnityEngine;
using TMPro; // important pour TMP_Text

public class DialogManager : MonoBehaviour
{
    [Header("Dialogs")]
    [SerializeField] private GameObject panelAlldialog;
    [SerializeField] private NextTextManager nextTextManager;
    [SerializeField] private ChoiceManager choiceManager;

    private List<GameObject> allDialogs;
    private int currentDialogIndex = 0;
    private GameObject currentDialog;

    [Header("UI Feedback")]
    [SerializeField] private GameObject feedbackPanel;   // Panel avec le texte de feedback
    [SerializeField] private TMP_Text feedbackText;      // Texte du feedback

    private void Awake()
    {
        allDialogs = new List<GameObject>();

        foreach (Transform child in panelAlldialog.transform)
        {
            allDialogs.Add(child.gameObject);
        }

        if (allDialogs.Count == 0)
        {
            Debug.LogWarning("[DialogManager] Aucun dialog trouvé dans panelAlldialog.");
            return;
        }

        currentDialogIndex = 0;
        currentDialog = allDialogs[currentDialogIndex];

        // Active le premier dialog et initialise les textes
        currentDialog.SetActive(true);
        nextTextManager.UpdateText(currentDialog);

        // On cache le feedback au départ
        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (choiceManager != null)
        {
            choiceManager.OnChoiceSelectedWithFeedback += ShowFeedback;
        }
        else
        {
            Debug.LogWarning("[DialogManager] ChoiceManager n'est pas assigné dans l'inspector.");
        }
    }

    private void OnDisable()
    {
        if (choiceManager != null)
            choiceManager.OnChoiceSelectedWithFeedback -= ShowFeedback;
    }

    /// <summary>
    /// Appelé quand le joueur a choisi une réponse, avec le texte de feedback.
    /// </summary>
    public void ShowFeedback(string feedback)
    {
        // Si pas de feedback, on enchaîne direct sur le dialog suivant
        if (string.IsNullOrEmpty(feedback))
        {
            NextDialog();
            return;
        }

        if (feedbackPanel != null) feedbackPanel.SetActive(true);
        if (feedbackText != null)  feedbackText.text = feedback;
    }

    /// <summary>
    /// Appelé par le bouton "Continuer" du panel de feedback.
    /// </summary>
    public void OnFeedbackContinue()
    {
        if (feedbackPanel != null) feedbackPanel.SetActive(false);

        // Maintenant seulement on passe à la question / dialog suivant
        NextDialog();
    }

    private void NextDialog()
    {
        // On passe au dialog suivant
        currentDialogIndex += 1;

        if (currentDialogIndex < allDialogs.Count)
        {
            // Désactive l'ancien dialog
            if (currentDialog != null)
                currentDialog.SetActive(false);

            // Met à jour currentDialog avant d'appeler UpdateText
            currentDialog = allDialogs[currentDialogIndex];
            currentDialog.SetActive(true);

            Debug.Log("Il y a un next dialog: " + currentDialog.name);

            // Initialise le NextTextManager avec le nouveau dialog
            nextTextManager.UpdateText(currentDialog);
        }
        else
        {
            Debug.Log("Il n'y a pas de next dialog");

            // 👉 Ici on déclenche la fin de journée / fin du quiz
            if (choiceManager != null)
            {
                Debug.Log("[DialogManager] Fin des dialogs, on appelle TriggerEndOfDay sur ChoiceManager.");
                choiceManager.TriggerEndOfDay();
            }
            else
            {
                Debug.LogWarning("[DialogManager] Impossible d'appeler TriggerEndOfDay : ChoiceManager est null.");
            }
        }
    }
}
