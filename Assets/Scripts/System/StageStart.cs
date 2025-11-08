using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Coroutineを使うために必要

public class StageStart : MonoBehaviour
{
    // 移動させるImageコンポーネント
    public Image targetImage;

    // 移動時間 (秒)
    public float duration = 0.5f;

    private readonly float startX = 0f;

    private readonly float endX = 1920f;

    public AnimationCurve movementCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    void Start()
    {
        if (targetImage != null)
        {
            // RectTransformを取得
            RectTransform rt = targetImage.GetComponent<RectTransform>();
            if (rt != null)
            {
                // X座標を修正後の開始位置(1920)に設定
                Vector3 currentPos = rt.anchoredPosition;
                rt.anchoredPosition = new Vector3(startX, currentPos.y, currentPos.z);
            }
        }

        // ⭐ 修正: シーン読み込み後すぐに移動コルーチンを開始 ⭐
        StartCoroutine(MoveImageCoroutine());

        // 以前の OnButtonPressed() の内容は削除されます。
    }

    // Imageを移動させるコルーチン
    private IEnumerator MoveImageCoroutine()
    {
        // RectTransformを取得
        RectTransform rt = targetImage.GetComponent<RectTransform>();
        if (rt == null)
        {
            Debug.LogError("Target Image is missing RectTransform!");
            yield break;
        }

        // 初期設定
        float elapsedTime = 0f;
        // Start()で設定された現在の位置 (x=1920) がスタート地点
        Vector3 startPosition = rt.anchoredPosition;
        // 終了位置 (x=0) を設定
        Vector3 endPosition = new Vector3(endX, startPosition.y, startPosition.z);

        // 移動ループ
        while (elapsedTime < duration)
        {
            float timeRatio = elapsedTime / duration;

            // AnimationCurveを使って加速/減速を適用
            float curveValue = movementCurve.Evaluate(timeRatio);

            // Lerpで新しい位置を計算
            rt.anchoredPosition = Vector3.Lerp(startPosition, endPosition, curveValue);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // 終了時に正確な最終位置を設定
        rt.anchoredPosition = endPosition;
    }
}