using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackController : MonoBehaviour {
    private GameObject _iceBeam;
    private GameObject _hero;
    private SpriteRenderer _heroSprite;
    private ParticleSystem _heroAura;
    private AudioSource _iceBeamSound;

    private bool _canAttack = true;

    [SerializeField]
    private Sprite _heroIdle;
    [SerializeField]
    private Sprite _heroAttacking;

    [SerializeField]
    private GameObject _iceBeamGroup;
    [SerializeField]
    private GameObject _iceRuneGroup;
    [SerializeField]
    private GameObject _glacierGroup;
    [SerializeField]
    private GameObject _icicleGroup;

    private float _iceRuneCooldown = 0.0f;
    private float _glacierCooldown = 0.0f;
    private float _icicleCooldown = 0.0f;

    // Use this for initialization
    void Start () {
        _hero = GameObject.Find("Hero");
        _heroSprite = _hero.GetComponent<SpriteRenderer>();
        _heroAura = GameObject.Find("HeroAura").GetComponent<ParticleSystem>();
        _heroAura.Stop();
        _iceBeamSound = GameObject.Find("IceBeamSound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonUp(0))
        {
            _heroSprite.sprite = _heroIdle;
            _heroAura.Stop();
            _iceBeamSound.Pause();
            if (_iceBeam)
            {
                GameObject.Destroy(_iceBeam);
                _iceBeam = null;
            }
        }

        if (!this._canAttack)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _heroSprite.sprite = _heroAttacking;
            _iceBeamSound.Play();
            _heroAura.Play();
        }
		if (Input.GetMouseButton(0))
        {
            if (!_iceBeam)
            {
                this._iceBeam = GameObject.Instantiate(Resources.Load("IceBeam") as GameObject);
                this._iceBeam.name = "IceBeam";
                var heroPos = this._hero.transform.position;
                heroPos.z += 1;
                _iceBeam.transform.position = heroPos;
            }

            var mousePosition = Input.mousePosition;
            var heroPosition = Camera.main.WorldToScreenPoint(_hero.transform.position);
            mousePosition.x = mousePosition.x - heroPosition.x;
            mousePosition.y = mousePosition.y - heroPosition.y;
            var angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
            _iceBeam.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if (((this._iceBeam.transform.localEulerAngles.z + 90) % 360) < 180)
            {
                if (this._hero.transform.localScale.x == -1.0f)
                {
                    this._hero.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
            } else {

                if (this._hero.transform.localScale.x == 1.0f)
                {
                    this._hero.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Q) && this._iceRuneCooldown <= 0.0f && this._uiController) {
            var iceRune = GameObject.Instantiate(Resources.Load("IceRuneAttack") as GameObject);
            iceRune.name = "IceRune";
            var runePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            runePosition.z = 3;
            iceRune.transform.position = runePosition;

            this._iceRuneCooldown = 2.0f;
            this._iceRuneGroup.transform.Find("Shadow").GetComponent<Image>().enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.W) && this._glacierCooldown <= 0.0f) {
            var glacialPush = GameObject.Instantiate(Resources.Load("GlacialPushAttack") as GameObject);
            glacialPush.name = "GlacialPush";
            glacialPush.transform.position = this._hero.transform.position;

            var mousePosition = Input.mousePosition;
            var heroPosition = Camera.main.WorldToScreenPoint(_hero.transform.position);

            var directionVector = new Vector3(mousePosition.x - heroPosition.x, mousePosition.y - heroPosition.y, 0.0f);
            var angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
            var glacialSprite = glacialPush.transform.Find("Sprite");
            glacialSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            glacialPush.GetComponent<GlacialBehavior>().SetMovementVector(directionVector.normalized);

            this._glacierCooldown = 3.0f;
            this._glacierGroup.transform.Find("Shadow").GetComponent<Image>().enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.E) && this._icicleCooldown <= 0.0f)
        { // && abilityReady
            var icicle = GameObject.Instantiate(Resources.Load("IcicleAttack") as GameObject);
            icicle.name = "Icicle";
            var iciclePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            iciclePosition.z = -5;
            icicle.transform.position = iciclePosition;

            this._icicleCooldown = 4.0f;
            this._icicleGroup.transform.Find("Shadow").GetComponent<Image>().enabled = true;
        }

        TickTimers();
    }

    public void GameOver()
    {
        this._canAttack = false;
    }

    private void TickTimers()
    {
        if (this._iceRuneCooldown > 0.0f)
        {
            this._iceRuneCooldown -= Time.deltaTime;
            this._iceRuneGroup.transform.Find("Cooldown").GetComponent<TextMeshProUGUI>().text = ((int)(Mathf.Ceil(this._iceRuneCooldown))).ToString();

            if (this._iceRuneCooldown <= 0.0f)
            {
                this._iceRuneGroup.transform.Find("Shadow").GetComponent<Image>().enabled = false;
                this._iceRuneGroup.transform.Find("Cooldown").GetComponent<TextMeshProUGUI>().text = "";
            }
        }
        if (this._glacierCooldown > 0.0f)
        {
            this._glacierCooldown -= Time.deltaTime;
            this._glacierGroup.transform.Find("Cooldown").GetComponent<TextMeshProUGUI>().text = ((int)(Mathf.Ceil(this._glacierCooldown))).ToString();

            if (this._glacierCooldown <= 0.0f)
            {
                this._glacierGroup.transform.Find("Shadow").GetComponent<Image>().enabled = false;
                this._glacierGroup.transform.Find("Cooldown").GetComponent<TextMeshProUGUI>().text = "";
            }
        }
        if (this._icicleCooldown > 0.0f)
        {
            this._icicleCooldown -= Time.deltaTime;
            this._icicleGroup.transform.Find("Cooldown").GetComponent<TextMeshProUGUI>().text = ((int)(Mathf.Ceil(this._icicleCooldown))).ToString();

            if (this._icicleCooldown <= 0.0f)
            {
                this._icicleGroup.transform.Find("Shadow").GetComponent<Image>().enabled = false;
                this._icicleGroup.transform.Find("Cooldown").GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }
}
