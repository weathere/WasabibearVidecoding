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

    [Tooltip("融化後的最小 Y 軸縮放值 (到達此值即停止融化)")]
    [SerializeField] private float minScaleY = 0.1f;

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
        float newScaleY = Mathf.MoveTowards(currentScale.y, minScaleY, meltSpeed * Time.deltaTime);
        float deltaScaleY = currentScale.y - newScaleY;

        if (deltaScaleY > 0f)
        {
            // 更新縮放
            transform.localScale = new Vector3(currentScale.x, newScaleY, currentScale.z);

            // 由於預設軸心在中心，向下融化需補償高度差的一半
            transform.position -= new Vector3(0f, deltaScaleY * 0.5f, 0f);
        }

        // 檢查是否已達到融化下限
        if (Mathf.Approximately(newScaleY, minScaleY))
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
