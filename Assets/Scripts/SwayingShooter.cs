using UnityEngine;

/// <summary>
/// 可以在設定的角度範圍內來回擺動的發射器
/// </summary>
public class SwayingShooter : MonoBehaviour
{
    [Header("擺動設定")]
    [Tooltip("最小擺動角度 (度)")]
    [SerializeField] private float minAngle = -45f;

    [Tooltip("最大擺動角度 (度)")]
    [SerializeField] private float maxAngle = 45f;

    [Tooltip("擺動速度")]
    [SerializeField] private float swaySpeed = 2f;

    private float timeElapsed = 0f;

    private void Update()
    {
        Sway();
    }

    /// <summary>
    /// 控制發射器在指定角度範圍內來回擺動
    /// </summary>
    private void Sway()
    {
        timeElapsed += Time.deltaTime * swaySpeed;
        
        // 使用 Mathf.PingPong 讓數值在 0 到 (maxAngle - minAngle) 之間來回變化
        float angleRange = maxAngle - minAngle;
        float currentAngle = minAngle + Mathf.PingPong(timeElapsed * 10f * swaySpeed, angleRange);

        // 套用旋轉到 Z 軸 (適用於 2D)
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
    }
}
