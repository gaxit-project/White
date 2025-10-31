using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    // --- ズーム設定 ---
    [Header("Zoom Settings")]
    [SerializeField] private float zoomInSize = 2f;    // ズームイン時のorthographicSize (より近く)
    [SerializeField] private float zoomOutSize = 5f;   // ズームアウト時のorthographicSize (通常/より遠く)
    [SerializeField] private float zoomSpeed = 5f;     // ズームの速さ

    // --- カメラ移動設定 ---
    [Header("Camera Pan Settings")]
    [SerializeField] private float panSpeed = 5f;      // カメラ移動の速さ

    private Camera mainCamera;
    private float targetZoomSize;
    private Vector3 targetPosition; // 目標とするカメラ位置

    private void Awake()
    {
        // メインカメラコンポーネントを取得
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
        {
            Debug.LogError("Camera component not found on this GameObject.");
            enabled = false; // カメラがなければスクリプトを無効化
            return;
        }

        // 2Dカメラであることを確認し、初期設定を行う
        mainCamera.orthographic = true;
        targetZoomSize = zoomOutSize;
        targetPosition = transform.position; // 初期位置をセット
    }

    private void Update()
    {
        // 1. ズームの更新: 現在のorthographicSizeをターゲットの値に Lerp (線形補間) で近づける
        mainCamera.orthographicSize = Mathf.Lerp(
            mainCamera.orthographicSize,
            targetZoomSize,
            Time.deltaTime * zoomSpeed
        );

        // 2. 位置の更新: 現在の位置を目標位置にスムーズに移動
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * panSpeed
        );
    }

    /// <summary>
    /// カメラをズームイン (アップ) させる
    /// </summary>
    public void ZoomIn()
    {
        targetZoomSize = zoomInSize;
    }

    /// <summary>
    /// カメラをズームアウト (引く) させる
    /// </summary>
    public void ZoomOut()
    {
        targetZoomSize = zoomOutSize;
    }

    /// <summary>
    /// カメラを即座に初期サイズに戻す（シーンロード時などに使用）
    /// </summary>
    public void ZoomOutImmediate()
    {
        if (mainCamera != null)
        {
            mainCamera.orthographicSize = zoomOutSize;
            targetZoomSize = zoomOutSize; // ターゲットも初期値にリセット

            // 位置のターゲットも現在の位置にリセットすることで、移動を停止
            targetPosition = transform.position;
        }
    }

    /// <summary>
    /// カメラの目標位置を設定し、ズームインを開始する (Goal時に使用)
    /// </summary>
    /// <param name="position">カメラがフォーカスするワールド座標</param>
    public void FocusOnPosition(Vector3 position)
    {
        // カメラのZ座標を維持したまま、XとYを目標位置に設定
        // 2Dカメラの場合、Z軸は変更しないのが一般的です
        targetPosition = new Vector3(position.x, position.y + 1, transform.position.z);

        // ズームイン実行
        ZoomIn();
    }
}