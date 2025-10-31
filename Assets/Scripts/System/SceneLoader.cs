using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // 🎯 インスペクターで設定可能にする 🎯
    [Header("Scene Settings")]
    [SerializeField] private string nextSceneName = "Level_02"; // デフォルトの次のシーン名

    /// <summary>
    /// インスペクターで設定された名前のシーンをロードします。
    /// </summary>
    public void LoadNextScene()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("移動先のシーン名がインスペクターで設定されていません。");
            return;
        }

        try
        {
            Debug.Log($"Loading scene: {nextSceneName}");
            // インスペクターで設定されたシーン名を使用してロード
            SceneManager.LoadScene(nextSceneName);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"シーンのロードに失敗しました。シーン名 '{nextSceneName}' がBuild Settingsに存在するか確認してください。エラー: {e.Message}");
        }
    }

    /// <summary>
    /// 現在のシーンをリロード（再読み込み）します。
    /// </summary>
    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        try
        {
            Debug.Log($"Reloading scene: {currentSceneName}");
            SceneManager.LoadScene(currentSceneName);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"現在のシーンのリロードに失敗しました。エラー: {e.Message}");
        }
    }

    // ⚠️ 注意: LoadSceneByName(string) は使用しなくなりますが、
    // 外部からの汎用的な呼び出しのために残しておくことも可能です。
    // 今回は次のステップで StageStates から LoadNextScene() を呼び出します。
}