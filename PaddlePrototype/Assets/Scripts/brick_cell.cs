using UnityEngine;

public class BrickCell : MonoBehaviour, IBallHitReceiver
{
    private BrickManager manager;
    private Vector2Int cell;
    private bool isFixed;
    private int version;

    public void Init(
        BrickManager manager,
        Vector2Int cell,
        bool isFixed,
        int version
    )
    {
        this.manager = manager;
        this.cell = cell;
        this.isFixed = isFixed;
        this.version = version;
    }

    public void OnBallHit()
    {
        if (isFixed)
            return;

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (manager != null)
            manager.NotifyBrickDestroyed(cell, isFixed, version);
    }

    public bool IsFixedBrick()
    {
        return isFixed;
    }

    //unity 물리 사용 시 (circlecast 방식엔 필요 x)
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball"))
            return;

        BallController ball = collision.gameObject.GetComponent<BallController>();

        if (ball != null)
           //ball.DecreaseByBlockHit();

        if (isFixed)
            return;

        Destroy(gameObject);
    }*/
}