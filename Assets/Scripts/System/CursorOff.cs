using UnityEngine;

public class CursorController : MonoBehaviour
{
    void Start()
    {
        // 1. �J�[�\�����\���ɂ���
        Cursor.visible = false;

        // 2. �J�[�\�����Q�[���E�B���h�E�̒����Ƀ��b�N����
        //    ����ɂ��A�}�E�X��������Ă��J�[�\������ʊO�ɏo�Ȃ��Ȃ�܂�
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Mouse cursor is hidden and locked.");
    }
}