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

    public void StartGame()
    {
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
