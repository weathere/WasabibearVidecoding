using UnityEngine;

/// <summary>
/// 奶油發射器，負責在按下空白鍵時生成並發射奶油
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

    private void Update()
    {
        // 偵測是否按下空白鍵
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootButter();
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

        // 在發射點生成奶油實例
        GameObject butterInstance = Instantiate(butterPrefab, spawnPoint.position, spawnPoint.rotation);

        // 取得 Rigidbody2D 組件並施加推力
        Rigidbody2D rb = butterInstance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // 朝發射點的右方 (向前面向) 施加瞬間推力
            rb.AddForce(spawnPoint.right * shootForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("生成物件缺少 Rigidbody2D 組件，無法施加推力！", butterInstance);
        }
    }
}
