using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveLiftH : MonoBehaviour
{
    private enum MoveDir
    {
        Up,
        Down,
    }

    [SerializeField]
    private MoveDir _moveDir;

    //移動の速さ
    [SerializeField]
    private float _moveSpeed = 0.0f;
    //移動幅
    [SerializeField]
    private float _moveRange = 1;

    private Rigidbody2D Rb { get; set; }

    private float t { get; set; } = 0.0f;

    //初期位置
    private Vector2 InitPos { get; set; }
    //移動前の位置
    private Vector2 BeforePos { get; set; }
    //移動後の位置
    private Vector2 AfterPos { get; set; }

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();

        InitPos = transform.position;
        BeforePos = transform.position;

        if (_moveDir == MoveDir.Down)
        {
            _moveRange *= -1;
        }

        AfterPos = transform.position + new Vector3(_moveRange, _moveRange);
    }

    private void FixedUpdate()
    {
        //Rb.MovePosition(new Vector2(InitPos.x, Mathf.Lerp(BeforePos.y, AfterPos.y, t)));
        Rb.MovePosition(new Vector2(InitPos.x, Mathf.SmoothStep(BeforePos.y, AfterPos.y, t)));

        t += _moveSpeed * Time.fixedDeltaTime;

        ChangeMoveInfo();
    }

    private void ChangeMoveInfo()
    {
        if (t > 1.0f)
        {
            var temp = BeforePos;
            BeforePos = AfterPos;
            AfterPos = temp;

            t = 0.0f;
        }
    }
}