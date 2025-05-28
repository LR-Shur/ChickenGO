// Scripts/Core/GameManager.cs
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        Init,       // 初始状态（加载资源、UI 等）
        Ready,      // 准备开始（可显示倒计时“3,2,1 Go!”）
        Playing,    // 正在游戏中
        Ended       // 回合结束
    }

    public GameState State { get; private set; } = GameState.Init;

    [Header("回合时长（秒）")]
    public float roundDuration = 15 * 60f;

    private float timer = 0f;
    public event Action OnRoundStart;
    public event Action OnRoundEnd;
    public event Action<float> OnTimerChanged; // 剩余时间

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        ChangeState(GameState.Ready);
    }

    private void Update()
    {
        switch (State)
        {
            case GameState.Ready:
                // 可以在这里加入开局前倒计时动画，简化原型直接进入
                ChangeState(GameState.Playing);
                break;

            case GameState.Playing:
                timer -= Time.deltaTime;
                OnTimerChanged?.Invoke(timer);
                if (timer <= 0f)
                {
                    ChangeState(GameState.Ended);
                }
                break;

            case GameState.Ended:
                // 这里做结算逻辑，比如加载结算界面
                break;
        }
    }

    private void ChangeState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.Ready:
                timer = roundDuration;
                break;
            case GameState.Playing:
                OnRoundStart?.Invoke();
                break;
            case GameState.Ended:
                OnRoundEnd?.Invoke();
                break;
        }
    }

    /// <summary>
    /// 重新开始当前场景
    /// </summary>
    public void RestartRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
