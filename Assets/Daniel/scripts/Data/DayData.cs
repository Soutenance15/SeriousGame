using UnityEngine;

[CreateAssetMenu(menuName = "SeriousGame/JourneeData")]
public class DayData : ScriptableObject
{
    [Header("Nom de la journée (ex: Journée type d'un salarié diabétique)")]
    public string dayName = "Journee diabete type 2";

    [Header("Valeurs de départ")]
    [Tooltip("Glycémie de départ en g/L (ex: 1.20)")]
    public float startGlycemie = 1.20f;

    [Tooltip("Énergie de départ (0 - 100)")]
    public int startEnergy = 60;

    [Tooltip("Plaisir de départ (0 - 100)")]
    public int startPleasure = 50;

    [Header("Liste des étapes (questions) dans l'ordre")]
    public QuestionStep[] steps;
}
