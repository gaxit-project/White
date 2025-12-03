using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    private const string BGM_KEY = "BGM_VOLUME";
    private const string SE_KEY = "SE_VOLUME";

    private const float SAFE_MIN_VALUE = 0.0001f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ---------------- BGM ----------------
    public void SetBGMVolume(float value)
    {
        float safeValue = (value <= 0f) ? SAFE_MIN_VALUE : value;
        audioMixer.SetFloat("BGM", Mathf.Log10(safeValue) * 20);

        PlayerPrefs.SetFloat(BGM_KEY, value);
        PlayerPrefs.Save();
    }

    // ---------------- SE ----------------
    public void SetSEVolume(float value)
    {
        float safeValue = (value <= 0f) ? SAFE_MIN_VALUE : value;
        audioMixer.SetFloat("SE", Mathf.Log10(safeValue) * 20);

        PlayerPrefs.SetFloat(SE_KEY, value);
        PlayerPrefs.Save();
    }

    public void LoadVolume()
    {
        float bgmValue = PlayerPrefs.GetFloat(BGM_KEY, 1f);
        float seValue = PlayerPrefs.GetFloat(SE_KEY, 1f);

        if (bgmSlider != null) bgmSlider.value = bgmValue;
        if (seSlider != null) seSlider.value = seValue;

        audioMixer.SetFloat("BGM", Mathf.Log10(Mathf.Max(bgmValue, SAFE_MIN_VALUE)) * 20);
        audioMixer.SetFloat("SE", Mathf.Log10(Mathf.Max(seValue, SAFE_MIN_VALUE)) * 20);
    }

    // ---------------- スライダー登録 ----------------
    public void RegisterSliders(Slider bgm, Slider se)
    {
        bgmSlider = bgm;
        seSlider = se;

        // 保存値を取得
        float savedBgm = PlayerPrefs.GetFloat(BGM_KEY, 1f);
        float savedSe = PlayerPrefs.GetFloat(SE_KEY, 1f);

        // Slider に保存値を強制代入（イベント登録前）
        if (bgmSlider != null) bgmSlider.SetValueWithoutNotify(savedBgm);
        if (seSlider != null) seSlider.SetValueWithoutNotify(savedSe);

        // Mixer に反映
        float safeBgm = (savedBgm <= 0f) ? SAFE_MIN_VALUE : savedBgm;
        float safeSe = (savedSe <= 0f) ? SAFE_MIN_VALUE : savedSe;
        audioMixer.SetFloat("BGM", Mathf.Log10(safeBgm) * 20);
        audioMixer.SetFloat("SE", Mathf.Log10(safeSe) * 20);

        // スライダーの操作イベントを登録
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.RemoveAllListeners();
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }
        if (seSlider != null)
        {
            seSlider.onValueChanged.RemoveAllListeners();
            seSlider.onValueChanged.AddListener(SetSEVolume);
        }
    }

}
