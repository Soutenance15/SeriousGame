using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextTextManager : MonoBehaviour
{
    [SerializeField] private NextTextEvent nextTextEvent;

    [SerializeField] private GameObject panelAllTexts;
    private List<GameObject> allTexts;

    private int currentTextIndex = 0;
    private GameObject currentText;

    public Action StartNewChoice;

    [Header("Illustration liée à la ligne de texte")]
    [SerializeField] private Image illustrationImage;   // Image entre les barres et la question

    private void Awake()
    {
        UpdateText(panelAllTexts);
    }

    private void OnEnable()
    {
        if (nextTextEvent != null)
            nextTextEvent.RegisterListener(OnNextTextClick);
    }

    private void OnDisable()
    {
        if (nextTextEvent != null)
            nextTextEvent.UnregisterListener(OnNextTextClick);
    }

    void OnNextTextClick(Text text)
    {
        currentTextIndex += 1;
        Debug.Log("Current Index : " + currentTextIndex);
        Debug.Log("allTexts.Count : " + allTexts.Count);

        if (currentTextIndex < allTexts.Count)
        {
            Debug.Log("Il y a un next text");

            // Cache le texte courant
            currentText.SetActive(false);

            // Nouveau texte courant
            currentText = allTexts[currentTextIndex];
            currentText.SetActive(true);

            // 🔹 Met à jour l'image en fonction de ce texte
            UpdateIllustrationForCurrentText();

            if (currentTextIndex == allTexts.Count - 1 && (null != text.GetNextButton()))
            {
                // Securité
                text.GetNextButton().gameObject.SetActive(false);
                StartNewChoice?.Invoke();
            }
        }
    }

    public void UpdateText(GameObject panelAllTexts)
    {
        this.panelAllTexts = panelAllTexts;
        allTexts = new List<GameObject>();
        currentTextIndex = 0;

        foreach (Transform child in panelAllTexts.transform)
        {
            if (child.gameObject.GetComponent<TextMeshProUGUI>() != null)
            {
                child.gameObject.SetActive(false);
                allTexts.Add(child.gameObject);
            }
        }

        if (allTexts.Count == 0)
        {
            Debug.LogWarning("[NextTextManager] Aucun TextMeshProUGUI trouvé dans ce panel.");
            return;
        }

        currentText = allTexts[currentTextIndex];
        currentText.SetActive(true);

        // 🔹 Première image pour le premier texte
        UpdateIllustrationForCurrentText();
    }

    private void UpdateIllustrationForCurrentText()
    {
        if (illustrationImage == null || currentText == null)
            return;

        DialogData data = currentText.GetComponent<DialogData>();

        if (data != null && data.illustration != null)
        {
            illustrationImage.sprite = data.illustration;
            illustrationImage.gameObject.SetActive(true);
        }
        else
        {
            // Si tu préfères garder l'ancienne image, commente la ligne suivante
            illustrationImage.gameObject.SetActive(false);
        }
    }
}
