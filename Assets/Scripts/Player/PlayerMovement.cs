using UnityEngine;
using Prime31;
using System;

public class PlayerMovement : MonoBehaviour
{
    // movement config
    /// <summary>
    /// 重力
    /// </summary>
    public float gravity = -25f;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float runSpeed = 8f;
    /// <summary>
    /// 地面阻尼
    /// </summary>
    public float groundDamping = 20f;
    /// <summary>
    /// 空气阻尼
    /// </summary>
    public float inAirDamping = 5f;
    /// <summary>
    /// 跳跃高度
    /// </summary>
    public float jumpHeight = 3f;

    private float normalizedHorizontalSpeed = 0;
    private CharacterController2D controller;
    private Animator animator;
    private RaycastHit2D lastControllerColliderHit;
    private Vector3 velocity;

    void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
        //注册事件
        controller.onControllerCollidedEvent += OnControllerCollider;
        controller.onTriggerEnterEvent += OnTriggerEnterEvent;
        controller.onTriggerExitEvent += OnTriggerExitEvent;
    }

    private void OnTriggerExitEvent(Collider2D col)
    {
        Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
        TriggerBase trigger = col.GetComponent<TriggerBase>();
        if (trigger != null)
        {
            trigger.Exit(gameObject);
        }
    }

    private void OnTriggerEnterEvent(Collider2D col)
    {
        Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
        TriggerBase trigger = col.GetComponent<TriggerBase>();
        if (trigger != null)
        {
            trigger.Enter(gameObject);
        }
    }

    private void OnControllerCollider(RaycastHit2D hit)
    {
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );        
    }
    
    void Update()
    {
        if (controller.isGrounded)
        {
            velocity.y = 0;
        }
        //控制左右移动
        if (Input.GetKey(KeyCode.RightArrow))
        {
            normalizedHorizontalSpeed = 1;
            if (transform.localScale.x < 0f)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            if (controller.isGrounded)
                animator.Play(Animator.StringToHash("Run"));
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            normalizedHorizontalSpeed = -1;
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            if (controller.isGrounded)
                animator.Play(Animator.StringToHash("Run"));
        }
        else
        {
            normalizedHorizontalSpeed = 0;
            if (controller.isGrounded)
                animator.Play(Animator.StringToHash("Idle"));
        }
        //控制跳跃
        if (controller.isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            animator.Play(Animator.StringToHash("Jump"));
        }

        // 应用横向速度平滑
        var smoothedMovementFactor = controller.isGrounded ? groundDamping : inAirDamping;
        velocity.x = Mathf.Lerp(velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);

        // 应用重力
        velocity.y += gravity * Time.deltaTime;

        // 向下跳跃
        if (controller.isGrounded && Input.GetKey(KeyCode.DownArrow))
        {
            velocity.y *= 3f;
            controller.ignoreOneWayPlatformsThisFrame = true;
        }

        controller.move(velocity * Time.deltaTime);

        // 更新自身的速度
        velocity = controller.velocity;
    }
}
