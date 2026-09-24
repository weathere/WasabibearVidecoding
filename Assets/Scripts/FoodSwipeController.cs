using System.Collections;
using UnityEngine;
using TMPro;

public class FoodSwipeController : MonoBehaviour
{
    public Transform foodContainer;
    public float snapSpeed = 10f;
    public GameObject mainGameUIGroup;
    public float fadeDuration = 0.5f;

    [Header("Countdown Settings")]
    public GameObject countdownUIGroup;
    public TMP_Text countdownText;
    public GameObject gameplayUI;

    private bool isDragging = false;
    private Vector3 dragStartMousePos;
    private Vector3 dragStartContainerPos;
    private Camera mainCamera;
    public Transform targetSnapObject;
    private bool isSnapping = false;
    private bool isLocked = false;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //if (isLocked) return;

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
        if (isLocked) return;
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

    public void ConfirmSelection()
    {
        if (targetSnapObject != null)
        {
            Debug.Log("玩家選擇了: " + targetSnapObject.name);
        }
    }

    public void ConfirmSelection(Transform clickedFood)
    {
        // 更新當前選中目標為點擊的食物
        targetSnapObject = clickedFood;
        isSnapping = true; // 讓托盤自動滑動對齊到被點擊的食物
        Debug.Log("玩家點擊並選擇了: " + targetSnapObject.name);

        // 漸隱清場邏輯：對其他未被選中的食物執行漸隱協程
        if (foodContainer != null)
        {
            foreach (Transform child in foodContainer)
            {
                if (child != clickedFood)
                {
                    StartCoroutine(FadeOutFood(child.gameObject));
                }
            }
        }

        // 顯示主遊戲介面
        if (mainGameUIGroup != null)
        {
            mainGameUIGroup.SetActive(true);
        }

        // 鎖定操作：改用 isLocked 變數鎖定輸入
        isLocked = true;

        // 開始倒數計時與進入遊戲協程
        StartCoroutine(GameStartSequence());
    }

    private IEnumerator FadeOutFood(GameObject foodObj)
    {
        SpriteRenderer spriteRenderer = foodObj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color startColor = spriteRenderer.color;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(startColor.a, 0f, elapsedTime / fadeDuration);
                spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }

            // 確保完全透明
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        }

        // 漸隱完成後將物件隱藏
        foodObj.SetActive(false);
    }

    private IEnumerator AnimateCountdownText(string textContent)
    {
        if (countdownText != null)
        {
            countdownText.text = textContent;
            
            Color col = countdownText.color;
            countdownText.color = new Color(col.r, col.g, col.b, 1f);

            Transform textTransform = countdownText.transform;
            textTransform.localScale = new Vector3(5f, 5f, 5f);

            // 階段 1：砸下縮放 (0.2 秒內從 5 縮小到 1)
            float scaleDuration = 0.2f;
            float elapsed = 0f;
            while (elapsed < scaleDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / scaleDuration;
                float currentScale = Mathf.Lerp(5f, 1f, t);
                textTransform.localScale = new Vector3(currentScale, currentScale, currentScale);
                yield return null;
            }
            textTransform.localScale = Vector3.one;

            // 階段 2：停留 (0.5 秒)
            yield return new WaitForSeconds(0.5f);

            // 階段 3：漸隱消失 (0.3 秒內 alpha 從 1 變 0)
            float fadeDurationText = 0.3f;
            elapsed = 0f;
            Color originalColor = countdownText.color;
            while (elapsed < fadeDurationText)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDurationText;
                float alpha = Mathf.Lerp(1f, 0f, t);
                countdownText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
            countdownText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        }
    }

    private IEnumerator GameStartSequence()
    {
        // 等待托盤滑動置中
        yield return new WaitUntil(() => !isSnapping);

        // 打開倒數畫面
        if (countdownUIGroup != null)
            countdownUIGroup.SetActive(true);

        // 依序執行倒數文字特效
        yield return StartCoroutine(AnimateCountdownText("3"));
        yield return StartCoroutine(AnimateCountdownText("2"));
        yield return StartCoroutine(AnimateCountdownText("1"));
        yield return StartCoroutine(AnimateCountdownText("START!"));

        // 倒數結束，切換介面
        if (countdownUIGroup != null)
            countdownUIGroup.SetActive(false);

        if (gameplayUI != null)
            gameplayUI.SetActive(true);
    }
}
