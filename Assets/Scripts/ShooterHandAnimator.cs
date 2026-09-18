using UnityEngine;

/// <summary>
/// 處理發射器手部動畫的腳本，當按下空白鍵時進行擠壓形變，鬆開時恢復原狀。
/// </summary>
public class ShooterHandAnimator : MonoBehaviour
{
    [Header("手部動畫設定")]
    [Tooltip("手部的 Transform (用於形變動畫)")]
    [SerializeField] private Transform handTransform;

    [Tooltip("按下發射時手部的擠壓變形比例")]
    [SerializeField] private Vector3 handSquashScale = new Vector3(0.9f, 1.1f, 1f);

    [Tooltip("手部變形與恢復的過渡速度")]
    [SerializeField] private float handAnimSpeed = 10f;

    private Vector3 handOriginalScale;

    private void Start()
    {
        if (handTransform == null)
        {
            handTransform = transform; // 如果未指定，預設使用自身
        }
        handOriginalScale = handTransform.localScale;
    }

    private void Update()
    {
        bool isHolding = Input.GetKey(KeyCode.Space);
        UpdateHandAnimation(isHolding);
    }

    /// <summary>
    /// 更新手部擠壓與恢復的形變動畫
    /// </summary>
    private void UpdateHandAnimation(bool isHolding)
    {
        if (handTransform == null) return;

        Vector3 targetScale = isHolding ? Vector3.Scale(handOriginalScale, handSquashScale) : handOriginalScale;
        handTransform.localScale = Vector3.Lerp(handTransform.localScale, targetScale, Time.deltaTime * handAnimSpeed);
    }
}
