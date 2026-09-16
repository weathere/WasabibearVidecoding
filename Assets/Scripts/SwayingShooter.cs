using UnityEngine;

/// <summary>
/// 可以在設定的角度範圍內來回擺動，並支援左右來回移動的發射器
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

    [Header("移動設定")]
    [Tooltip("是否啟用左右來回移動")]
    [SerializeField] private bool enableMove = false;

    [Tooltip("左右移動的最大距離 (從初始位置起算的一側距離)")]
    [SerializeField] private float moveDistance = 2f;

    [Tooltip("左右移動的速度")]
    [SerializeField] private float moveSpeed = 2f;

    private float timeElapsed = 0f;
    private float moveTimeElapsed = 0f;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        Sway();
        if (enableMove)
        {
            MoveHorizontally();
        }
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
