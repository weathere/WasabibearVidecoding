using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// 遊戲管理器，採用單例模式 (Singleton)
/// </summary>
public class GameManager : MonoBehaviour
{
    // 單例實例
    public static GameManager Instance { get; private set; }

    [Header("UI Fade Settings")]
    public CanvasGroup titleUIFadeGroup;

    [Header("Countdown Settings")]
    public GameObject countdownUIGroup;
    public TMP_Text countdownText;
    public GameObject gameplayUI;
    public float textPopScale = 3f;

    [Header("Gameplay Start Settings")]
    public Rigidbody2D wasabiCapRb; // 芥末蓋子的剛體
    public MonoBehaviour[] gameScriptsToEnable; // 要啟動的遊戲腳本陣列

    private void Awake()
    {
        // 若已存在其他實例，則銷毀此重複物件
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // 視需求決定是否需要 DontDestroyOnLoad，此處保留
        // DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log("Vibe Coding 環境設定成功！");
    }

    public void StartGameFlow()
    {
        StartCoroutine(GameStartSequence());
    }

    private IEnumerator GameStartSequence()
    {
        // 第一步先做 UI 漸隱
        if (titleUIFadeGroup != null)
        {
            float fadeDuration = 0.5f;
            float elapsed = 0f;
            float startAlpha = titleUIFadeGroup.alpha;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;
                titleUIFadeGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);
                yield return null;
            }
            titleUIFadeGroup.alpha = 0f;
            titleUIFadeGroup.gameObject.SetActive(false);
        }

        // 打開倒數畫面
        if (countdownUIGroup != null)
            countdownUIGroup.SetActive(true);

        // 依序執行倒數文字特效
        yield return StartCoroutine(AnimateCountdownText("3"));
        yield return StartCoroutine(AnimateCountdownText("2"));
        yield return StartCoroutine(AnimateCountdownText("1"));
        yield return StartCoroutine(AnimateCountdownText("START"));

        // 倒數結束，切換介面
        if (countdownUIGroup != null)
            countdownUIGroup.SetActive(false);

        if (gameplayUI != null)
            gameplayUI.SetActive(true);

        // 讓芥末蓋子掉下來
        if (wasabiCapRb != null)
        {
            wasabiCapRb.bodyType = RigidbodyType2D.Dynamic;
        }

        // 啟動所有指定的遊戲腳本
        if (gameScriptsToEnable != null)
        {
            foreach (MonoBehaviour script in gameScriptsToEnable)
            {
                if (script != null)
                {
                    script.enabled = true;
                }
            }
        }

        // TODO: 啟動計時器
    }

    private IEnumerator AnimateCountdownText(string textContent)
    {
        if (countdownText != null)
        {
            countdownText.text = textContent;
            
            Color col = countdownText.color;
            countdownText.color = new Color(col.r, col.g, col.b, 1f);

            Transform textTransform = countdownText.transform;
            Vector3 originalScale = textTransform.localScale;

            // 階段 1：砸下縮放 (0.2 秒內從 originalScale * textPopScale 縮小到 originalScale)
            float scaleDuration = 0.2f;
            float elapsed = 0f;
            while (elapsed < scaleDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / scaleDuration;
                textTransform.localScale = Vector3.Lerp(originalScale * textPopScale, originalScale, t);
                yield return null;
            }
            textTransform.localScale = originalScale;

            // 階段 2：停留 (0.5 秒)
            yield return new WaitForSeconds(0.5f);

            // 階段 3：漸隱消失 (0.3 秒內 alpha 從 1 變 0)
            float fadeDurationText = 0.3f;
            elapsed = 0f;
            Color originalColor = countdownText.color;
            while (elapsed < fadeDurationText)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDurationText;
                float alpha = Mathf.Lerp(1f, 0f, t);
                countdownText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
            countdownText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        }
    }
}
