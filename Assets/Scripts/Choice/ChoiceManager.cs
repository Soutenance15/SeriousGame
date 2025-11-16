using System;
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

    [Header("Choix UI")]
    [SerializeField] 
    GameObject panelAllChoices;

    List<GameObject> allChoices;
    int currentChoiceIndex = 0;
    GameObject currentChoice;

    [SerializeField] 
    NextTextManager nextTextManager;

    bool everStarted;

    // ✅ Nouvel event : envoie le feedback (texte) au DialogManager
    public event Action<string> OnChoiceSelectedWithFeedback;

    void Awake()
    {
        // Init sliders avec les valeurs du joueur
        sliderGlycemie.value = player.GetGlycemie();
        sliderEnergy.value = player.GetEnergy();
        sliderPleasure.value = player.GetPleasure();

        // Récupère tous les groupes de choix enfants
        allChoices = new List<GameObject>();

        foreach (Transform child in panelAllChoices.transform)
        {
            allChoices.Add(child.gameObject);
        }

        if (allChoices.Count > 0)
        {
            currentChoiceIndex = 0;
            currentChoice = allChoices[currentChoiceIndex];
        }
    }

    private void OnEnable()
    {
        nextTextManager.StartNewChoice += NextChoice;

        if (choiceEvent != null)
            choiceEvent.RegisterListener(OnChoiceClick);
    }

    private void OnDisable()
    {
        nextTextManager.StartNewChoice -= NextChoice;

        if (choiceEvent != null)
            choiceEvent.UnregisterListener(OnChoiceClick);
    }

    void OnChoiceClick(Choice choice)
    {
        if (choice == null)
            return;

        // 🔢 Applique les deltas au joueur
        player.AddGlycemie(choice.GetGlycemie());
        player.AddEnergy(choice.GetEnergy());
        player.AddPleasure(choice.GetPleasure());

        // 💉 Met à jour les sliders
        sliderGlycemie.value = player.GetGlycemie();
        sliderEnergy.value = player.GetEnergy();
        sliderPleasure.value = player.GetPleasure();

        // 📝 Envoie le feedback au DialogManager (GetMessageFinal = ton texte de feedback)
        OnChoiceSelectedWithFeedback?.Invoke(choice.GetMessageFinal());

        // ❌ Empêche de recliquer sur ce groupe de choix
        if (currentChoice != null)
            currentChoice.SetActive(false);

        // ❌ On ne passe PLUS directement au dialog suivant ici
        // StartNewDialog?.Invoke();  // -> supprimé
    }

    void NextChoice()
    {
        if (allChoices == null || allChoices.Count == 0)
            return;

        if (everStarted)
        {
            currentChoiceIndex += 1;
        }
        else
        {
            everStarted = true;
        }

        if (currentChoiceIndex < allChoices.Count())
        {
            Debug.Log("Il y a un next choice");

            // Cache le choix courant
            if (currentChoice != null)
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
