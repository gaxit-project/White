using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip soundClip;   // 再生したい音のAudioClip
    private AudioSource audioSource;

    void Start()
    {
        // AudioSourceコンポーネントを取得（なければ自動追加）
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 音量やループ設定なども可能
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    // 音を鳴らすメソッド
    public void Play()
    {
        audioSource.PlayOneShot(soundClip);
    }
}
