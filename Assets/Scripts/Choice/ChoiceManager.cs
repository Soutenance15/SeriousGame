using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField]
    Slider sliderGlycemie;

    [SerializeField]
    Slider sliderEnergy;

    [SerializeField]
    Slider sliderPleasure;

    [SerializeField]
    ThePlayer player;

    [SerializeField]
    ChoiceEvent choiceEvent;

    [SerializeField]
    GameObject panelAllChoices;
    List<GameObject> allChoices;

    int currentChoiceIndex = 0;
    GameObject currentChoice;

    void Awake()
    {
        sliderGlycemie.value = player.GetGlycemie();
        sliderEnergy.value = player.GetEnergy();
        sliderPleasure.value = player.GetPleasure();

        // Choice
        allChoices = new List<GameObject>();

        foreach (Transform child in panelAllChoices.transform)
        {
            allChoices.Add(child.gameObject);
        }

        currentChoice = allChoices[currentChoiceIndex];

    }

    private void OnEnable()
    {
        if (choiceEvent != null)
            choiceEvent.RegisterListener(OnChoiceClick);
    }

    private void OnDisable()
    {
        if (choiceEvent != null)
            choiceEvent.UnregisterListener(OnChoiceClick);
    }

    void OnChoiceClick(Choice choice)
    {
        player.AddGlycemie(choice.GetGlycemie());
        player.AddEnergy(choice.GetEnergy());
        player.AddPleasure(choice.GetPleasure());

        sliderGlycemie.value = player.GetGlycemie();
        sliderEnergy.value = player.GetEnergy();
        sliderPleasure.value = player.GetPleasure();

        NextChoice();

        Debug.Log(choice.GetTextChoice());
        Debug.Log(choice.GetMessageFinal());
    }

    void NextChoice()
    {
        currentChoiceIndex += 1;
        if (currentChoiceIndex < allChoices.Count())
        {
            Debug.Log("Il ya un next choice");

            // Cache le choix courant
            currentChoice.SetActive(false);

            // Change le choix courant
            currentChoice = allChoices[currentChoiceIndex];

            // Affiche le nouveau choix courant
            currentChoice.SetActive(true);
        }
        else
        {
            Debug.Log("Il n'y a pas de next choice");
        }
    }
}
