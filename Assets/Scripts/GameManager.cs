
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject deathPanel;
    public TMP_Text scoreTxt;
    public TMP_Text deathScoreTxt;
    public int score;

    int scoreToGet = 50;

    public UpgradeChoose chooser;
    public Shield[] shield;

    public InputActionReference throwAction;

    public int shieldAmount = 1;
    public int shieldActive = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void OnEnable()
    {
        if (throwAction != null)
        {
            throwAction.action.performed += HandleThrowInput;
            throwAction.action.Enable();
        }
    }

    void OnDisable()
    {
        if (throwAction != null)
        {
            throwAction.action.performed -= HandleThrowInput;
        }
    }

    void Start()
    {
        scoreTxt.text = score.ToString();
    }

    public void AddScore(int addition)
    {
        score += addition;
        scoreTxt.text = score.ToString();
    }

    void Update()
    {
        if (score >= scoreToGet)
        {
            ChooseUpgrades();
            float scoreFloat = scoreToGet;
            scoreToGet = (int)(scoreFloat * 1.25f);
        }
    }

    void HandleThrowInput(InputAction.CallbackContext context)
    {
        if (shieldAmount <= 0 || shieldActive >= shieldAmount)
        {
            return;
        }

        shield[shieldActive].HandleThrowInput();
    }

    public void ChooseUpgrades()
    {
        chooser.ResetPanels();
        Time.timeScale = 0;
    }

    public void SelectUpgrade(UpgradePanel panel)
    {
        UpgradeOption option = panel.option;

        if (option.upgradeType == UpgradeOption.UpgradeOptions.Damage)
        {
            for (int i = 0; i < shieldAmount; i++)
            {
                if (option.upgradeSymbol == UpgradeOption.UpgradeAmountType.Add)
                {
                    shield[i].dmg += option.increaseAmt;
                }
                else
                {
                    shield[i].dmg -= option.increaseAmt;
                }
            }
        }
        else if (option.upgradeType == UpgradeOption.UpgradeOptions.Knockback)
        {
            for (int i = 0; i < shieldAmount; i++)
            {
                if (option.upgradeSymbol == UpgradeOption.UpgradeAmountType.Add)
                {
                    shield[i].knockback += option.increaseAmt;
                }
                else
                {
                    shield[i].knockback -= option.increaseAmt;
                }
            }
        }
        else if (option.upgradeType == UpgradeOption.UpgradeOptions.Speed)
        {
            for (int i = 0; i < shieldAmount; i++)
            {
                if (option.upgradeSymbol == UpgradeOption.UpgradeAmountType.Add)
                {
                    shield[i].throwSpeed += option.increaseAmt;
                }
                else
                {
                    shield[i].throwSpeed -= option.increaseAmt;
                }
            }
        }
        else if (option.upgradeType == UpgradeOption.UpgradeOptions.ShieldAmt)
        {
            if (option.upgradeSymbol == UpgradeOption.UpgradeAmountType.Add)
            {
                AddShield();
            }
        }

        chooser.panel.SetActive(false);
        Time.timeScale = 1;
    }

    public void AddShield()
    {
        if (shieldAmount >= shield.Length)
        {
            return;
        }

        shield[shieldAmount].gameObject.SetActive(true);
        shieldAmount++;

        shieldActive %= shieldAmount;

        Debug.Log($"Shield Amount: {shieldAmount}");
    }

    public void ChangeActiveShield()
    {
        shieldActive = (shieldActive + 1) % shieldAmount;

        Debug.Log($"Active Shield: {shieldActive}, Shield Amount: {shieldAmount}");
    }

    public void DeathStuff()
    {
        deathScoreTxt.text = $"Score: {score}";
        Time.timeScale = 0;
    }
}
