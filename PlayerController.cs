using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float baseMovementSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    
    private float currentMovementSpeed;
    private Vector3 moveDirection;
    private Rigidbody rb;

    // 아이템 효과 관련 변수
    private float speedBoostMultiplier = 1f;
    private float speedBoostDuration = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentMovementSpeed = baseMovementSpeed;
    }

    private void Start()
    {
        currentMovementSpeed = baseMovementSpeed;
    }

    private void Update()
    {
        // 입력값 처리만 Update에서 수행
        HandleInput();
        
        // 아이템 지속시간 체크
        UpdateSpeedBoost();
    }

    private void FixedUpdate()
    {
        // 실제 이동은 FixedUpdate에서 처리
        Move();
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // A, D
        float verticalInput = Input.GetAxisRaw("Vertical");     // W, S

        moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
    }

    private void Move()
    {
        if (moveDirection != Vector3.zero)
        {
            Vector3 movement = moveDirection * (currentMovementSpeed * speedBoostMultiplier);
            rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        }
    }

    // 아이템 효과 관련 메서드
    public void ApplySpeedBoost(float multiplier, float duration)
    {
        speedBoostMultiplier = multiplier;
        speedBoostDuration = duration;
    }

    private void UpdateSpeedBoost()
    {
        if (speedBoostDuration > 0)
        {
            speedBoostDuration -= Time.deltaTime;
            
            if (speedBoostDuration <= 0)
            {
                speedBoostMultiplier = 1f;
            }
        }
    }
} 