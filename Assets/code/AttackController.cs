using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackController : MonoBehaviour
{
    private UIController _uiController;
    private GameObject _iceBeam;
    private GameObject _hero;
    private SpriteRenderer _heroSprite;
    private ParticleSystem _heroAura;
    private AudioSource _iceBeamSound;
    private AudioSource _iceRuneSound;
    private AudioSource _glacierSound;
    private AudioSource _iceSpikeSound;

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

    [SerializeField]
    private GameObject _iceBeamBar;
    private float _iceBeamPower = 100.0f;

    // Use this for initialization
    void Start () {
        this._uiController = GetComponent<UIController>();
        this._hero = GameObject.Find("Hero");
        this._heroSprite = this._hero.GetComponent<SpriteRenderer>();
        this._heroAura = GameObject.Find("HeroAura").GetComponent<ParticleSystem>();
        this._heroAura.Stop();
        this._iceBeamSound = GameObject.Find("IceBeamSound").GetComponent<AudioSource>();
        this._iceRuneSound = GameObject.Find("IceRuneSound").GetComponent<AudioSource>();
        this._glacierSound = GameObject.Find("GlacierSound").GetComponent<AudioSource>();
        this._iceSpikeSound = GameObject.Find("IceSpikeSound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonUp(0))
        {
            StopCastingIceBeam();
        }

        if (!this._canAttack)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (this._iceBeamPower > 0 && this._uiController._skillSystem._hasIceBeam1)
            {
                this._heroSprite.sprite = this._heroAttacking;
                this._iceBeamSound.Play();
                this._heroAura.Play();
            }
        }
		if (Input.GetMouseButton(0))
        {
            if (this._iceBeamPower > 0 && this._uiController._skillSystem._hasIceBeam1)
            {
                if (!_iceBeam)
                {
                    this._iceBeam = GameObject.Instantiate(Resources.Load("IceBeam") as GameObject);
                    this._iceBeam.name = "IceBeam";
                    if (!this._uiController._skillSystem._hasIceBeam2)
                    {
                        this._iceBeam.transform.localScale = new Vector3(0.75f, 0.75f, 1.0f);
                        var trail = this._iceBeam.transform.Find("Trail");
                        if (trail)
                        {
                            GameObject.Destroy(trail.gameObject);
                        }
                    }
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
                }
                else
                {
                    if (this._hero.transform.localScale.x == 1.0f)
                    {
                        this._hero.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    }
                }

                if (!this._uiController._skillSystem._hasIceBeam4)
                {
                    this._iceBeamPower--;
                    var newScale = this._iceBeamBar.transform.localScale;
                    newScale.x = this._iceBeamPower / 100.0f;
                    this._iceBeamBar.transform.localScale = newScale;

                    if (this._iceBeamPower <= 0.5f)
                    {
                        StopCastingIceBeam();
                    }
                }
            }
        }
        else
        {
            if (this._iceBeamPower < 100)
            {
                this._iceBeamPower += 0.5f;
                var newScale = this._iceBeamBar.transform.localScale;
                newScale.x = this._iceBeamPower / 100.0f;
                this._iceBeamBar.transform.localScale = newScale;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && this._iceRuneCooldown <= 0.0f && this._uiController._skillSystem._hasIceRune1) {
            var iceRune = GameObject.Instantiate(Resources.Load("IceRuneAttack") as GameObject);
            iceRune.name = "IceRune";
            var runePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            runePosition.z = 3;
            iceRune.transform.position = runePosition;
            if (!this._uiController._skillSystem._hasIceRune4)
            {
                iceRune.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
            }
            this._iceRuneSound.Play();

            this._iceRuneCooldown = (this._uiController._skillSystem._hasIceRune3) ? 3.0f : 5.0f;
            this._iceRuneGroup.transform.Find("Shadow").GetComponent<Image>().enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.W) && this._glacierCooldown <= 0.0f && this._uiController._skillSystem._hasGlacier1) {
            var glacialPush = GameObject.Instantiate(Resources.Load("GlacialPushAttack") as GameObject);
            glacialPush.name = "GlacialPush";
            glacialPush.transform.position = this._hero.transform.position;
            if (!this._uiController._skillSystem._hasGlacier3)
            {
                glacialPush.transform.localScale = new Vector3(1.0f, 0.75f, 1.0f);
            }
            this._glacierSound.Play();

            var mousePosition = Input.mousePosition;
            var heroPosition = Camera.main.WorldToScreenPoint(_hero.transform.position);

            var directionVector = new Vector3(mousePosition.x - heroPosition.x, mousePosition.y - heroPosition.y, 0.0f);
            var angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
            var glacialSprite = glacialPush.transform.Find("Sprite");
            glacialSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            glacialPush.GetComponent<GlacialBehavior>().SetMovementVector(directionVector.normalized);

            this._glacierCooldown = (this._uiController._skillSystem._hasGlacier2) ? 3.0f : 6.0f;
            this._glacierGroup.transform.Find("Shadow").GetComponent<Image>().enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.E) && this._icicleCooldown <= 0.0f && this._uiController._skillSystem._hasIcicle1)
        { // && abilityReady
            var icicle = GameObject.Instantiate(Resources.Load("IcicleAttack") as GameObject);
            icicle.name = "Icicle";
            var iciclePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            iciclePosition.z = -5;
            icicle.transform.position = iciclePosition;
            if (!this._uiController._skillSystem._hasIcicle2) {
                icicle.transform.localScale = new Vector3(0.75f, 0.75f, 1.0f);
            }
            this._iceSpikeSound.Play();

            this._icicleCooldown = (this._uiController._skillSystem._hasIcicle4) ? 4.0f : 7.0f;
            this._icicleGroup.transform.Find("Shadow").GetComponent<Image>().enabled = true;
        }

        TickTimers();
    }

    private void StopCastingIceBeam()
    {
        this._heroSprite.sprite = _heroIdle;
        this._heroAura.Stop();
        this._iceBeamSound.Pause();
        if (this._iceBeam)
        {
            GameObject.Destroy(this._iceBeam);
            this._iceBeam = null;
        }
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
