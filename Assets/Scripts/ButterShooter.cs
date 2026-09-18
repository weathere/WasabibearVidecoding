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

    [Header("隨機偏移設定 (避免擠壓爆炸)")]
    [Tooltip("生成位置的微小隨機範圍")]
    [SerializeField] private float positionRandomness = 0.05f;

    [Tooltip("發射力度的微小隨機比例 (例如 0.05 代表 ±5%)")]
    [SerializeField] private float forceRandomness = 0.05f;

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

        // 加上極微小的隨機位置偏移，避免剛生成的物件完全重疊而互相劇烈碰撞
        Vector3 randomPosOffset = new Vector3(
            Random.Range(-positionRandomness, positionRandomness),
            Random.Range(-positionRandomness, positionRandomness),
            0f
        );
        Vector3 spawnPosition = spawnPoint.position + randomPosOffset;

        // 在發射點生成奶油實例，並使用 spawnPoint 的旋轉
        GameObject butterInstance = Instantiate(butterPrefab, spawnPosition, spawnPoint.rotation);

        // 取得 Rigidbody2D 組件並施加推力
        Rigidbody2D rb = butterInstance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // 加上極微小的力度隨機變化
            float randomizedForce = shootForce * (1f + Random.Range(-forceRandomness, forceRandomness));

            // 嚴格依照發射點當前的本地方向（右方）施加瞬間推力，使其隨旋轉角度改變
            rb.AddForce(spawnPoint.right * randomizedForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("生成物件缺少 Rigidbody2D 組件，無法施加推力！", butterInstance);
        }
    }
}
