using UnityEngine;

/// <summary>
/// 可以持續進行順時針旋轉，並支援左右來回移動的發射器
/// </summary>
public class SwayingShooter : MonoBehaviour
{
    [Header("旋轉設定")]
    [Tooltip("順時針旋轉速率 (度/秒)")]
    [SerializeField] private float rotationSpeed = 90f;

    [Header("移動設定")]
    [Tooltip("是否啟用左右來回移動")]
    [SerializeField] private bool enableMove = false;

    [Tooltip("左右移動的最大距離 (從初始位置起算的一側距離)")]
    [SerializeField] private float moveDistance = 2f;

    [Tooltip("左右移動的速度")]
    [SerializeField] private float moveSpeed = 2f;

    private float moveTimeElapsed = 0f;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        RotateClockwise();
        if (enableMove)
        {
            MoveHorizontally();
        }
    }

    /// <summary>
    /// 控制發射器持續進行順時針旋轉
    /// </summary>
    private void RotateClockwise()
    {
        // 順時針旋轉使用負的 Z 軸角度 (Unity 中正轉為逆時針)
        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 控制發射器左右來回移動
    /// </summary>
    private void MoveHorizontally()
    {
        moveTimeElapsed += Time.deltaTime * moveSpeed;

        // 使用 PingPong 讓 X 軸座標在 -moveDistance 到 +moveDistance 之間來回擺動
        float xOffset = Mathf.PingPong(moveTimeElapsed * moveSpeed, moveDistance * 2f) - moveDistance;
        
        transform.position = new Vector3(initialPosition.x + xOffset, initialPosition.y, initialPosition.z);
    }
}
