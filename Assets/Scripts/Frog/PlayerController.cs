using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// λ��ö��
    /// </summary>
    private enum enDirection
    {
        enUP, enLeft, enRight
    }

    /// <summary>
    /// frog����ĸ������
    /// </summary>
    private Rigidbody2D rb;
    /// <summary>
    /// frog����Ķ������
    /// </summary>
    private Animator anim;
    /// <summary>
    /// 
    /// </summary>
    private SpriteRenderer spRender;

    /// <summary>
    /// ��λ��Ծ���ȣ�Ĭ��2.1
    /// </summary>
    public float jumpDistance = 2.1f;
    /// <summary>
    /// �ƶ�����
    /// </summary>
    private float m_MoveDistance = 0.0f;
    /// <summary>
    /// Frog ��Ӧ������λ��
    /// </summary>
    private Vector2 m_Destination;
    /// <summary>
    /// �������λ��
    /// </summary>
    private Vector2 m_TouchPosition;

    /// <summary>
    /// �Ƿ�Ϊ��������
    /// </summary>
    private bool m_bHold = false;
    /// <summary>
    /// �Ƿ���Ծ
    /// </summary>
    private bool m_bJump = false;
    /// <summary>
    /// �Ƿ������Ծ
    /// </summary>
    private bool m_bCanJump = false;
    /// <summary>
    /// ��������λ��
    /// </summary>
    private enDirection m_enDir = enDirection.enUP;

    /// <summary>
    /// Ӧ�ó�������ʱ����ʼ����Ӧ����ı�����״̬
    /// </summary>
    private void Awake()
    {
        //��ȡFrog��Ӧ��Rigidbody2D���
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spRender = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// �̶���ʱ�������������������㣺Frog��ǰ�������
    /// </summary>
    private void FixedUpdate()
    {
        if (m_bJump)
        {
            rb.position = Vector2.Lerp(transform.position, m_Destination, 0.134f);
        }
    }

    /// <summary>
    /// ����״̬
    /// </summary>
    private void Update()
    {
        if (m_bCanJump == true)
        {
            //������Ծ����
            JumpTrigger();
        }
    }

    /// <summary>
    /// �������ֺ���
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        //���Ҳ����ǽ��Ӧ��Tag == Border������ײ������ǽʱ����Ϸ����
        if (collision.CompareTag("Border") == true)
        {
            Debug.Log("Border: Game Over!");
        }

        if (m_bJump == false && collision.CompareTag("Obstacle") == true)
        {
            Debug.Log("Border: Game Over!");
        }
    }

    #region Input Event
    /// <summary>
    /// �㰴��Ծ
    /// </summary>
    /// <param name="context"></param>
    public void Jump(InputAction.CallbackContext context)
    {
        //TODO: ִ����Ծ����Ծ�ľ��룬��¼������������Ծ����Ч
        if (context.performed && !m_bJump)
        {
            m_MoveDistance = jumpDistance;
            m_bCanJump = true;
        }
    }

    /// <summary>
    /// ������Ծ
    /// </summary>
    /// <param name="context"></param>
    public void LongJump(InputAction.CallbackContext context)
    {
        if (context.performed && !m_bJump)
        {
            m_MoveDistance = jumpDistance * 2;
            m_bHold = true;
        }

        //���������Ҵ��ڵ�ǰΪ����״̬
        if (context.canceled && m_bHold && !m_bJump)
        {
            m_bCanJump = true;
            m_bHold = false;
        }
    }

    /// <summary>
    /// ��ȡ����ĵ�����
    /// </summary>
    /// <param name="context"></param>
    public void GetTouchPosition(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            m_TouchPosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
            var offset = ((Vector3)m_TouchPosition - transform.position).normalized;
            if (Mathf.Abs(offset.x) <= 0.7f)
            {
                m_enDir = enDirection.enUP;
            }
            else if (offset.x < 0)
            {
                m_enDir = enDirection.enLeft;
            }
            else if (offset.x > 0)
            {
                m_enDir = enDirection.enRight;
            }
        }
    }
    #endregion

    /// <summary>
    /// ������Ծ����
    /// </summary>
    public void JumpTrigger()
    {
        m_bCanJump = false;
        Debug.Log(m_enDir);
        switch (m_enDir)
        {
            case enDirection.enUP:
                //�޸Ķ����л�����
                anim.SetBool("IsSide", false);
                //��ȡ��Ծ�������
                m_Destination = new Vector2(transform.position.x, transform.position.y + m_MoveDistance);
                break;
            case enDirection.enLeft:
                //�޸Ķ����л�����
                anim.SetBool("IsSide", true);
                //��ȡ��Ծ�������
                m_Destination = new Vector2(transform.position.x - m_MoveDistance, transform.position.y);
                //�޸����ž��� Vector3.one = (1, 1, 1)
                transform.localScale = Vector3.one;
                break;
            case enDirection.enRight:
                //�޸Ķ����л�����
                anim.SetBool("IsSide", true);
                //��ȡ��Ծ�������
                m_Destination = new Vector2(transform.position.x + m_MoveDistance, transform.position.y);
                //�޸����ž���
                transform.localScale = new Vector3(-1, 1, 1);
                break;
        }

        //����Jump����
        anim.SetTrigger("Jump");
    }

    #region Animation Event
    /// <summary>
    /// ��Ծ������ʼʱ��
    /// </summary>
    public void StartJumpAnimationEvent()
    {
        //������ʼ���ı��Ѿ���Ծ��״̬
        m_bJump = true;

        //�޸�����ͼ��
        spRender.sortingLayerName = "Top Layer";
    }

    /// <summary>
    /// ��Ծ�������ʱ��
    /// </summary>
    public void FinishJumpAnimationEvent()
    {
        //���������������Ѿ���Ծ��״̬
        m_bJump = false;

        //�޸�����ͼ��
        spRender.sortingLayerName = "Middle Layer";
    }
    #endregion
}
