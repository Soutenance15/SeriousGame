using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NextTextManager : MonoBehaviour
{
    [SerializeField]
    NextTextEvent nextTextEvent;

    [SerializeField]
    GameObject panelAllTexts;
    List<GameObject> allTexts;

    int currentTextIndex = 0;
    GameObject currentText;

    void Awake()
    {
        // Choice
        allTexts = new List<GameObject>();

        foreach (Transform child in panelAllTexts.transform)
        {
            allTexts.Add(child.gameObject);
        }

        currentText = allTexts[currentTextIndex];
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
        NextText();
    }

    void NextText()
    {
        currentTextIndex += 1;
        if (currentTextIndex < allTexts.Count())
        {
            Debug.Log("Il y a un next text");

            // Cache le text courant
            currentText.SetActive(false);

            // Change le text courant
            currentText = allTexts[currentTextIndex];

            // Affiche le nouveau text courant
            currentText.SetActive(true);
        }
        else
        {
            Debug.Log("Il n'y a pas de next text");
        }
    }
}
