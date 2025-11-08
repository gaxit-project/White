using UnityEngine;
using UnityEngine.UI; // Imageを使うために必要

public class ImageMover : MonoBehaviour
{
    public SceneLoader sceneLoader;
    // 移動させるImageコンポーネント
    public Image targetImage;

    // 移動時間 (秒)
    public float duration = 0.5f;

    // y座標の開始位置
    private readonly float startY = 1080f;

    // y座標の終了位置
    private readonly float endY = 0f;

    public AnimationCurve movementCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    void Start()
    {
        if (targetImage != null)
        {
            // RectTransformを取得
            RectTransform rt = targetImage.GetComponent<RectTransform>();
            if (rt != null)
            {
                // y座標を開始位置に設定
                Vector3 currentPos = rt.anchoredPosition;
                rt.anchoredPosition = new Vector3(currentPos.x, startY, currentPos.z);
            }
        }
    }

    public void OnButtonPressed()
    {
        StopAllCoroutines();
        StartCoroutine(MoveImageCoroutine());
    }

    // Imageを移動させるコルーチン
    private System.Collections.IEnumerator MoveImageCoroutine()
    {
        // RectTransformを取得
        RectTransform rt = targetImage.GetComponent<RectTransform>();
        if (rt == null)
        {
            Debug.LogError("Target Image is missing RectTransform!");
            yield break; // エラーならコルーチンを終了
        }

        // 初期設定
        float elapsedTime = 0f;
        Vector3 startPosition = rt.anchoredPosition;
        Vector3 endPosition = new Vector3(startPosition.x, endY, startPosition.z);

        // 移動ループ
        while (elapsedTime < duration)
        {
            // 経過時間 / 全時間 = 0から1に変化する割合
            float timeRatio = elapsedTime / duration;

            // AnimationCurveを使って加速/減速を適用
            // 例: timeRatioが0.5の時、movementCurve.Evaluate(0.5)の値がLerpの第3引数になる
            float curveValue = movementCurve.Evaluate(timeRatio);

            // Lerpで新しい位置を計算
            rt.anchoredPosition = Vector3.Lerp(startPosition, endPosition, curveValue);

            // 経過時間を更新
            elapsedTime += Time.deltaTime;

            // 次のフレームまで待機
            yield return null;
        }

        // 終了時に正確な最終位置を設定
        rt.anchoredPosition = endPosition;
        sceneLoader.LoadNextScene();
    }
}