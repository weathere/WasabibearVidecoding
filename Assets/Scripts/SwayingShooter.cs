using UnityEngine;

/// <summary>
/// 保持旋轉固定，並讓物件位置進行順時針環繞位移的發射器
/// </summary>
public class SwayingShooter : MonoBehaviour
{
    [Header("位置順時針環繞設定")]
    [Tooltip("是否啟用順時針位移環繞")]
    [SerializeField] private bool enableOrbit = true;

    [Tooltip("環繞半徑 (位移距離)")]
    [SerializeField] private float orbitRadius = 2f;

    [Tooltip("環繞速率 (度/秒)")]
    [SerializeField] private float orbitSpeed = 90f;

    private Vector3 centerPosition;
    private float currentAngle = 0f;
    private Quaternion initialRotation;

    private void Start()
    {
        centerPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (enableOrbit)
        {
            OrbitClockwise();
        }
        
        // 確保旋轉角度保持不變
        transform.rotation = initialRotation;
    }

    /// <summary>
    /// 控制發射器位置進行順時針環繞位移
    /// </summary>
    private void OrbitClockwise()
    {
        // 順時針方向：角度隨時間遞減
        currentAngle -= orbitSpeed * Time.deltaTime;
        
        // 避免數值過大
        if (currentAngle <= -360f)
        {
            currentAngle += 360f;
        }

        float rad = currentAngle * Mathf.Deg2Rad;

        float x = centerPosition.x + Mathf.Cos(rad) * orbitRadius;
        float y = centerPosition.y + Mathf.Sin(rad) * orbitRadius;

        transform.position = new Vector3(x, y, centerPosition.z);
    }
}
