using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField]
    GameObject panelAlldialog;

    [SerializeField]
    NextTextManager nextTextManager;

    List<GameObject> allDialogs;

    int currentDialogIndex = 0;
    GameObject currentDialog;

    void Awake()
    {
        // nextTextManager = GetComponent<NextTextManager>();

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
    }

    void OnEnable()
    {
        nextTextManager.StartNewDialog += NextDialog;
    }

    void OnDisable()
    {
        nextTextManager.StartNewDialog -= NextDialog;
    }

    void NextDialog()
    {
        // Désactive l'ancien dialog
        if (currentDialog != null)
            currentDialog.SetActive(false);

        // Avance l'index
        currentDialogIndex += 1;

        if (currentDialogIndex < allDialogs.Count)
        {
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
        }
    }
}
