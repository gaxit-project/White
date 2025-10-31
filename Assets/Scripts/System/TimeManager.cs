using UnityEngine;
using System.Collections;
using System.IO; // 🎯 ファイル操作のために追加 🎯

public class TimeManager : MonoBehaviour
{
    // --- ファイル保存設定 ---
    private string saveFilePath;
    private const string fileName = "ClearTimes.txt";
    private const string subFolderName = "GameData"; // 実行ファイルと同じフォルダ内に作るサブフォルダ名
    // -------------------------

    private float startTime;
    private bool isRunning = false;

    void Awake()
    {
        // 🎯 実行ファイルのあるディレクトリを取得 🎯
        string applicationDirectory = Directory.GetCurrentDirectory();

        // サブフォルダを結合したパスを作成
        string dataFolder = Path.Combine(applicationDirectory, subFolderName);

        // フォルダが存在しない場合は作成
        if (!Directory.Exists(dataFolder))
        {
            try
            {
                Directory.CreateDirectory(dataFolder);
                Debug.Log($"Created data directory: {dataFolder}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to create data directory at {dataFolder}. Check write permissions. Error: {e.Message}");
            }
        }

        // 最終的なファイルパスを構築
        saveFilePath = Path.Combine(dataFolder, fileName);
    }

    void Start()
    {
        // シーン開始時にタイマーをスタート
        StartTimer();
    }

    void Update()
    {
        // タイマーが実行中の場合、現在時間を表示（デバッグ用）
        if (isRunning)
        {
            // Debug.Log("Current Time: " + GetElapsedTime().ToString("F2"));
        }
    }

    /// <summary>
    /// タイマーを開始します。
    /// </summary>
    public void StartTimer()
    {
        startTime = Time.time;
        isRunning = true;
        Debug.Log("Timer Started.");
    }

    /// <summary>
    /// タイマーを停止し、クリアタイムを整数（秒）で返します。
    /// </summary>
    /// <returns>経過時間（秒、整数）</returns>
    public int StopTimer() // 🎯 戻り値を float から int に変更 🎯
    {
        if (isRunning)
        {
            isRunning = false;
            float rawElapsedTime = GetElapsedTime();

            // 小数点以下を切り捨てて整数に変換
            int elapsedSeconds = Mathf.FloorToInt(rawElapsedTime);

            // 🎯 追加: ファイルに時間を書き込む 🎯
            SaveTimeToFile(elapsedSeconds);

            // ログにクリアタイムを表示
            Debug.Log("🎉 Clear Time: " + elapsedSeconds + " seconds");

            return elapsedSeconds;
        }
        return 0; // 実行中でない場合は 0 を返す（float から int への変更に伴い 0f から 0 に変更）
    }

    /// <summary>
    /// 現在の経過時間を取得します。
    /// </summary>
    public float GetElapsedTime()
    {
        return Time.time - startTime;
    }

    /// <summary>
    /// ファイルにクリアタイムを追記します。
    /// </summary>
    private void SaveTimeToFile(int time)
    {
        try
        {
            // 書き込むデータ形式を設定
            string dataToSave = $"Cleared at: {System.DateTime.Now:yyyy-MM-dd HH:mm:ss}, Time: {time} seconds\n";

            // ファイルに追記 (Append)。ファイルが存在しない場合は自動で作成されます。
            File.AppendAllText(saveFilePath, dataToSave);

            Debug.Log($"Time saved successfully to: {saveFilePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to write time to file: {e.Message}. Check write permissions for the directory: {Path.GetDirectoryName(saveFilePath)}");
        }
    }
}