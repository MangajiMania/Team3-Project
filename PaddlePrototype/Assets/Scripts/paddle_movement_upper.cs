using UnityEngine;
using UnityEngine.InputSystem; // 1. 네임스페이스 추가
using UnityEngine.U2D;

public class UpperPadController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float paddleWidth = 0.7f;
    [SerializeField] private float activeCollisionEnabled = 1f;
    private float transparentAlpha = 0.3f;
    private Camera mainCamera;

    private SpriteShapeRenderer shapeRenderer; 
    private Color originalColor; 

    private Collider2D paddleCollider;
    private Transform tr;
    private bool isCollisionOn = true;

    void Start()
    {
        mainCamera = Camera.main;
        shapeRenderer = GetComponent<SpriteShapeRenderer>();
        originalColor = shapeRenderer.color;
        paddleCollider = GetComponent<Collider2D>();
        tr = GetComponent<Transform>();
        tr.localScale = new Vector3(paddleWidth,0.5f,1f);
    }

    void Update()
    {
        // 2. Mouse.current를 사용하여 클릭 체크
        if (activeCollisionEnabled == 1) 
        {
                if (Mouse.current.leftButton.isPressed)
            {
                SetAlpha(1.0f);
                MovePad();
                paddleCollider.enabled = true;
            }
            else
            {
                SetAlpha(transparentAlpha);
                paddleCollider.enabled = false;
            }
        }
        else
        {
            paddleCollider.enabled = false;
        }
        
    }

    void MovePad()
    {
        // 3. 마우스 위치 읽기
        Vector2 mousePixelPos = Mouse.current.position.ReadValue();
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(new Vector3(mousePixelPos.x, mousePixelPos.y, -mainCamera.transform.position.z));
        
        Vector3 targetPos = new Vector3(mousePos.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        float clampedX = Mathf.Clamp(transform.position.x, -5.5f, 5.5f);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    void SetAlpha(float alpha)
    {
        Color newColor = originalColor;
        newColor.a = alpha;
        shapeRenderer.color = newColor;
    }
}