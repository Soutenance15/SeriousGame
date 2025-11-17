using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextTextManager : MonoBehaviour
{
    [SerializeField]
    NextTextEvent nextTextEvent;

    [SerializeField]
    GameObject panelAllTexts;
    List<GameObject> allTexts;

    int currentTextIndex = 0;
    GameObject currentText;

    public Action StartNewChoice;

    void Awake()
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
        Debug.Log("Current Index : + " + currentTextIndex);
        Debug.Log("allTexts.Count : + " + allTexts.Count);
        if (currentTextIndex < allTexts.Count())
        {
            Debug.Log("Il y a un next text");

            // Cache le text courant
            currentText.SetActive(false);

            // Change le text courant
            currentText = allTexts[currentTextIndex];

            // Affiche le nouveau text courant
            currentText.SetActive(true);
            if (currentTextIndex == allTexts.Count() - 1 && (null != text.GetNextButton()))
            {
                // Securité
                // Le dernier dialog ne doit pas avoir de bouton next normalement.
                text.GetNextButton().gameObject.SetActive(false);
                StartNewChoice?.Invoke();
            }
        }
        // else
        // {
        //     StartNewChoice?.Invoke();
        //     Debug.Log("Il n'y a pas de next text");
        // }
    }

    public void UpdateText(GameObject panelAllTexts)
    {
        this.panelAllTexts = panelAllTexts;
        allTexts = new List<GameObject>();
        currentTextIndex = 0;
        foreach (Transform child in panelAllTexts.transform)
        {
            if (null != child.gameObject.GetComponent<TextMeshProUGUI>())
            {
                child.gameObject.SetActive(false);
                allTexts.Add(child.gameObject);
            }
        }

        currentText = allTexts[currentTextIndex];
        currentText.SetActive(true);
    }
}
