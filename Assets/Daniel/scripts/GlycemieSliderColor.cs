using UnityEngine;
using UnityEngine.UI;

public class GlycemieSliderColor : MonoBehaviour
{
    [SerializeField] 
    Slider glycemieSlider;

    [SerializeField] 
    Image fillImage;

    [Header("Couleurs")]
    [SerializeField] 
    Color hypoColor = new Color(0.3f, 0.3f, 1f);   // bleu/violet

    [SerializeField] 
    Color normalColor = new Color(0.2f, 0.8f, 0.2f); // vert

    [SerializeField] 
    Color hyperColor = new Color(1f, 0.4f, 0.2f);  // orange/rouge

    [Header("Seuils glycémie")]
    [SerializeField] 
    float minGly = 0.7f;

    [SerializeField] 
    float maxGly = 2.0f;

    [SerializeField] 
    float normalLow = 0.8f;  // début zone normale

    [SerializeField] 
    float normalHigh = 1.4f; // fin zone normale

    void Awake()
    {
        if (glycemieSlider == null)
            glycemieSlider = GetComponent<Slider>();

        // S'assure que les valeurs min/max du slider matchent les bornes
        glycemieSlider.minValue = minGly;
        glycemieSlider.maxValue = maxGly;

        UpdateColor(glycemieSlider.value);
        glycemieSlider.onValueChanged.AddListener(UpdateColor);
    }

    void OnDestroy()
    {
        if (glycemieSlider != null)
            glycemieSlider.onValueChanged.RemoveListener(UpdateColor);
    }

    void UpdateColor(float value)
    {
        if (fillImage == null)
            return;

        Color targetColor;

        if (value <= normalLow)
        {
            // Entre minGly et normalLow → lerp Hypo → Normal
            float t = Mathf.InverseLerp(minGly, normalLow, value);
            targetColor = Color.Lerp(hypoColor, normalColor, t);
        }
        else if (value >= normalHigh)
        {
            // Entre normalHigh et maxGly → lerp Normal → Hyper
            float t = Mathf.InverseLerp(normalHigh, maxGly, value);
            targetColor = Color.Lerp(normalColor, hyperColor, t);
        }
        else
        {
            // Zone normale → couleur normale
            targetColor = normalColor;
        }

        fillImage.color = targetColor;
    }
}
