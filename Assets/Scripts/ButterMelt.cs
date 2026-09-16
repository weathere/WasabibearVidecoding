using UnityEngine;

/// <summary>
/// 奶油融化效果腳本
/// 碰到碰撞體後等待指定時間開始融化，並補償 Y 軸位移以緊貼地面
/// </summary>
public class ButterMelt : MonoBehaviour
{
    [Header("融化設定")]
    [Tooltip("碰撞後等待開始融化的時間 (秒)")]
    [SerializeField] private float meltDelay = 1f;

    [Tooltip("融化速度 (縮小 localScale.y 的速度)")]
    [SerializeField] private float meltSpeed = 0.5f;

    [Tooltip("融化後的最小縮放值 (X, Y, Z 各軸到達此值即停止融化)")]
    [SerializeField] private Vector3 minScale = new Vector3(1f, 0.1f, 1f);

    private bool hasCollided = false;
    private bool isMelting = false;
    private bool isMelted = false;
    private float timer = 0f;

    private void Update()
    {
        // 尚未碰撞或已融化完畢則不執行
        if (!hasCollided || isMelted)
        {
            return;
        }

        // 碰撞後等待融化倒數
        if (!isMelting)
        {
            timer += Time.deltaTime;
            if (timer >= meltDelay)
            {
                isMelting = true;

                // 開始融化時將剛體設為靜態或暫停運動，避免物理碰撞干擾位移
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }
            }
            return;
        }

        // 執行融化縮小
        Vector3 currentScale = transform.localScale;
        Vector3 newScale = Vector3.MoveTowards(currentScale, minScale, meltSpeed * Time.deltaTime);
        float deltaScaleY = currentScale.y - newScale.y;

        // 更新縮放
        transform.localScale = newScale;

        // 由於預設軸心在中心，向下融化需補償 Y 軸高度差的一半
        if (deltaScaleY > 0f)
        {
            transform.position -= new Vector3(0f, deltaScaleY * 0.5f, 0f);
        }

        // 檢查各軸是否均已達到融化下限
        if (transform.localScale == minScale)
        {
            isMelted = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TriggerMelt();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TriggerMelt();
    }

    private void TriggerMelt()
    {
        if (!hasCollided)
        {
            hasCollided = true;
        }
    }
}
