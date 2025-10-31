using UnityEngine;

public class CursorController : MonoBehaviour
{
    void Start()
    {
        // 1. カーソルを非表示にする
        Cursor.visible = false;

        // 2. カーソルをゲームウィンドウの中央にロックする
        //    これにより、マウス操作をしてもカーソルが画面外に出なくなります
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Mouse cursor is hidden and locked.");
    }
}