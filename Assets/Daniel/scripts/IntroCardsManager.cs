using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroCardsManager : MonoBehaviour
{
    [System.Serializable]
    public class IntroCard
    {
        [TextArea] public string body;
        public string buttonLabel;
        public Sprite illustration;
    }

    [Header("Références UI (TMP)")]
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button skipButton;

    [Header("Illustration")]
    [SerializeField] private Image illustrationImage;

    [Header("Effet de fondu")]
    [SerializeField] private CanvasGroup panelCanvasGroup;
    [SerializeField] private float fadeDuration = 0.35f;

    [Header("Cartes d'intro")]
    [SerializeField] private IntroCard[] cards;

    [Header("Scène à charger après l'intro")]
    [SerializeField] private string nextSceneName = "MorningScene";

    private int currentIndex = 0;
    private Coroutine fadeRoutine;

    private void Start()
    {
        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextClicked);

        if (skipButton != null)
            skipButton.onClick.AddListener(LoadGameScene);

        ShowCard(0);
    }

    private void ShowCard(int index)
    {
        if (cards == null || cards.Length == 0) return;
        if (index < 0 || index >= cards.Length) return;

        IntroCard card = cards[index];

        // Mettre à jour le texte (plus de titleText)
        bodyText.text = card.body;
        buttonText.text = string.IsNullOrEmpty(card.buttonLabel) ? "Suivant" : card.buttonLabel;

        // Mettre à jour l'image
        if (illustrationImage != null)
        {
            illustrationImage.sprite = card.illustration;
            illustrationImage.gameObject.SetActive(card.illustration != null);
        }

        // Fade-in
        if (panelCanvasGroup != null)
        {
            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);

            panelCanvasGroup.alpha = 0f;
            fadeRoutine = StartCoroutine(FadeInRoutine());
        }
    }

    private IEnumerator FadeInRoutine()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            panelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        panelCanvasGroup.alpha = 1f;
    }

    private void OnNextClicked()
    {
        if (currentIndex >= cards.Length - 1)
        {
            LoadGameScene();
            return;
        }

        currentIndex++;
        ShowCard(currentIndex);
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
