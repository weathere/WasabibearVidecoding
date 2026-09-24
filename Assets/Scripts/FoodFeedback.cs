using System.Collections;
using UnityEngine;

public class FoodFeedback : MonoBehaviour
{
    private Vector3 originalScale;
    private float lastSquishTime = 0f;
    public float squishCooldown = 0.1f; // 避免被多顆醬料連續觸發的冷卻時間
    public float impactThreshold = 2f; // 衝擊力道門檻，用來過濾掉只是在滾動的醬料
    private Coroutine squishCoroutine;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 檢查力道 (如果撞擊力太小就當作是滾動，直接 return)
        if (collision.relativeVelocity.magnitude < impactThreshold) return;

        // 檢查冷卻 (如果剛彈跳過就 return)
        if (Time.time < lastSquishTime + squishCooldown) return;

        // 通過檢查，更新時間
        lastSquishTime = Time.time;

        // 執行彈跳動畫
        if (squishCoroutine != null) StopCoroutine(squishCoroutine);
        squishCoroutine = StartCoroutine(SquishRoutine());
    }

    private IEnumerator SquishRoutine()
    {
        // 先壓扁拉寬
        transform.localScale = new Vector3(originalScale.x * 1.05f, originalScale.y * 0.9f, originalScale.z);

        // 等待時間
        yield return new WaitForSeconds(0.05f);

        // 恢復原狀
        transform.localScale = originalScale;
    }
}
