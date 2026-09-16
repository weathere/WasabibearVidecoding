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

    [Header("發射設定")]
    [Tooltip("要發射的物件預制物 (Prefab)")]
    [SerializeField] private GameObject projectilePrefab;

    [Tooltip("發射點位置")]
    [SerializeField] private Transform firePoint;

    [Tooltip("發射間隔時間 (秒)")]
    [SerializeField] private float fireInterval = 1f;

    private float fireTimer = 0f;
    private float timeElapsed = 0f;

    private void Update()
    {
        Sway();
        HandleFiring();
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

    /// <summary>
    /// 處理定時發射邏輯
    /// </summary>
    private void HandleFiring()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            return;
        }

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            fireTimer = 0f;
            Shoot();
        }
    }

    /// <summary>
    /// 執行發射
    /// </summary>
    private void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}
