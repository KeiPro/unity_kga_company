using UnityEngine;
using System;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour, ICharacter
{
    public static event Action<Transform> OnPlayerSpawned;

    public Action onRun;
    public Action onWalk;
    public Action onJump;

    private int m_level;
    private int m_hp;
    private int m_maxHp;
    private int m_stamina;

    private int m_maxStamina;

    private Animator m_animator;

    private Transform m_cameraTransfrom;

    CharacterMovement m_characterMovement;
    // [SerializeField]
    // GameObject m_radarPrefab;
    [SerializeField]
    Radar m_radar;





    private bool m_isJumping = false;

    public int Level
    {
        get { return m_level; }
        set {
            if (value >= 0)
                m_level = value;
        }
    }

    public int HP
    {
        get { return m_hp; }
        set {
            if (value <= m_maxHp)
                m_hp = value;
            else
                m_hp = m_maxHp;
        }
    }

    public int Stamina
    {
        get { return m_stamina; }
        set {

            if (value <= m_maxStamina)
                m_stamina = value;
            else
                m_stamina = m_maxStamina;
        }
    }

    public int MaxHP
    {
        get { return m_maxHp; }
        set { m_maxHp = value; }
    }

    public int MaxStamina
    {
        get { return m_maxStamina; }
        set { m_maxStamina = value; }
    }
    public Animator animator
    {
        get { return m_animator; }
        set { m_animator = value; }
    }





    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            transform.position = new Vector3(transform.position.x, 13f, transform.position.z);
            OnPlayerSpawned?.Invoke(transform);

            const string playerSelfLayerName = "PlayerSelf";
            SetLayerRecursively(transform.gameObject, LayerMask.NameToLayer(playerSelfLayerName));
        }
        else
        {
            const string defaultLayer = "Default";
            SetLayerRecursively(transform.gameObject, LayerMask.NameToLayer(defaultLayer));
        }
    }

    private const string _radarName = "Char_Rader";
    private void SetLayerRecursively(GameObject playerChildObj, int newLayer)
    {
        if (playerChildObj == null)
            return;

        if (playerChildObj.name == _radarName)
            return;

        playerChildObj.layer = newLayer;

        foreach (Transform child in playerChildObj.transform)
        {
            if (child != null)
            {
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        m_characterMovement = GetComponent<CharacterMovement>();
        //m_inventory = GetComponent<CharacterInventory>();
        m_characterMovement.onJumpEnd = () => {
            m_isJumping = false;
        };

        CreateRadar();
    }

    void Update()
    {
        if (!IsOwner)
            return;

        MoveMentInput();
        RadarInput();
        TestInput();
    }

    void TestInput()
    {
        if (Input.GetKeyDown(KeyCode.U) && IsOwner)
        {
            SubmitTributeServerRpc(100);
        }
    }
        
    [ServerRpc]
    private void SubmitTributeServerRpc(int price)
    {
        // 서버에서 메시지 발행
        var message = new TributeSubmittedEventMessage { price = price };
        GetComponent<ServerMessagePublisher>().Publish(message);
    }

    void ResetCharactorState()
    {
        // 모든 파라미터 가져오기
        AnimatorControllerParameter[] parameters = animator.parameters;

        foreach (var p in parameters)
        {
            if (p.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(p.name, false);
            }
        }
    }

    void MoveMentInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetCharactorState();
            animator.SetBool("IsJump", true);
            m_isJumping = true;
            onJump();
        }
        // 이동
        else if (Input.GetKey(KeyCode.W))
        {

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (m_isJumping != true)
                {
                    ResetCharactorState();
                    animator.SetBool("IsRun", true);
                }
                onRun();
            }

            else
            {
                if (m_isJumping != true)
                {
                    ResetCharactorState();
                    animator.SetBool("IsWalking", true);
                }
                onWalk();
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (m_isJumping != true)
            {
                ResetCharactorState();
                animator.SetBool("IsStrafe", true);
            }

        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (m_isJumping != true)
            {
                ResetCharactorState();
                animator.SetBool("IsBackWalking", true);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (m_isJumping != true)
            {
                ResetCharactorState();
                animator.SetBool("IsStrafe", true);
            }
        }

        else
        {
            if (m_isJumping != true)
            {
                ResetCharactorState();
                animator.SetBool("IsIdle", true);
            }
        }
    }


    void RadarInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
            m_radar.StartRadar();
    }
    void CreateRadar()
    {
        // m_radar = Instantiate(m_radarPrefab).GetComponent<Radar>();
        // m_radar.transform.SetParent(transform, false);
        // m_radar.transform.position = gameObject.transform.position;
    }
}
