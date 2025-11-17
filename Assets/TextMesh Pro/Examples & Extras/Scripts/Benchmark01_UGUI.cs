using UnityEngine;
using System.Collections;
using TMPro;  // ✅ Nécessaire pour TextMeshProUGUI & TextAlignmentOptions

namespace TMPro.Examples
{
    public class Benchmark01_UGUI : MonoBehaviour
    {
        public int BenchmarkType = 0;

        public Canvas canvas;
        public TMP_FontAsset TMProFont;

        private TextMeshProUGUI m_textMeshPro;

        private const string label01 = "The <#0050FF>count is: </color>";
        // private const string label02 = "The <color=#0050FF>count is: </color>"; // plus utilisé

        private Material m_material01;
        private Material m_material02;

        IEnumerator Start()
        {
            // On force le mode TextMeshPro UGUI (plus de Text classique)
            BenchmarkType = 0;

            // Ajoute automatiquement un composant TextMeshProUGUI sur l'objet
            m_textMeshPro = gameObject.AddComponent<TextMeshProUGUI>();

            if (TMProFont != null)
                m_textMeshPro.font = TMProFont;

            m_textMeshPro.fontSize = 48;
            m_textMeshPro.alignment = TextAlignmentOptions.Center;
            m_textMeshPro.extraPadding = true;

            // Matériau de base de la font
            m_material01 = m_textMeshPro.font.material;

            // Optionnel : autre matériau pour l'effet de switch (assure-toi qu'il existe, sinon laisse null)
            m_material02 = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - BEVEL");

            for (int i = 0; i <= 1000000; i++)
            {
                // Met à jour le texte avec un compteur
                m_textMeshPro.text = label01 + (i % 1000);

                // Tous les 999, on alterne le material si le 2e existe
                if (i % 1000 == 999 && m_material02 != null)
                {
                    m_textMeshPro.fontSharedMaterial =
                        (m_textMeshPro.fontSharedMaterial == m_material01)
                        ? m_material02
                        : m_material01;
                }

                yield return null;
            }

            yield return null;
        }
    }
}
