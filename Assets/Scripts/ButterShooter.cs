using UnityEngine;

/// <summary>
/// 奶油發射器，負責在按住發射鍵時持續連發奶油
/// </summary>
public class ButterShooter : MonoBehaviour
{
    [Header("發射設定")]
    [Tooltip("要發射的奶油預製物 (需掛載 Rigidbody2D)")]
    [SerializeField] private GameObject butterPrefab;

    [Tooltip("奶油生成的起始位置")]
    [SerializeField] private Transform spawnPoint;

    [Tooltip("發射推力大小")]
    [SerializeField] private float shootForce = 10f;

    [Tooltip("連發間隔時間 (秒)")]
    [SerializeField] private float fireInterval = 0.2f;

    private float fireTimer = 0f;

    private void Update()
    {
        // 偵測是否持續按住空白鍵
        if (Input.GetKey(KeyCode.Space))
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireInterval)
            {
                fireTimer = 0f;
                ShootButter();
            }
        }
        else
        {
            // 當放開按鍵時，重置計時器以便下次按下時能立刻發射第一發
            fireTimer = fireInterval;
        }
    }

    /// <summary>
    /// 生成並發射奶油物件
    /// </summary>
    private void ShootButter()
    {
        if (butterPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("ButterPrefab 或 SpawnPoint 尚未設定！", this);
            return;
        }

        // 在發射點生成奶油實例，並使用 spawnPoint 的旋轉
        GameObject butterInstance = Instantiate(butterPrefab, spawnPoint.position, spawnPoint.rotation);

        // 取得 Rigidbody2D 組件並施加推力
        Rigidbody2D rb = butterInstance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // 嚴格依照發射點當前的本地方向（右方）施加瞬間推力，使其隨旋轉角度改變
            rb.AddForce(spawnPoint.right * shootForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("生成物件缺少 Rigidbody2D 組件，無法施加推力！", butterInstance);
        }
    }
}
