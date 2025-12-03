using UnityEngine;
using UnityEngine.UI;

public class OptionSceneSetup : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    private void Start()
    {
        // Canvas が有効になってからスライダー登録
        StartCoroutine(RegisterAfterFrame());
    }

    private System.Collections.IEnumerator RegisterAfterFrame()
    {
        yield return null; // 1フレーム待つ
        if (VolumeManager.Instance != null)
        {
            VolumeManager.Instance.RegisterSliders(bgmSlider, seSlider);
        }
    }
}
