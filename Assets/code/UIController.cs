using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {
    private RoundController _roundController;

    [SerializeField]
    private GameObject _introGroup;
    [SerializeField]
    private GameObject _nextRoundButton;
    [SerializeField]
    private GameObject _skillNameTextObject;
    private TextMeshProUGUI _skillNameText;
    [SerializeField]
    private GameObject _skillDescriptionTextObject;
    private TextMeshProUGUI _skillDescriptionText;
    [SerializeField]
    private GameObject _forgetTextObject;
    private TextMeshProUGUI _forgetText;
    [SerializeField]
    private GameObject _yesButtonObject;
    [SerializeField]
    private GameObject _skillErrorTextObject;
    private TextMeshProUGUI _skillErrorText;
    [SerializeField]
    private GameObject _skillGroup;
    private CanvasGroup _iceBeamButtons;
    private CanvasGroup _iceRuneButtons;
    private CanvasGroup _glacierButtons;
    private CanvasGroup _icicleButtons;
    [SerializeField]
    private GameObject _gameOverGroup;

    private string _currentSkill;

    public struct SkillSystem
    {
        public bool _hasIceBeam1;
        public bool _hasIceBeam2;
        public bool _hasIceBeam3;
        public bool _hasIceBeam4;
        public bool _hasIceRune1;
        public bool _hasIceRune2;
        public bool _hasIceRune3;
        public bool _hasIceRune4;
        public bool _hasGlacier1;
        public bool _hasGlacier2;
        public bool _hasGlacier3;
        public bool _hasGlacier4;
        public bool _hasIcicle1;
        public bool _hasIcicle2;
        public bool _hasIcicle3;
        public bool _hasIcicle4;

        public void Init()
        {
            _hasIceBeam1 = true;
            _hasIceBeam2 = true;
            _hasIceBeam3 = true;
            _hasIceBeam4 = true;
            _hasIceRune1 = true;
            _hasIceRune2 = true;
            _hasIceRune3 = true;
            _hasIceRune4 = true;
            _hasGlacier1 = true;
            _hasGlacier2 = true;
            _hasGlacier3 = true;
            _hasGlacier4 = true;
            _hasIcicle1 = true;
            _hasIcicle2 = true;
            _hasIcicle3 = true;
            _hasIcicle4 = true;
        }
    }
    public SkillSystem _skillSystem;

    // Use this for initialization
    void Start () {
        _roundController = GetComponent<RoundController>();
        _nextRoundButton.GetComponent<Button>().onClick.AddListener(onNextRoundClicked);

        this._skillNameText = this._skillNameTextObject.GetComponent<TextMeshProUGUI>();
        this._skillDescriptionText = this._skillDescriptionTextObject.GetComponent<TextMeshProUGUI>();
        this._skillErrorText = this._skillErrorTextObject.GetComponent<TextMeshProUGUI>();
        this._forgetText = this._forgetTextObject.GetComponent<TextMeshProUGUI>();

        this._introGroup.GetComponent<CanvasGroup>().alpha = 1.0f;
        this._introGroup.GetComponent<CanvasGroup>().blocksRaycasts = true;

        this._yesButtonObject.GetComponent<Button>().onClick.AddListener(onForgetButtonClicked);

        this._iceBeamButtons = this._skillGroup.transform.Find("IceBeamSkills").GetComponent<CanvasGroup>();
        this._iceRuneButtons = this._skillGroup.transform.Find("IceRuneSkills").GetComponent<CanvasGroup>();
        this._glacierButtons = this._skillGroup.transform.Find("GlacierSkills").GetComponent<CanvasGroup>();
        this._icicleButtons = this._skillGroup.transform.Find("IcicleSkills").GetComponent<CanvasGroup>();

        var skillButtons = this._iceBeamButtons.GetComponentsInChildren<Button>();
        skillButtons[0].onClick.AddListener(onIceBeamButton1Clicked);
        skillButtons[1].onClick.AddListener(onIceBeamButton2Clicked);
        skillButtons[2].onClick.AddListener(onIceBeamButton3Clicked);
        skillButtons[3].onClick.AddListener(onIceBeamButton4Clicked);
        skillButtons = this._iceRuneButtons.GetComponentsInChildren<Button>();
        skillButtons[0].onClick.AddListener(onIceRuneButton1Clicked);
        skillButtons[1].onClick.AddListener(onIceRuneButton2Clicked);
        skillButtons[2].onClick.AddListener(onIceRuneButton3Clicked);
        skillButtons[3].onClick.AddListener(onIceRuneButton4Clicked);
        skillButtons = this._glacierButtons.GetComponentsInChildren<Button>();
        skillButtons[0].onClick.AddListener(onGlacierButton1Clicked);
        skillButtons[1].onClick.AddListener(onGlacierButton2Clicked);
        skillButtons[2].onClick.AddListener(onGlacierButton3Clicked);
        skillButtons[3].onClick.AddListener(onGlacierButton4Clicked);
        skillButtons = this._icicleButtons.GetComponentsInChildren<Button>();
        skillButtons[0].onClick.AddListener(onIcicleButton1Clicked);
        skillButtons[1].onClick.AddListener(onIcicleButton2Clicked);
        skillButtons[2].onClick.AddListener(onIcicleButton3Clicked);
        skillButtons[3].onClick.AddListener(onIcicleButton4Clicked);

        _skillSystem.Init();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
        {
            this._introGroup.GetComponent<CanvasGroup>().alpha = 0.0f;
            this._introGroup.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
	}

    public void GameOver()
    {
        this._gameOverGroup.GetComponent<CanvasGroup>().alpha = 1.0f;
        this._gameOverGroup.transform.Find("SubtitleText2").GetComponent<TextMeshProUGUI>().text =
            "You lasted " + (this._roundController.CurrentRound - 1).ToString() + " rounds!";
        this._gameOverGroup.transform.Find("PlayAgainButton").GetComponent<Button>().onClick.AddListener(onPlayAgainClick);
    }

    private void onPlayAgainClick()
    {
        SceneManager.LoadScene(0);
    }

    public void ShowBetweenLevel()
    {
        this._nextRoundButton.GetComponent<Image>().enabled = true;
        this._nextRoundButton.GetComponent<Button>().enabled = true;
        this._nextRoundButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().enabled = true;
        this._skillGroup.GetComponent<CanvasGroup>().alpha = 1.0f;
        this._skillGroup.GetComponent<CanvasGroup>().blocksRaycasts = true;
        this._skillGroup.GetComponent<CanvasGroup>().interactable = true;
        this._skillNameText.text = "";
        this._skillDescriptionText.text = "";
        SetForgetDialog(false);
    }
    public void HideSkills()
    {
        _skillGroup.GetComponent<CanvasGroup>().alpha = 0.0f;
        _skillGroup.GetComponent<CanvasGroup>().blocksRaycasts = false;
        _skillGroup.GetComponent<CanvasGroup>().interactable = false;
    }

    public void onNextRoundClicked()
    {
        _nextRoundButton.GetComponent<Image>().enabled = false;
        _nextRoundButton.GetComponent<Button>().enabled = false;
        _nextRoundButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().enabled = false;
        _skillGroup.GetComponent<CanvasGroup>().alpha = 0.0f;
        _skillGroup.GetComponent<CanvasGroup>().blocksRaycasts = false;

        _roundController.StartRound();
    }

    private void onForgetButtonClicked()
    {
        GameObject currentButton = null;
        switch(this._currentSkill)
        {
            case "Ice Beam":
                this._skillSystem._hasIceBeam1 = false;
                currentButton = this._iceBeamButtons.transform.Find("SkillIceBeam1").gameObject;
                break;
            case "Ice Eternity":
                this._skillSystem._hasIceBeam4 = false;
                currentButton = this._iceBeamButtons.transform.Find("SkillIceBeam4").gameObject;
                break;
            case "Ice Rune":
                this._skillSystem._hasIceRune1 = false;
                currentButton = this._iceBeamButtons.transform.Find("SkillIceRune4").gameObject;
                break;
            case "Glacier":
                this._skillSystem._hasGlacier1 = false;
                currentButton = this._iceBeamButtons.transform.Find("SkillGlacier4").gameObject;
                break;
            case "Icicle Smash":
                this._skillSystem._hasIcicle1 = false;
                currentButton = this._iceBeamButtons.transform.Find("SkillIcicle4").gameObject;
                break;
        }

        if (currentButton)
        {
            currentButton.GetComponent<Button>().enabled = false;
            currentButton.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            // currentButton.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, 125.0f);
        }
        HideSkills();
    }

    private void SetForgetDialog(bool enabled)
    {
        this._forgetText.enabled = enabled;
        this._yesButtonObject.GetComponent<Button>().enabled = enabled;
        this._yesButtonObject.GetComponent<Image>().enabled = enabled;
        this._yesButtonObject.transform.Find("Text").GetComponent<TextMeshProUGUI>().enabled = enabled;
    }

    private void onIceBeamButton1Clicked()
    {
        this._currentSkill = "Ice Beam";
        this._skillNameText.text = "Ice Beam";
        this._skillDescriptionText.text = "Gain the ability to spawn a constant beam of ice.";
        this._skillErrorText.enabled = this._skillSystem._hasIceBeam2;
        SetForgetDialog(!this._skillSystem._hasIceBeam2);
    }
    private void onIceBeamButton2Clicked()
    {
        this._skillErrorText.enabled = this._skillSystem._hasIceBeam3;
        SetForgetDialog(!this._skillSystem._hasIceBeam3);
    }
    private void onIceBeamButton3Clicked()
    {
        this._skillErrorText.enabled = this._skillSystem._hasIceBeam4;
        SetForgetDialog(!this._skillSystem._hasIceBeam4);
    }
    private void onIceBeamButton4Clicked()
    {
        this._currentSkill = "Ice Eternity";
        this._skillNameText.text = "Ice Eternity";
        this._skillDescriptionText.text = "No cooldown on your ice beam. Shoot it forever!";
        this._skillErrorText.enabled = false;
        SetForgetDialog(true);
    }

    private void onIceRuneButton1Clicked()
    {
        this._currentSkill = "Ice Rune";
        this._skillNameText.text = "Ice Rune";
        this._skillDescriptionText.text = "Gain the ability to lay runes of ice that explode.";

        this._skillErrorText.enabled = this._skillSystem._hasIceRune2;
        SetForgetDialog(!this._skillSystem._hasIceRune2);
    }
    private void onIceRuneButton2Clicked()
    {
        this._skillErrorText.enabled = this._skillSystem._hasIceRune3;
        SetForgetDialog(!this._skillSystem._hasIceRune3);
    }
    private void onIceRuneButton3Clicked()
    {
        this._skillErrorText.enabled = this._skillSystem._hasIceRune4;
        SetForgetDialog(!this._skillSystem._hasIceRune4);
    }
    private void onIceRuneButton4Clicked()
    {
        this._skillErrorText.enabled = false;
        SetForgetDialog(true);
    }

    private void onGlacierButton1Clicked()
    {
        this._currentSkill = "Glacial Push";
        this._skillNameText.text = "Glacial Push";
        this._skillDescriptionText.text = "Gain the ability to create a wall of ice to push away enemies.";
        this._skillErrorText.enabled = this._skillSystem._hasGlacier2;
        SetForgetDialog(!this._skillSystem._hasGlacier2);
    }
    private void onGlacierButton2Clicked()
    {
        this._skillErrorText.enabled = this._skillSystem._hasGlacier3;
        SetForgetDialog(!this._skillSystem._hasGlacier3);
    }
    private void onGlacierButton3Clicked()
    {
        this._skillErrorText.enabled = this._skillSystem._hasGlacier4;
        SetForgetDialog(!this._skillSystem._hasGlacier4);
    }
    private void onGlacierButton4Clicked()
    {
        this._skillErrorText.enabled = false;
        SetForgetDialog(true);
    }

    private void onIcicleButton1Clicked()
    {
        this._currentSkill = "Icicle Smash";
        this._skillNameText.text = "Icicle Smash";
        this._skillDescriptionText.text = "Gain the ability to create a large icicle that does high damage.";
        this._skillErrorText.enabled = this._skillSystem._hasIcicle2;
        SetForgetDialog(!this._skillSystem._hasIcicle2);
    }
    private void onIcicleButton2Clicked()
    {
        this._skillErrorText.enabled = this._skillSystem._hasIcicle3;
        SetForgetDialog(!this._skillSystem._hasIcicle3);
    }
    private void onIcicleButton3Clicked()
    {
        this._skillErrorText.enabled = this._skillSystem._hasIcicle4;
        SetForgetDialog(!this._skillSystem._hasIcicle4);
    }
    private void onIcicleButton4Clicked()
    {
        this._skillErrorText.enabled = false;
        SetForgetDialog(true);
    }
}
