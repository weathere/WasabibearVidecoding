using UnityEngine;

/// <summary>
/// 遊戲管理器，採用單例模式 (Singleton)
/// </summary>
public class GameManager : MonoBehaviour
{
    // 單例實例
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // 若已存在其他實例，則銷毀此重複物件
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // 切換場景時保留此物件
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log("Vibe Coding 環境設定成功！");
    }
}
