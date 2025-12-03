using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class PauseSetting : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        // 初期状態は非表示
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// ポーズ画面を開く
    /// </summary>
    public void OpenPause()
    {
        // Canvas を表示
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // スライダー登録（VolumeManager 側で保存値を反映）
        if (VolumeManager.Instance != null)
        {
            VolumeManager.Instance.RegisterSliders(bgmSlider, seSlider);
        }
        else
        {
            Debug.LogError("VolumeManager インスタンスが見つかりません。");
        }
    }

    /// <summary>
    /// ポーズ画面を閉じる
    /// </summary>
    public void ClosePause()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// ポーズトグル（キーやボタンから呼ぶ）
    /// </summary>
    public void TogglePause()
    {
        if (canvasGroup.alpha > 0f)
            ClosePause();
        else
            OpenPause();
    }
}
