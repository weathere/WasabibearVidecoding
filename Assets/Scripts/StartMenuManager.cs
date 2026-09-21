using UnityEngine;

public class StartMenuManager : MonoBehaviour
{
    [Header("Animators")]
    public Animator animUp;
    public Animator animDown;
    public Animator animLeft;
    public Animator animRight;

    [Header("UI Groups")]
    public GameObject chooseFoodGroup;

    private bool hasStarted = false;

    private void Update()
    {
        // 檢查是否點擊畫面的任何地方（滑鼠左鍵點擊 或 螢幕觸控）
        if (!hasStarted && (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        if (hasStarted) return;
        hasStarted = true;

        if (animUp != null)
            animUp.Play("UpAnimationName");
        
        if (animDown != null)
            animDown.Play("DownAnimationName");
        
        if (animLeft != null)
            animLeft.Play("LeftAnimationName");
        
        if (animRight != null)
            animRight.Play("RightAnimationName");

        if (chooseFoodGroup != null)
            chooseFoodGroup.SetActive(true);
    }
}
