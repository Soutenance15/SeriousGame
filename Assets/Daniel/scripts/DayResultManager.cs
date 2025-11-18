using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DayResultManager : MonoBehaviour
{
    [Header("Références gameplay")]
    [SerializeField] private ThePlayer player;
    [SerializeField] private ChoiceManager choiceManager;

    [Header("UI Résumé de journée")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI bodyText;

    [Header("Seuils Glycémie (g/L)")]
    [SerializeField] private float glyLowMax = 0.7f;
    [SerializeField] private float glyHighMin = 1.6f;

    [Header("Seuils Vitalité (0-100)")]
    [SerializeField] private float vitLowMax = 30f;
    [SerializeField] private float vitHighMin = 70f;

    [Header("Seuils Plaisir (0-100)")]
    [SerializeField] private float plaisirLowMax = 20f;
    [SerializeField] private float plaisirHighMin = 60f;

    private void Awake()
    {
        if (resultPanel != null)
            resultPanel.SetActive(false);

        // Sécurité : si tu as oublié de drag & drop, on essaie de trouver tout seul
        if (choiceManager == null)
        {
            choiceManager = UnityEngine.Object.FindFirstObjectByType<ChoiceManager>();
            if (choiceManager == null)
                Debug.LogWarning("[DayResultManager] Aucun ChoiceManager trouvé dans la scène.");
        }

        if (player == null)
        {
            player = UnityEngine.Object.FindFirstObjectByType<ThePlayer>();
            if (player == null)
                Debug.LogWarning("[DayResultManager] Aucun ThePlayer trouvé dans la scène.");
        }
    }

    private void OnEnable()
    {
        if (choiceManager != null)
        {
            Debug.Log("[DayResultManager] Abonnement à OnAllChoicesDone.");
            choiceManager.OnAllChoicesDone += ShowDayResult;
        }
    }

    private void OnDisable()
    {
        if (choiceManager != null)
        {
            choiceManager.OnAllChoicesDone -= ShowDayResult;
        }
    }

    private void ShowDayResult()
    {
        Debug.Log("[DayResultManager] ShowDayResult appelé : affichage du panneau de fin.");

        if (player == null || resultPanel == null)
        {
            Debug.LogWarning("[DayResultManager] Player ou ResultPanel manquant.");
            return;
        }

        float g = player.GetGlycemie();
        float v = player.GetEnergy();
        float p = player.GetPleasure();

        bool glyLow  = g < glyLowMax;
        bool glyHigh = g > glyHighMin;
        bool glyOK   = !glyLow && !glyHigh;

        bool vitLow  = v < vitLowMax;
        bool vitHigh = v > vitHighMin;

        bool plLow   = p < plaisirLowMax;
        bool plHigh  = p > plaisirHighMin;

        string title;
        string body;

        if (glyHigh && vitLow)
        {
            title = "Journée à risque !!";
            body =
                "Ta glycémie est restée trop haute et ton corps finit clairement épuisé.\n\n" +
                "Tu as cumulé des choix qui ont pesé sur ta santé : repas riches, stress, manque de récupération.\n" +
                "Dans la vraie vie, ce genre de journée répétée fatigue énormément l’organisme sur le long terme.";
        }
        else if (glyHigh && plHigh)
        {
            title = "Beaucoup de plaisir… mais à quel prix ?";
            body =
                "Tu as privilégié le plaisir immédiat : snacks sucrés, repas riches, petites tentations.\n\n" +
                "Résultat : ta glycémie finit trop haute, même si toi, sur le moment, tu as passé une 'bonne' journée.\n" +
                "C’est exactement le dilemme de beaucoup de personnes diabétiques.";
        }
        else if (glyOK && vitLow)
        {
            title = "Tu as tenu… mais ton corps est vidé.";
            body =
                "Globalement, ta glycémie n’est pas catastrophique, mais ton niveau de vitalité est très bas.\n\n" +
                "Tu as peut-être pris de 'bons' choix de santé, mais sans assez de pauses ou de récupération.\n" +
                "Sur le long terme, cette fatigue accumulée pèse beaucoup.";
        }
        else if (glyOK && vitHigh && !plLow)
        {
            title = "Journée bien gérée ";
            body =
                "Tu as trouvé un vrai équilibre : glycémie raisonnable, vitalité correcte et un peu de plaisir.\n\n" +
                "Ce type de journée montre qu’on peut vivre avec le diabète sans être dans la frustration totale.";
        }
        else if (glyOK && plLow)
        {
            title = "Santé protégée… mais beaucoup de frustration.";
            body =
                "Tu as protégé ta glycémie aujourd’hui, mais au prix d’un plaisir très bas.\n\n" +
                "Vivre uniquement dans le contrôle est difficile à tenir. L’enjeu, c’est d’équilibrer santé et plaisir.";
        }
        else
        {
            title = "Une journée en équilibre instable.";
            body =
                "Ta journée ne penche ni totalement du côté du risque, ni totalement du côté du contrôle.\n\n" +
                "Certains choix ont aidé ton corps, d’autres l’ont mis en difficulté.\n" +
                "C’est une journée 'mixte', comme beaucoup dans la vraie vie.";
        }

        if (titleText != null)
            titleText.text = title;

        if (bodyText != null)
            bodyText.text = body;

        resultPanel.SetActive(true);
    }

    public void CloseResultPanel()
    {
        if (resultPanel != null)
            resultPanel.SetActive(false);
    }

    // 🔁 Bouton "Rejouer"
    public void ReplayDay()
    {
        Debug.Log("[DayResultManager] Rejouer la journée.");
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    // 🏠 Bouton "Menu"
    public void GoToMenu()
    {
        Debug.Log("[DayResultManager] Retour au menu.");
        SceneManager.LoadScene("MainMenu"); // ← Mets ici le nom EXACT de ta scène menu
    }
}
