using UnityEngine;

public class BallController : MonoBehaviour
{
    private float speed = 10f; // 공의 초기 속도
    [SerializeField] private float baseSpeed = 10f; // 공의 초기 속도
    [SerializeField] private float maxSpeed = 30f; // 최대 속도

    private float power = 10f;
    [SerializeField] private float basePower = 10f;

    [SerializeField] private float paddleSpeedIncrease = 5f;
    [SerializeField] private float paddlePowerIncrease = 10f;

    [SerializeField] private float blockSpeedDecrease = 5f;
    [SerializeField] private float blockPowerDecrease = 10f;

    [SerializeField] private float centerPullStrength = 1.0f; // 중앙으로 끌리는 정도
    [SerializeField] private float centerZone = 0.2f; // 중앙에서 수직으로 튕기는 범위

    [SerializeField] private float ballRadius = 0.7f;

    private Rigidbody2D rb;
    private Transform tr;
    private bool isGameStarted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        LaunchBall();
        isGameStarted = true;
        speed = baseSpeed;
        power = basePower;
        tr = GetComponent<Transform>();
        tr.localScale = new Vector3(ballRadius,ballRadius,ballRadius);
    }

    void LaunchBall()
    {
        // 위쪽 방향 중 랜덤한 각도로 발사
        float xRandom = Random.Range(-1f, 1f);
        Vector2 direction = new Vector2(xRandom, 1f).normalized;
        
        rb.linearVelocity = direction * speed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {   
        speed += 5;
        power += 10;
        if (collision.gameObject.name.Contains("paddle_up") || collision.gameObject.name.Contains("paddle_down"))
        {
            float xOffset = transform.position.x - collision.transform.position.x;

            // 데드존 안이라면 X축 속도를 즉시 0으로 소멸시킴
            if (Mathf.Abs(xOffset) < centerZone)
            {
                // 현재 속도에서 X만 0으로 만들고 Y 방향은 유지하며 속도 재설정
                float yDirection = (collision.gameObject.name.Contains("paddle_down")) ? 0.5f : -0.5f;
                rb.linearVelocity = new Vector2(0, yDirection).normalized * speed;
                
                // 디버깅용 선 그리기 (Game 뷰에서 확인 가능)
                Debug.DrawRay(transform.position, rb.linearVelocity, Color.red, 1f);
            }
        }
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude != speed && rb.linearVelocity != Vector2.zero)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        }

        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }

        if (speed >= baseSpeed)
        {
            speed -= 0.1f; // 속도가 줄어들 수단이 아직 없어서 임시로 작성
        }
       
    }
    
    private void OnDrawGizmos()
    {
        // 씬 뷰에서 데드존 영역을 확인합니다.
        // 유니티 상단 Gizmos 버튼이 켜져 있어야 보입니다.

        // 주의: 이 코드는 임시 확인용입니다. 
        // 패들이 여러 개라면 더 복잡한 로직이 필요하지만, 
        // 지금은 데드존 수치 감을 잡기 위해 아래처럼 간단히 구현해 봅니다.

        Gizmos.color = Color.cyan;
        GameObject bottomPaddle = GameObject.Find("paddle_down"); // 실제 아래 패들 이름으로 변경
        if (bottomPaddle != null)
        {
            Vector3 paddlePos = bottomPaddle.transform.position;
            // 데드존 구간을 하늘색 선으로 표시
            Gizmos.DrawLine(paddlePos + new Vector3(-centerZone, 0.5f, 0), paddlePos + new Vector3(-centerZone, -0.5f, 0));
            Gizmos.DrawLine(paddlePos + new Vector3(centerZone, 0.5f, 0), paddlePos + new Vector3(centerZone, -0.5f, 0));
        }
    }

    //벽돌과 충돌 시 속도 감소
    public void DecreaseByBlockHit()
    {
        speed = Mathf.Max(speed - blockSpeedDecrease, baseSpeed);
        power = Mathf.Max(power - blockPowerDecrease, basePower);

        if (rb != null && rb.linearVelocity != Vector2.zero)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        }
    }
}