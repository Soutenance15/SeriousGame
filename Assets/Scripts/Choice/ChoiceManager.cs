using System;
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

    void Awake()
    {
        sliderGlycemie.value = player.GetGlycemie();
        sliderEnergy.value = player.GetEnergy();
        sliderPleasure.value = player.GetPleasure();
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

        Debug.Log(choice.GetTextChoice());
        Debug.Log(choice.GetMessageFinal());
    }
}
