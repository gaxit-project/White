using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LiftMoveW : MonoBehaviour
{
    [SerializeField] private float moveDistance = 2f; // �ړ�����
    [SerializeField] private float moveSpeed = 2f;    // �ړ����x

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool movingRight = true;

    private void Start()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.right * moveDistance;
    }

    private void Update()
    {
        // ���݂̖ړI�n�Ɍ������Ĉړ�
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // ���B�����������؂�ւ��i2�b���Ƃɉ�������C���[�W�j
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            movingRight = !movingRight;
            targetPos = movingRight ? startPos + Vector3.right * moveDistance : startPos;
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        // Player�Ƃ͏Փ˂𖳎�����
        if (collision.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }*/
}
