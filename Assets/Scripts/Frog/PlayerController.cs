using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

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
    /// frog�������Ⱦ���
    /// </summary>
    private SpriteRenderer spRender;
    /// <summary>
    /// frog������������
    /// </summary>
    private PlayerInput playerInput;
    /// <summary>
    /// frog�������ײ���
    /// </summary>
    private BoxCollider2D boxColl;

    [Header("Score")]
    public int stepScore = 10;
    private int m_iTotalScore = 0;

    [Header("Jump")]
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
    private bool m_bIsJump = false;
    /// <summary>
    /// �Ƿ������Ծ
    /// </summary>
    private bool m_bCanJump = false;
    /// <summary>
    /// ��������λ��
    /// </summary>
    private enDirection m_enDir = enDirection.enUP;
    /// <summary>
    /// ��ײ��������
    /// </summary>
    private RaycastHit2D[] results = new RaycastHit2D[3];
    /// <summary>
    /// �Ƿ�����
    /// </summary>
    private bool m_bIsDead = false;

    /// <summary>
    /// Ӧ�ó�������ʱ����ʼ����Ӧ����ı�����״̬
    /// </summary>
    private void Awake()
    {
        //��ȡFrog��Ӧ��Rigidbody2D���
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spRender = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
        boxColl = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// �̶���ʱ�������������������㣺Frog��ǰ�������
    /// </summary>
    private void FixedUpdate()
    {
        if (m_bIsJump)
        {
            rb.position = Vector2.Lerp(transform.position, m_Destination, 0.134f);
        }
    }

    /// <summary>
    /// ����״̬
    /// </summary>
    private void Update()
    {
        if (m_bIsDead == true)
        {
            DisableInput();
            return;
        }

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
        if (collision.CompareTag("Border") == true || collision.CompareTag("Car") == true)
        {
            Debug.Log("Border: Game Over!");
            m_bIsDead = true;
        }

        //�ж������Ƿ��䵽С����
        if (m_bIsJump == false && collision.CompareTag("Water") == true)
        {
            //��ȡ���ܴ������귢��������߲���֮�ཻ��ģ��
            Physics2D.RaycastNonAlloc(transform.position + Vector3.up * 0.1f, Vector2.zero, results);
            bool bInWater = true;
            foreach (var hit2D in results)
            {
                if (hit2D.collider == null) continue;
                if (hit2D.collider.CompareTag("Wood") == true)
                {
                    Debug.Log("On the Wood!");
                    transform.SetParent(hit2D.collider.transform);
                    bInWater = false;
                }
            }

            //������ˮ�У�����Ϸ����
            if (bInWater == true && m_bIsJump == false)
            {
                Debug.Log("Water: Game Over!");
                m_bIsDead = true;
            }
        }

        if (m_bIsJump == false && collision.CompareTag("Obstacle") == true)
        {
            Debug.Log("Obstacle: Game Over!");
            m_bIsDead = true;
        }

        //��������game over��UI
        if (m_bIsDead == true)
        {
            //֪ͨ��Ϸ����
            EventHandler.CallGameOverEvent();
            //ȡ����ײ���
            boxColl.enabled = false;
        }
    }

    #region Input Event
    /// <summary>
    /// �㰴��Ծ
    /// </summary>
    /// <param name="context"></param>
    public void Jump(InputAction.CallbackContext context)
    {
        //TODO: ������Ծ����Ч
        if (context.performed && m_bIsJump == false)
        {
            //��Ծ�ľ���
            m_MoveDistance = jumpDistance;
            //ִ����Ծ
            m_bCanJump = true;

            //����������Ծʱ��¼����
            if (m_enDir == enDirection.enUP)
            {
                m_iTotalScore += stepScore;
            }
        }
    }

    /// <summary>
    /// ������Ծ
    /// </summary>
    /// <param name="context"></param>
    public void LongJump(InputAction.CallbackContext context)
    {
        if (context.performed && m_bIsJump == false)
        {
            m_MoveDistance = jumpDistance * 2;
            m_bHold = true;
        }

        //���������Ҵ��ڵ�ǰΪ����״̬
        if (context.canceled && m_bHold && m_bIsJump == false)
        {
            m_bCanJump = true;
            m_bHold = false;

            //����������Ծʱ��¼����
            if (m_enDir == enDirection.enUP)
            {
                m_iTotalScore += stepScore * 2;
            }
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
        m_bIsJump = true;

        //�޸�����ͼ��
        spRender.sortingLayerName = "Top Layer";

        //�޸�Frog�ĸ������
        transform.parent = null;
    }

    /// <summary>
    /// ��Ծ�������ʱ��
    /// </summary>
    public void FinishJumpAnimationEvent()
    {
        //���������������Ѿ���Ծ��״̬
        m_bIsJump = false;

        //�޸�����ͼ��
        spRender.sortingLayerName = "Middle Layer";

        if (m_enDir == enDirection.enUP && m_bIsDead == false)
        {
            //�����÷֡����μ��
            EventHandler.CallGetScoreEvent(m_iTotalScore);
        }
    }
    #endregion

    /// <summary>
    /// ��m_bIsDead == true, ����Player Input
    /// </summary>
    private void DisableInput()
    {
        playerInput.enabled = false;
    }
}
