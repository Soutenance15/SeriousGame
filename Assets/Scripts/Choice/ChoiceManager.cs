using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // ✅ Pour TextMeshPro

public class ChoiceManager : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] 
    Slider sliderGlycemie;

    [SerializeField] 
    Slider sliderEnergy;

    [SerializeField] 
    Slider sliderPleasure;

    [Header("Textes des Sliders")]
    [SerializeField]
    TextMeshProUGUI glycemieText;

    [SerializeField]
    TextMeshProUGUI energyText;

    [SerializeField]
    TextMeshProUGUI pleasureText;

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

    // Envoie le feedback au DialogManager
    public event Action<string> OnChoiceSelectedWithFeedback;

    void Awake()
    {
        // Init sliders avec les valeurs du joueur
        sliderGlycemie.value = player.GetGlycemie();
        sliderEnergy.value = player.GetEnergy();
        sliderPleasure.value = player.GetPleasure();

        // Init des textes des sliders
        if (glycemieText != null)
            glycemieText.text = player.GetGlycemie().ToString("0.00") + " g/L";

        if (energyText != null)
            energyText.text = player.GetEnergy().ToString("0");

        if (pleasureText != null)
            pleasureText.text = player.GetPleasure().ToString("0");

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

        // ----------------------------
        // 🌞 MALUS DE FATIGUE SELON LA PHASE DE LA JOURNÉE (VERSION PLUS DOUCE)
        // ----------------------------
        float energyDrift = 0f;
        float pleasureDrift = 0f;

        // 0-2 = Matin (fatigue légère)
        if (currentChoiceIndex <= 2)
        {
            energyDrift = -2f;
            pleasureDrift = -1f;
        }
        // 3-7 = Travail + midi (journée qui pèse)
        else if (currentChoiceIndex <= 7)
        {
            energyDrift = -4f;
            pleasureDrift = -2f;
        }
        // 8-11 = Après-midi + soir (fin de journée plus lourde)
        else
        {
            energyDrift = -6f;
            pleasureDrift = -3f;
        }

        // ----------------------------
        // 🧪 DEBUG — AFFICHAGE DES VALEURS APPLIQUÉES
        // ----------------------------
        Debug.Log(
            $"[CHOICE #{currentChoiceIndex}] " +
            $"\n→ Choix: {choice.name}" +
            $"\n   Glycémie: {choice.GetGlycemie()}" +
            $"\n   Energie: {choice.GetEnergy()} + Drift({energyDrift}) = {choice.GetEnergy() + energyDrift}" +
            $"\n   Plaisir: {choice.GetPleasure()} + Drift({pleasureDrift}) = {choice.GetPleasure() + pleasureDrift}"
        );

        // ----------------------------
        // 💉 APPLICATION DES MODIFS
        // ----------------------------

        // Glycémie : uniquement les choix
        player.AddGlycemie(choice.GetGlycemie());

        // Énergie & Plaisir : choix + fatigue
        player.AddEnergy(choice.GetEnergy() + energyDrift);
        player.AddPleasure(choice.GetPleasure() + pleasureDrift);

        // Valeurs cibles après mise à jour du player
        float targetGly = player.GetGlycemie();
        float targetEnergy = player.GetEnergy();
        float targetPleasure = player.GetPleasure();

        // ----------------------------
        // 🎚 ANIMATION PROGRESSIVE DES SLIDERS
        // ----------------------------
        StartCoroutine(AnimateSlider(sliderGlycemie, sliderGlycemie.value, targetGly, 0.3f, glycemieText, true));
        StartCoroutine(AnimateSlider(sliderEnergy,   sliderEnergy.value,   targetEnergy, 0.3f, energyText, false));
        StartCoroutine(AnimateSlider(sliderPleasure, sliderPleasure.value, targetPleasure, 0.3f, pleasureText, false));

        // Feedback vers le dialog manager
        OnChoiceSelectedWithFeedback?.Invoke(choice.GetMessageFinal());

        // Désactive le groupe de choix courant
        if (currentChoice != null)
            currentChoice.SetActive(false);
    }

    void NextChoice()
    {
        if (allChoices == null || allChoices.Count == 0)
            return;

        if (everStarted)
            currentChoiceIndex += 1;
        else
            everStarted = true;

        if (currentChoiceIndex < allChoices.Count())
        {
            Debug.Log("Il y a un next choice");

            if (currentChoice != null)
                currentChoice.SetActive(false);

            currentChoice = allChoices[currentChoiceIndex];
            currentChoice.SetActive(true);
        }
        else
        {
            Debug.Log("Il n'y a pas de next choice");
        }
    }

    // ------------------------------------------------
    // 🎞️ ANIMATION DOUCE DES SLIDERS + TEXTES
    // ------------------------------------------------
private IEnumerator AnimateSlider(
    Slider slider,
    float startValue,
    float targetValue,
    float duration,
    TextMeshProUGUI valueText,
    bool isGlycemie
)
{
    float time = 0f;

    // ---- BOUNCE : petit boost du scale au début ----
    Transform s = slider.transform;
    Vector3 baseScale = s.localScale;
    Vector3 boostedScale = baseScale * 1.10f;   // petite montée
    Vector3 overShootScale = baseScale * 1.04f; // micro rebond final

    // start → boost
    float bounceInTime = 0.1f;
    float bounceOutTime = 0.12f;

    // Phase 1 : petit pop rapide
    float bt = 0f;
    while (bt < bounceInTime)
    {
        bt += Time.deltaTime;
        float t = bt / bounceInTime;
        t = t * t; // ease-in
        s.localScale = Vector3.Lerp(baseScale, boostedScale, t);
        yield return null;
    }

    // ---- SLIDER VALUE ANIMATION ----
    while (time < duration)
    {
        time += Time.deltaTime;
        float t = time / duration;
        t = t * t * (3f - 2f * t); // smoothstep

        float currentValue = Mathf.Lerp(startValue, targetValue, t);
        slider.value = currentValue;

        // Mise à jour du texte
        if (valueText != null)
        {
            if (isGlycemie)
                valueText.text = currentValue.ToString("0.00") + " g/L";
            else
                valueText.text = Mathf.RoundToInt(currentValue).ToString("0");
        }

        yield return null;
    }

    slider.value = targetValue;

    // ---- PHASE 2 : retour vers overshoot -> base ----
    // Petit rebond amorti
    float ot = 0f;
    while (ot < bounceOutTime)
    {
        ot += Time.deltaTime;
        float t = ot / bounceOutTime;
        t = Mathf.Sin(t * Mathf.PI); // rebond léger

        // overshoot → baseScale
        s.localScale = Vector3.Lerp(overShootScale, baseScale, t);

        yield return null;
    }

    s.localScale = baseScale;

    // Mise à jour définitive du texte
    if (valueText != null)
    {
        if (isGlycemie)
            valueText.text = targetValue.ToString("0.00") + " g/L";
        else
            valueText.text = Mathf.RoundToInt(targetValue).ToString("0");
    }
}

}
