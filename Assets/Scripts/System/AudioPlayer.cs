using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip soundClip;
    [SerializeField] private AudioMixerGroup outputGroup;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // OutputをMixerグループに設定
        if (outputGroup != null)
        {
            audioSource.outputAudioMixerGroup = outputGroup;
        }

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = 1f; // Mixer制御のため、必ず1fに設定

        // 【✅ 修正点】新しいAudioSourceに現在の音量設定を強制的に適用する
        // VolumeManagerがシングルトンである前提
        if (VolumeManager.Instance != null)
        {
            VolumeManager.Instance.LoadVolume();
        }
    }

    // 音を鳴らすメソッド
    public void Play()
    {
        if (soundClip != null)
        {
            audioSource.PlayOneShot(soundClip);
        }
    }
}