using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {
    private SpriteRenderer _sprite;
    private RoundController _roundController;
    private UIController _uiController;
    private AttackController _attackController;
    private EnemyController _enemyController;
    private AudioSource _enemyDeathSound;
    private GameObject _hero;
    private Vector2 _movementVector;
    private Quaternion _startingQuat;
    private const float MIN_SPEED = 0.5f;
    private float _maxSpeed = 1.8f;
    private float _speed = 1.8f;
    private const float STARTING_HEALTH = 100;
    private float _health = STARTING_HEALTH;

    private const float READJUST_TIME = 1.0f;
    private float _readjustTimer = 0.0f;

    // Use this for initialization
    void Start() {
        this._sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        this._roundController = GameObject.Find("MAIN_CONTROLLER").GetComponent<RoundController>();
        this._uiController = GameObject.Find("MAIN_CONTROLLER").GetComponent<UIController>();
        this._attackController = GameObject.Find("MAIN_CONTROLLER").GetComponent<AttackController>();
        this._enemyController = GameObject.Find("MAIN_CONTROLLER").GetComponent<EnemyController>();
        this._enemyDeathSound = GameObject.Find("EnemyDeathSound").GetComponent<AudioSource>();
        this._hero = GameObject.Find("Hero");
        this._startingQuat = this.transform.rotation;

        CalculateMovementVector();
    }

    // Update is called once per frame
    void Update() {
        this.transform.Translate(_movementVector);

        if (Vector2.Distance(this._hero.transform.position, this.transform.position) < 0.3f)
        {
            this._hero.GetComponent<Animator>().Play("Death");
            this._enemyController.GameOver();
            this._uiController.GameOver();
            this._attackController.GameOver();
            GameObject.Destroy(this.gameObject);
        }

        this._readjustTimer += Time.deltaTime;
        if (this._readjustTimer > READJUST_TIME)
        {
            CalculateMovementVector();
            this._readjustTimer = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            this._health = 0;
        }

        if (this._health <= 0)
        {
            var ghost = GameObject.Instantiate(Resources.Load("Ghost") as GameObject);
            var currentPosition = this.transform.position;
            currentPosition.z -= 1;
            currentPosition.y += 0.6f;
            ghost.transform.position = currentPosition;

            if (this._roundController)
            {
                this._roundController.EnemyKilled();
                this._roundController = null; // This method was getting hit twice so i set this to null so that it doesn't double count an enemy
            }
            this._enemyDeathSound.Play();

            GameObject.Destroy(this.gameObject);
        }
    }

    public float MaxSpeed {
        set { 
            this._maxSpeed = value;
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.name == "IceBeam") {
            this._health -= (this._uiController._skillSystem._hasIceBeam3) ? 2 : 1;
        } else if (collision.collider.name == "Wave") { // Ice Rune
            this._health -= (this._uiController._skillSystem._hasIceRune2) ? 3.0f : 1.5f;
        } else if (collision.collider.name == "Icicle") {
            this._health -= (this._uiController._skillSystem._hasIcicle3) ? STARTING_HEALTH : (STARTING_HEALTH * 0.75f);
        } else if (collision.collider.name == "GlacialPush") {
            this._health -= (this._uiController._skillSystem._hasGlacier4) ? 0.5f : 0.0f;
        }

        this._sprite.color = new Color((this._health / STARTING_HEALTH), 1, 1);
        this._speed = MIN_SPEED + ((this._health / STARTING_HEALTH) * (this._maxSpeed - MIN_SPEED));
        this.transform.rotation = this._startingQuat;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private void CalculateMovementVector()
    {
        var unitToHero = (this._hero.transform.position - this.transform.position).normalized;
        this._movementVector = unitToHero / 100.0f * this._speed;
        this._movementVector = unitToHero / 100.0f * this._speed;
    }
}
