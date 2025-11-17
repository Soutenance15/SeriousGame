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
        // ⚠️ On ne laisse plus ChoiceManager déclencher tout seul le "Next"
        // choiceManager.StartNewDialog += NextDialog;

        // On branche un event ou une méthode pour recevoir le feedback
        choiceManager.OnChoiceSelectedWithFeedback += ShowFeedback;
    }

    private void OnDisable()
    {
        // choiceManager.StartNewDialog -= NextDialog;
        if (choiceManager != null)
            choiceManager.OnChoiceSelectedWithFeedback -= ShowFeedback;
    }

    /// <summary>
    /// Appelé quand le joueur a choisi une réponse, avec le texte de feedback.
    /// </summary>
    public void ShowFeedback(string feedback)
    {
        // Si pas de feedback, on enchaîne direct
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

        // Maintenant seulement on passe à la question suivante
        NextDialog();
    }

    private void NextDialog()
    {
        currentDialogIndex += 1;

        if (currentDialogIndex < allDialogs.Count)
        {
            // Désactive l'ancien dialog
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
            // ici tu peux désactiver le canvas, lancer une autre scène, etc.
        }
    }
}
