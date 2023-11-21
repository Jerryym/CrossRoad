using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 位置枚举
    /// </summary>
    private enum enDirection
    {
        enUP, enLeft, enRight
    }

    /// <summary>
    /// frog自身的刚体组件
    /// </summary>
    private Rigidbody2D rb;
    /// <summary>
    /// frog自身的动画组件
    /// </summary>
    private Animator anim;
    /// <summary>
    /// 
    /// </summary>
    private SpriteRenderer spRender;

    /// <summary>
    /// 单位跳跃长度，默认2.1
    /// </summary>
    public float jumpDistance = 2.1f;
    /// <summary>
    /// 移动长度
    /// </summary>
    private float m_MoveDistance = 0.0f;
    /// <summary>
    /// Frog 对应的坐标位置
    /// </summary>
    private Vector2 m_Destination;
    /// <summary>
    /// 点击坐标位置
    /// </summary>
    private Vector2 m_TouchPosition;

    /// <summary>
    /// 是否为长按操作
    /// </summary>
    private bool m_bHold = false;
    /// <summary>
    /// 是否跳跃
    /// </summary>
    private bool m_bJump = false;
    /// <summary>
    /// 是否可以跳跃
    /// </summary>
    private bool m_bCanJump = false;
    /// <summary>
    /// 青蛙所处位置
    /// </summary>
    private enDirection m_enDir = enDirection.enUP;

    /// <summary>
    /// 应用程序运行时，初始化对应对象的变量和状态
    /// </summary>
    private void Awake()
    {
        //获取Frog对应的Rigidbody2D组件
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spRender = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 固定的时间间隔，进行相关物理计算：Frog当前坐标计算
    /// </summary>
    private void FixedUpdate()
    {
        if (m_bJump)
        {
            rb.position = Vector2.Lerp(transform.position, m_Destination, 0.134f);
        }
    }

    /// <summary>
    /// 更新状态
    /// </summary>
    private void Update()
    {
        if (m_bCanJump == true)
        {
            //触发跳跃动画
            JumpTrigger();
        }
    }

    /// <summary>
    /// 触发保持函数
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        //左右侧空气墙对应的Tag == Border，当碰撞到空气墙时，游戏结束
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
    /// 点按跳跃
    /// </summary>
    /// <param name="context"></param>
    public void Jump(InputAction.CallbackContext context)
    {
        //TODO: 执行跳跃，跳跃的距离，记录分数，播放跳跃的音效
        if (context.performed && !m_bJump)
        {
            m_MoveDistance = jumpDistance;
            m_bCanJump = true;
        }
    }

    /// <summary>
    /// 长按跳跃
    /// </summary>
    /// <param name="context"></param>
    public void LongJump(InputAction.CallbackContext context)
    {
        if (context.performed && !m_bJump)
        {
            m_MoveDistance = jumpDistance * 2;
            m_bHold = true;
        }

        //长按结束且处于当前为长按状态
        if (context.canceled && m_bHold && !m_bJump)
        {
            m_bCanJump = true;
            m_bHold = false;
        }
    }

    /// <summary>
    /// 获取点击的点坐标
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
    /// 触发跳跃动画
    /// </summary>
    public void JumpTrigger()
    {
        m_bCanJump = false;
        Debug.Log(m_enDir);
        switch (m_enDir)
        {
            case enDirection.enUP:
                //修改动画切换参数
                anim.SetBool("IsSide", false);
                //获取跳跃后的坐标
                m_Destination = new Vector2(transform.position.x, transform.position.y + m_MoveDistance);
                break;
            case enDirection.enLeft:
                //修改动画切换参数
                anim.SetBool("IsSide", true);
                //获取跳跃后的坐标
                m_Destination = new Vector2(transform.position.x - m_MoveDistance, transform.position.y);
                //修改缩放矩阵 Vector3.one = (1, 1, 1)
                transform.localScale = Vector3.one;
                break;
            case enDirection.enRight:
                //修改动画切换参数
                anim.SetBool("IsSide", true);
                //获取跳跃后的坐标
                m_Destination = new Vector2(transform.position.x + m_MoveDistance, transform.position.y);
                //修改缩放矩阵
                transform.localScale = new Vector3(-1, 1, 1);
                break;
        }

        //触发Jump动画
        anim.SetTrigger("Jump");
    }

    #region Animation Event
    /// <summary>
    /// 跳跃动画开始时间
    /// </summary>
    public void StartJumpAnimationEvent()
    {
        //动画开始，改变已经跳跃的状态
        m_bJump = true;

        //修改排序图层
        spRender.sortingLayerName = "Top Layer";
    }

    /// <summary>
    /// 跳跃动画完成时间
    /// </summary>
    public void FinishJumpAnimationEvent()
    {
        //动画结束，重置已经跳跃的状态
        m_bJump = false;

        //修改排序图层
        spRender.sortingLayerName = "Middle Layer";
    }
    #endregion
}
