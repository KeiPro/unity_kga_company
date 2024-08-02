using System;
using System.Collections;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovement : NetworkBehaviour
{
    public Action onJumpEnd;

    [SerializeField]
    private float m_moveSpeed = 5f;

    [SerializeField]
    float m_rotationSpeed = 2f;
    [SerializeField]
    float m_jumpSpeed = 25f;
    private CharacterController m_controller;
    private Vector3 m_moveDirection;

    private Transform m_cameraTransform;

    CharacterManager m_player;

    bool m_isPlayerRun = false;
    bool m_isGrounded = false;

    [SerializeField]
    float m_mass = 30f;
    float m_gravity = 10f;
    float m_currentGravity = 0;
    [SerializeField]
    float m_jumpHeight;

    bool m_isJumping = false;



    void Start()
    {
        m_controller = GetComponent<CharacterController>();
        m_cameraTransform = Camera.main.transform;
        m_player = GetComponent<CharacterManager>();
        m_player.onRun = OnPlayerRun;
        m_player.onWalk = OnPlayerWalk;
        m_player.onJump = OnPlayerJump;
    }

    void Update()
    {
        if (!IsOwner)
            return;
        
        PlayerRotate();
        PlayerMove();
    }

    void PlayerMove()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 cameraForward = m_cameraTransform.forward;
        Vector3 cameraRight = m_cameraTransform.right;

        cameraForward.y = 0f; // y축 방향은 고려하지 않음
        cameraRight.y = 0f;

        cameraForward.Normalize(); // 벡터 길이를 1로 정규화
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;
        float dotProduct = Vector3.Dot(moveDirection, cameraForward);
        float movespeed = m_moveSpeed;
        if (m_isPlayerRun == false)
        {
            if (Mathf.Approximately(dotProduct, 0f))
            {
                movespeed = m_moveSpeed * 0.75f;
            }
        }
        else
        {
            movespeed = m_moveSpeed * 2f;
        }

        moveDirection *= movespeed;
        m_controller.Move(GravityAppliedDirection(moveDirection) * Time.deltaTime);
    }

    void PlayerRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 회전값에 회전 속도를 곱하여 실제 회전량을 계산
        float rotationAmountX = mouseX * m_rotationSpeed;
        float rotationAmountY = mouseY * m_rotationSpeed;

        // 카메라의 현재 회전값을 가져옴
        Vector3 currentRotation = m_cameraTransform.rotation.eulerAngles;

        // Y 축 회전 적용
        m_cameraTransform.Rotate(Vector3.up, rotationAmountX, Space.World);
        transform.Rotate(Vector3.up, rotationAmountX, Space.World);

        // X 축 회전 적용 후 제한
        float targetAngleX = currentRotation.x - rotationAmountY;
        if (targetAngleX >= 270f && targetAngleX <= 360f)
            targetAngleX = Mathf.Clamp(targetAngleX, 270f, 360f);
        else if (targetAngleX <= 90f && targetAngleX >= -90f)
            targetAngleX = Mathf.Clamp(targetAngleX, -90f, 90f);
        else if (targetAngleX < 270f && targetAngleX > 180f)
            targetAngleX = 270f;
        else if (targetAngleX > 90f && targetAngleX < 180f)
            targetAngleX = 90f;

        m_cameraTransform.rotation = Quaternion.Euler(targetAngleX, m_cameraTransform.rotation.eulerAngles.y, 0);
    }

    void OnPlayerRun()
    {
        m_isPlayerRun = true;
    }

    void OnPlayerWalk()
    {
        m_isPlayerRun = false;
    }

    void OnPlayerJump()
    {
        if (m_isGrounded == true)
        {
            m_isJumping = true;
            m_isGrounded = false;
            StartCoroutine(JumpRoutine());
        }
    }

    Vector3 GravityAppliedDirection(Vector3 moveDirection)
    {
        CheckPlayerIsGrounded();
        if (m_isGrounded == false)
        {

            m_currentGravity += m_gravity * 0.1f;
            moveDirection.y -= m_currentGravity * Time.deltaTime * m_mass;

        }
        else return moveDirection;

        return moveDirection;
    }


    void CheckPlayerIsGrounded()
    {
        Ray ray = new Ray(m_controller.bounds.center, Vector3.down);

        int layerToIgnore = 6;
        int layerMask = ~(1 << layerToIgnore);
        // 광선이 땅과 충돌하는지 확인
        if (Physics.Raycast(ray, out RaycastHit hit, m_controller.bounds.extents.y + 0.1f, layerMask))
        {

            // 만약 물체에 맞았다면 해당 위치에 표시합니다.
            Debug.DrawLine(ray.origin, hit.point, Color.red);

            // 충돌한 오브젝트가 'Ground' 태그를 가지고 있는지 확인
            if (hit.collider.CompareTag("Ground"))
            {
                m_isGrounded = true; // 땅
                m_currentGravity = 0f;
                onJumpEnd();
            }
        }
        else
        {
            // 만약 아무것도 맞지 않았다면 ray를 끝까지 그립니다.
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
            m_isGrounded = false; // 하늘
        }
    }

    IEnumerator JumpRoutine()
    {
        float grav = m_gravity;
        m_gravity = 0f;
        while (Mathf.Ceil(transform.position.y) < m_jumpHeight)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, m_jumpHeight, m_jumpSpeed * Time.deltaTime), transform.position.z);
            yield return null;
        }

        m_isJumping = false;
        m_gravity = grav;
    }

    public bool IsInFieldOfView(Transform target)
    {
        Vector3 directionToTarget = (target.position - Camera.main.transform.position).normalized;
        float angleToTarget = Vector3.Angle(Camera.main.transform.forward, directionToTarget);
        Debug.Log(angleToTarget <= 90);
        Debug.Log(angleToTarget);
        return angleToTarget <= 90;
    }

    public bool IsInFieldOfView(Vector3 target)
    {
        Vector3 directionToTarget = (target - Camera.main.transform.position).normalized;
        float angleToTarget = Vector3.Angle(Camera.main.transform.forward, directionToTarget);
        Debug.Log(angleToTarget <= 90);
        Debug.Log(angleToTarget);
        return angleToTarget <= 90;
    }
}