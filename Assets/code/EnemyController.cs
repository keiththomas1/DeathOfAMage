using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    private RoundController _roundController;
    private float _enemySpawnTimer = 0.0f;
    private const float MIN_SPAWN_TIME = 0.6f;
    private const float MAX_SPAWN_TIME = 1.1f;
    private float _currentSpawnTime = 0.0f;
    private List<GameObject> _enemies;
    private bool _isSpawning = false;
    
	// Use this for initialization
	void Start () {
        _roundController = GetComponent<RoundController>();
        _enemies = new List<GameObject>();
        _currentSpawnTime = MAX_SPAWN_TIME;
    }
	
	// Update is called once per frame
	void Update () {
        if (this._isSpawning)
        {
            this._enemySpawnTimer += Time.deltaTime;
            if (this._enemySpawnTimer > this._currentSpawnTime)
            {
                this._enemySpawnTimer = 0.0f;
                this._currentSpawnTime = Random.Range(MIN_SPAWN_TIME, MAX_SPAWN_TIME);
                var randomPosition = new Vector3(
                    (Random.Range(0.0f, 1.0f) > 0.5f) ? -8.0f : 8.0f,
                    Random.Range(-9.0f, 9.0f),
                    2.0f);
                var newEnemy = GameObject.Instantiate(Resources.Load("Enemy1") as GameObject);
                newEnemy.transform.position = randomPosition;
                newEnemy.transform.localScale = new Vector3((randomPosition.x < 0) ? 1.0f : -1.0f, 1.0f, 1.0f);
                newEnemy.GetComponent<EnemyBehavior>().MaxSpeed = 1.8f + (this._roundController.CurrentRound / 10.0f);
                _enemies.Add(newEnemy);

                if (this._roundController.EnemiesThisRound <= _enemies.Count)
                {
                    StopSpawning();
                }
            }
        }
	}

    public void GameOver()
    {
        foreach(var enemy in this._enemies)
        {
            if (enemy)
            {
                GameObject.Destroy(enemy);
            }
        }
    }

    public void StopSpawning()
    {
        this._isSpawning = false;
        this._enemies = new List<GameObject>();
    }
    public void StartSpawning()
    {
        this._isSpawning = true;
    }
}
