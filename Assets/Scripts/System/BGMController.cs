using UnityEngine;

public class BGMController : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // 1. AudioSourceコンポーネントを取得
        audioSource = GetComponent<AudioSource>();

        // 2. AudioSourceが存在し、AudioClipが設定されていれば再生を開始する
        if (audioSource != null && audioSource.clip != null)
        {
            // BGMの再生を開始
            audioSource.Play();
        }
    }

    // 必要に応じて、他のスクリプトからBGMの再生・停止を制御するためのメソッドも追加できます
    public void StopBGM()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}