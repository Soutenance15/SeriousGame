using UnityEngine;

[System.Serializable]
public class ChoiceData
{
    [Header("Texte du bouton (ex : Petit déjeuner sucré)")]
    public string label;

    [Header("Variations")]
    [Tooltip("Variation de la glycémie en g/L (ex : +0.30)")]
    public float deltaGlycemie; 

    [Tooltip("Variation de l'énergie (ex : +10 ou -5)")]
    public int deltaEnergie;

    [Tooltip("Variation du plaisir (ex : +20 ou -5)")]
    public int deltaPlaisir;

    [Header("Feedback affiché après le choix")]
    [TextArea(2,5)]
    public string feedback;
}
