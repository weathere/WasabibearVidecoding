using UnityEngine;

public class FoodSwipeController : MonoBehaviour
{
    public Transform foodContainer;
    public float snapSpeed = 10f;

    private bool isDragging = false;
    private Vector3 dragStartMousePos;
    private Vector3 dragStartContainerPos;
    private Camera mainCamera;
    private Transform targetSnapObject;
    private bool isSnapping = false;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleInput();

        if (isSnapping && targetSnapObject != null)
        {
            // 計算目標物件距離畫面中心 (X = 0) 的世界座標 X 差值
            float currentXOffset = targetSnapObject.position.x;
            
            // 將 foodContainer 在 X 軸上平滑移動，使目標物件對齊 X = 0
            float newX = Mathf.Lerp(foodContainer.position.x, foodContainer.position.x - currentXOffset, Time.deltaTime * snapSpeed);
            foodContainer.position = new Vector3(newX, foodContainer.position.y, foodContainer.position.z);

            // 當非常接近 0 時直接對齊並結束吸附
            if (Mathf.Abs(currentXOffset) < 0.001f)
            {
                Vector3 finalPos = foodContainer.position;
                finalPos.x -= currentXOffset;
                foodContainer.position = finalPos;
                isSnapping = false;
            }
        }
    }

    private void HandleInput()
    {
        if (foodContainer == null) return;

        // 偵測滑鼠按下或手機觸控開始
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            isSnapping = false;
            dragStartMousePos = GetInputWorldPosition();
            dragStartContainerPos = foodContainer.position;
        }

        // 偵測拖曳中
        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 currentMousePos = GetInputWorldPosition();
            float differenceX = currentMousePos.x - dragStartMousePos.x;

            Vector3 targetPos = dragStartContainerPos;
            targetPos.x += differenceX;

            // 僅在 X 軸上跟隨移動，Y 軸保持不變
            foodContainer.position = new Vector3(targetPos.x, dragStartContainerPos.y, dragStartContainerPos.z);
        }

        // 偵測放開手指或滑鼠
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            FindClosestObjectAndSnap();
        }
    }

    private Vector3 GetInputWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    private void FindClosestObjectAndSnap()
    {
        if (foodContainer.childCount == 0) return;

        Transform closest = null;
        float minDistance = float.MaxValue;

        // 尋找 X 軸最接近 0 (畫面中心) 的子物件
        foreach (Transform child in foodContainer)
        {
            float distance = Mathf.Abs(child.position.x);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = child;
            }
        }

        targetSnapObject = closest;
        isSnapping = true;
    }
}
