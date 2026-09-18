using System.Collections;
using UnityEngine;

/// <summary>
/// 食物腳本，當被奶油碰到時會產生擠壓變形（Scale 彈性動畫）並恢復。
/// </summary>
public class Food : MonoBehaviour
{
    [Header("擠壓變形設定")]
    [Tooltip("碰到奶油時的縮放變形比例 (例如 X 變大、Y 變小)")]
    [SerializeField] private Vector3 squashScale = new Vector3(1.2f, 0.8f, 1f);

    [Tooltip("變形與恢復動畫的總時間長度 (秒)")]
    [SerializeField] private float duration = 0.2f;

    private Vector3 originalScale;
    private Coroutine squashCoroutine;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 檢查是否碰到奶油 (透過名稱判斷，或可改用 Tag)
        if (collision.gameObject.name.ToLower().Contains("butter"))
        {
            TriggerSquash();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.ToLower().Contains("butter"))
        {
            TriggerSquash();
        }
    }

    /// <summary>
    /// 觸發擠壓變形效果
    /// </summary>
    public void TriggerSquash()
    {
        if (squashCoroutine != null)
        {
            StopCoroutine(squashCoroutine);
        }
        squashCoroutine = StartCoroutine(SquashRoutine());
    }

    private IEnumerator SquashRoutine()
    {
        float elapsed = 0f;
        Vector3 targetScale = new Vector3(
            originalScale.x * squashScale.x,
            originalScale.y * squashScale.y,
            originalScale.z * squashScale.z
        );

        halfDuration:
        float halfDuration = duration / 2f;

        // 變形過程
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        transform.localScale = targetScale;
        elapsed = 0f;

        // 恢復原狀過程
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }

        transform.localScale = originalScale;
        squashCoroutine = null;
    }
}
