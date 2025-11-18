using UnityEngine;
using UnityEngine.UI;

public class CriticalFlash : MonoBehaviour
{
    [Header("Image de fond à faire pulser")]
    [SerializeField] private Image backgroundImage;

    [Header("Intensité du flash")]
    [SerializeField] private float minAlpha = 0.2f;
    [SerializeField] private float maxAlpha = 0.6f;

    [Header("Vitesse du flash")]
    [SerializeField] private float flashSpeed = 2f;

    private void Reset()
    {
        // Si tu mets ce script directement sur le Panel, il va choper l'Image du même objet
        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (backgroundImage == null)
            return;

        // Ping-pong entre min et max en fonction du temps
        float t = (Mathf.Sin(Time.unscaledTime * flashSpeed) + 1f) * 0.5f;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        Color c = backgroundImage.color;
        c.a = alpha;
        backgroundImage.color = c;
    }
}
