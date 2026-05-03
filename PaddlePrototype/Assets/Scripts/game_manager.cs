using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;

        [Header("Spawn")]
        public Vector2Int startCell = new Vector2Int(0, 0);

        [Header("Move Commands")]
        public List<EnemyGridMover.MoveCommand> moveCommands = new();

        [Header("Move Setting")]
        public float moveInterval = 1.0f;
        public float moveSpeed = 3.0f;
        public bool loopAllCommands = false;
    }

    [Header("Enemy")]
    [SerializeField] private BrickManager brickManager;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private List<EnemySpawnData> enemies = new();

    private readonly List<EnemyGridMover> spawnedEnemies = new();

    private void Start()
    {
        SpawnAllEnemies();
    }

    public void SpawnAllEnemies()
    {
        if (brickManager == null)
            brickManager = FindAnyObjectByType<BrickManager>();

        ClearEnemies();

        foreach (EnemySpawnData data in enemies)
        {
            SpawnEnemy(data);
        }
    }

    private void SpawnEnemy(EnemySpawnData data)
    {
        if (data.enemyPrefab == null)
            return;

        if (brickManager == null)
            return;

        Vector3 spawnPos = brickManager.GetCellWorldPosition(data.startCell);

        GameObject enemyObj = Instantiate(
            data.enemyPrefab,
            spawnPos,
            Quaternion.identity,
            enemyParent
        );

        EnemyGridMover mover = enemyObj.GetComponent<EnemyGridMover>();

        if (mover == null)
            mover = enemyObj.AddComponent<EnemyGridMover>();

        mover.Init(
           brickManager,
            data.startCell,
            data.moveCommands,
            data.moveInterval,
            data.moveSpeed,
            data.loopAllCommands
        );

        spawnedEnemies.Add(mover);
    }

    public void ClearEnemies()
    {
        foreach (EnemyGridMover enemy in spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy.gameObject);
        }

        spawnedEnemies.Clear();

        if (enemyParent != null)
        {
            for (int i = enemyParent.childCount - 1; i >= 0; i--)
            {
                Destroy(enemyParent.GetChild(i).gameObject);
            }
        }
    }
}