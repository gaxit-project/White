using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageClear : MonoBehaviour
{
    public SceneLoader sceneLoader;
    // 移動させるImageコンポーネント
    public Image targetImage;

    // 移動時間 (秒)
    public float duration = 0.5f;

    // x座標の開始位置: -1920f
    private readonly float startX = -1920f;

    // x座標の終了位置: 0f
    private readonly float endX = 0f;

    // 加速/減速を制御するためのカーブ
    public AnimationCurve movementCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    private bool pushed = false;

    // Start()はそのまま削除した状態を維持

    // ⭐ 修正: ボタンが押されたときに行う処理 ⭐
    public void OnButtonPressed()
    {
        if (!pushed)
        {
            pushed = true;
            // 1. 既に実行中のコルーチンを停止
            StopAllCoroutines();

            // 2. RectTransformを取得
            RectTransform rt = targetImage.GetComponent<RectTransform>();
            if (rt == null)
            {
                Debug.LogError("Target Image is missing RectTransform!");
                return;
            }

            // 3. ⭐ ImageのX座標を-1920fにリセットします ⭐
            Vector3 currentPos = rt.anchoredPosition;
            rt.anchoredPosition = new Vector3(startX, currentPos.y, currentPos.z);

            // 4. 移動コルーチンを開始
            StartCoroutine(MoveImageCoroutine());
        }
    }

    // Imageを移動させるコルーチン
    private IEnumerator MoveImageCoroutine()
    {
        // RectTransformのチェックはOnButtonPressedで済んでいるため簡略化可能
        RectTransform rt = targetImage.GetComponent<RectTransform>();
        if (rt == null) yield break;

        // 初期設定
        float elapsedTime = 0f;

        // OnButtonPressed()で設定された現在の位置 (x=-1920) がスタート地点
        Vector3 startPosition = rt.anchoredPosition;
        Vector3 endPosition = new Vector3(endX, startPosition.y, startPosition.z);

        // 移動ループ (中略... 加速しながらの移動処理)
        while (elapsedTime < duration)
        {
            float timeRatio = elapsedTime / duration;
            float curveValue = movementCurve.Evaluate(timeRatio);
            rt.anchoredPosition = Vector3.Lerp(startPosition, endPosition, curveValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 終了時に正確な最終位置を設定
        rt.anchoredPosition = endPosition;

        // 移動完了後にシーンロードを実行
        if (sceneLoader != null)
        {
            sceneLoader.LoadNextScene();
        }
    }
}