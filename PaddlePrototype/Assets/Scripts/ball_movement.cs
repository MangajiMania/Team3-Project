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

    public LayerMask collisionMask; // 벽과 패들 레이어를 선택하세요

    private Transform tr;
    private Vector2 direction;
    private bool isGameStarted = false;

    void Start()
    {
        direction = new Vector2(0.5f, 1f).normalized;
        //LaunchBall();
        isGameStarted = true;
        speed = baseSpeed;
        power = basePower;
        tr = GetComponent<Transform>();
        tr.localScale = new Vector3(ballRadius,ballRadius,ballRadius);
    }

    void Update()
    {
        MoveBall(speed * Time.deltaTime);
        if (speed < baseSpeed)
        {
            speed = baseSpeed;
        }
        if (speed >= maxSpeed)
        {
            speed = maxSpeed;
        }
        speed -= 0.025f;
    }

    void MoveBall(float distance)
    {
        // 1. CircleCast로 이동 경로에 장애물이 있는지 확인
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, ballRadius, direction, distance, collisionMask);

        if (hit.collider != null)
        {
            // 2. 충돌 지점까지 우선 이동 (충돌 지점에서 아주 살짝 띄움)
            float distanceToHit = hit.distance;
            transform.Translate(direction * distanceToHit, Space.World);

            // 3. 충돌 대상에 따른 반사 방향 계산
            float remainingDistance = distance - distanceToHit;
            UpdateDirection(hit);

            // 4. 남은 거리가 있다면 새로운 방향으로 다시 이동 (재귀 호출 방지를 위해 단순화)
            if (remainingDistance > 0)
            {
                transform.Translate(direction * remainingDistance, Space.World);
            }
        }
        else
        {
            // 충돌이 없다면 지정된 거리만큼 직선 이동
            transform.Translate(direction * distance, Space.World);
        }
    }

    void UpdateDirection(RaycastHit2D hit)
    {
        GameObject obj = hit.collider.gameObject;

        // 패들 충돌 로직
        if (obj.name.Contains("paddle_up") || obj.name.Contains("paddle_down"))
        {
            float xOffset = transform.position.x - obj.transform.position.x;
            float yDir = (transform.position.y > obj.transform.position.y) ? 1f : -1f;

            float returnX = 0f;
            if (Mathf.Abs(xOffset) >= centerZone)
            {
                returnX = -xOffset * centerPullStrength;
            }

            direction = new Vector2(returnX, yDir).normalized;
            if (obj.name.Contains("paddle_down"))
            {
                speed += 5f;
            }
        }
        else
        {
            // 벽이나 기타 오브젝트: 일반적인 물리 반사 법칙 적용
            direction = Vector2.Reflect(direction, hit.normal).normalized;
        }
    }

    // 에디터 씬 뷰에서 공의 충돌 범위를 확인하기 위한 기즈모
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ballRadius);
    }

    //void LaunchBall()
    //{
    //    // 위쪽 방향 중 랜덤한 각도로 발사
    //    float xRandom = Random.Range(-1f, 1f);
    //    Vector2 direction = new Vector2(xRandom, 1f).normalized;
    //    
    //    rb.linearVelocity = direction * speed;
    //}

    //private void OnCollisionStay2D(Collision2D collision)
    //{   
    //    speed += 5;
    //    power += 10;
    //    if (collision.gameObject.name.Contains("paddle_up") || collision.gameObject.name.Contains("paddle_down"))
    //    {
    //        float xOffset = transform.position.x - collision.transform.position.x;
//
    //        // 데드존 안이라면 X축 속도를 즉시 0으로 소멸시킴
    //        if (Mathf.Abs(xOffset) < centerZone)
    //        {
    //            // 현재 속도에서 X만 0으로 만들고 Y 방향은 유지하며 속도 재설정
    //            float yDirection = (collision.gameObject.name.Contains("paddle_down")) ? 0.5f : -0.5f;
    //            rb.linearVelocity = new Vector2(0, yDirection).normalized * speed;
    //            
    //            // 디버깅용 선 그리기 (Game 뷰에서 확인 가능)
    //            Debug.DrawRay(transform.position, rb.linearVelocity, Color.red, 1f);
    //        }
    //    }
    //}

    //void FixedUpdate()
    //{
    //    if (rb.linearVelocity.magnitude != speed && rb.linearVelocity != Vector2.zero)
    //    {
    //        rb.linearVelocity = rb.linearVelocity.normalized * speed;
    //    }
//
    //    if (speed > maxSpeed)
    //    {
    //        speed = maxSpeed;
    //    }
//
    //    if (speed >= baseSpeed)
    //    {
    //        speed -= 0.1f; // 속도가 줄어들 수단이 아직 없어서 임시로 작성
    //    }
    //   
    //}
    
}