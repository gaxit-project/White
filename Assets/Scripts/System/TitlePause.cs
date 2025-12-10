using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TitlePause : MonoBehaviour
{
    public static TitlePause Instance { get; private set; }

    [Header("Pause UI")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject firstPauseButton;

    [Header("Title UI")]
    [SerializeField] private GameObject titleUI;

    [Header("Clear UI Button")]

    [Header("Audio Sliders")]
    [SerializeField] private UnityEngine.UI.Slider bgmSlider;
    [SerializeField] private UnityEngine.UI.Slider seSlider;

    private bool isPaused = false;
    private bool slidersInitialized = false; // Slider 初期化済みか判定

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // シーン開始時に一度だけ Slider を初期化
        if (!slidersInitialized)
        {
            pauseMenuUI.SetActive(true); // 一時的にアクティブ
            if (VolumeManager.Instance != null)
            {
                VolumeManager.Instance.RegisterSliders(bgmSlider, seSlider);
            }
            pauseMenuUI.SetActive(false); // 初期化後は非表示
            slidersInitialized = true;
        }
    }

    public void TitleTogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    private void Pause()
    {
        titleUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // プレイヤーの操作停止
        var player = FindObjectOfType<PlayerMove>();
        if (player != null)
            player.SetGameState(false);

        // ポーズメニューの最初のボタンを選択
        StartCoroutine(SelectButtonNextFrame(firstPauseButton));
    }

    private void Resume()
    {
        titleUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // プレイヤーの操作再開
        var player = FindObjectOfType<PlayerMove>();
        if (player != null)
            player.SetGameState(true);

        // 選択状態を解除
        EventSystem.current.SetSelectedGameObject(null);
    }


    private IEnumerator SelectButtonNextFrame(GameObject button)
    {
        yield return null; // 1フレーム待つ
        yield return null; // もう1フレーム待つ

        EventSystem.current.SetSelectedGameObject(null);
        if (button != null)
        {
            EventSystem.current.SetSelectedGameObject(button);
        }
    }
}
