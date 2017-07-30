using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundController : MonoBehaviour {
    private UIController _uiController;
    private EnemyController _enemyController;

    [SerializeField]
    private GameObject _waveText;

    [SerializeField]
    private GameObject _enemiesLeftGroup;
    private Stack<SpriteRenderer> _enemiesLeftSprites;
    
    private int _currentRound = 0;
    private int _enemiesKilledThisRound = 0;
    private int _enemiesNeededThisRound = 10;

	// Use this for initialization
	void Start () {
        _uiController = GetComponent<UIController>();
        _enemyController = GetComponent<EnemyController>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartRound()
    {
        this._currentRound++;
        this._enemiesKilledThisRound = 0;
        this._enemiesNeededThisRound = 10 + (3 * this._currentRound);

        var enemySprites = this._enemiesLeftGroup.GetComponentsInChildren<SpriteRenderer>();
        foreach(var sprite in enemySprites)
        {
            sprite.enabled = false;
        }
        this._enemiesLeftSprites = new Stack<SpriteRenderer>();
        for (int i=0; i<this._enemiesNeededThisRound; i++)
        {
            if (enemySprites[i] != null)
            {
                enemySprites[i].enabled = true;
                this._enemiesLeftSprites.Push(enemySprites[i]);
            }
        }

        this._waveText.GetComponent<TextMeshProUGUI>().text = "Wave " + this._currentRound.ToString();
        this._waveText.GetComponent<Animator>().Play("TextGrowAndFade");

        this._enemyController.StartSpawning();
    }

    public int EnemiesThisRound
    {
        get
        {
            return this._enemiesNeededThisRound;
        }
    }
    public int CurrentRound
    {
        get
        {
            return this._currentRound;
        }
    }

    public void EnemyKilled()
    {
        this._enemiesKilledThisRound++;
        if (this._enemiesLeftSprites.Count > 0)
        {
            var enemySprite = this._enemiesLeftSprites.Pop();
            enemySprite.enabled = false;
        }
        if (this._enemiesKilledThisRound >= _enemiesNeededThisRound)
        {
            this._uiController.ShowBetweenLevel();
        }
    }
}
