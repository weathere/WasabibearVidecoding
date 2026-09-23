using System.Collections;
using UnityEngine;

public class StartMenuManager : MonoBehaviour
{
    [Header("Animators")]
    public Animator animUp;
    public Animator animDown;
    public Animator animLeft;
    public Animator animRight;
    public Animator animUI;
    public Animator animFoodEnter;
    public Animator GreenBear;
    public Animator Handin;

    [Header("Settings")]
    public float transitionDelay = 1.0f;

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

        // 觸發狀態機切換到離場動畫 (請確認你的 Animator Controller 裡有這個 Trigger 名稱，若不是請替換)
        string triggerName = "TriggerExit";

        if (animUp != null)
            animUp.SetTrigger(triggerName);
        
        if (animDown != null)
            animDown.SetTrigger(triggerName);
        
        if (animLeft != null)
            animLeft.SetTrigger(triggerName);
        
        if (animRight != null)
            animRight.SetTrigger(triggerName);

        if (animUI != null)
            animUI.SetTrigger(triggerName);

        StartCoroutine(TransitionToNextScene());
    }

    private IEnumerator TransitionToNextScene()
    {
        yield return new WaitForSeconds(transitionDelay);

        if (chooseFoodGroup != null)
            chooseFoodGroup.SetActive(true);

        if (animFoodEnter != null)
            animFoodEnter.SetTrigger("TriggerEnter");

        if (GreenBear != null)
            GreenBear.SetTrigger("TriggerEnter");

        if (Handin != null)
            Handin.SetTrigger("TriggerEnter");
    }
}
