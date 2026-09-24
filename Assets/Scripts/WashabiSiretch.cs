using UnityEngine;

public class WasabiStretch : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 如果醬料在空中往下掉 (Y軸速度為負)
        if (rb.velocity.y < -0.1f) 
        {
            // 將 Y 軸拉長，速度越快拉越長
            float stretch = Mathf.Abs(rb.velocity.y) * 0.1f; 
            transform.localScale = new Vector3(originalScale.x, originalScale.y + stretch, originalScale.z);
            
            // 讓圖片旋轉朝向掉落的方向
            transform.up = rb.velocity; 
        }
        else 
        {
            // 碰到食物停下來後，恢復原本胖胖的圓形(坨起來)
            transform.localScale = originalScale;
            transform.rotation = Quaternion.identity;
        }
    }
}
