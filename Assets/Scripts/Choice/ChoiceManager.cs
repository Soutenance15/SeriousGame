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

    [Header("Max Values Sliders")]
    [SerializeField]
    float glycemieMax;

    [SerializeField]
    float energyMax;

    [SerializeField]
    float pleasureMax;

    [SerializeField]
    ChoiceEvent choiceEvent;

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
        sliderGlycemie.value += choice.GetGlycemie();
        sliderEnergy.value += choice.GetEnergy();
        sliderPleasure.value += choice.GetPleasure();
        if (sliderGlycemie.value > glycemieMax)
            sliderGlycemie.value = glycemieMax;
        if (sliderEnergy.value > energyMax)
            sliderEnergy.value = energyMax;
        if (sliderPleasure.value > pleasureMax)
            sliderPleasure.value = pleasureMax;

        Debug.Log(choice.GetTextChoice());
        Debug.Log(choice.GetMessageFinal());
    }
}
