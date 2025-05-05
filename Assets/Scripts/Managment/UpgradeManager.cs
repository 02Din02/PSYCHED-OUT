using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private int currency;
    [SerializeField] private int attemptNum;
    [SerializeField] private TextMeshProUGUI currencyDisplay;
     [SerializeField] private TextMeshProUGUI attemptNumberDisplay;
    [SerializeField] private TextMeshProUGUI costDisplay;
    [SerializeField] private TextMeshProUGUI currentStatsDisplay;
    [SerializeField] private TextMeshProUGUI upgradeEffectDisplay;


    private Dictionary<string, int> upgradeTracker = new Dictionary<string, int>();

    [SerializeField] private PlayerStats statsManager;
    // set in editor, should affect CurrentStats object
    [SerializeField] private PlayerStats baseStats;
    // set in editor, should be BaseStats object

    private readonly string[] upgradeList = { "Instinct", "Adrenaline", "Vital", "Harmony" };
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Initializing UpgradeManager");
        currency = DataManager.Instance.currency;
        attemptNum = DataManager.Instance.attemptNum;
        UpdateText();
        foreach (string upgrade in upgradeList)
        {
            upgradeTracker.Add(upgrade, 0);
        }
        SyncStats();
        Debug.Log("UpgradeManager Initialized");
    }

    void UpdateText()
    {
        currencyDisplay.text = $"{currency}";
        attemptNumberDisplay.text = $"# {attemptNum}";
    }

    public void PurchaseUpgrade(string upgrade)
    {
        Debug.Assert(upgradeTracker.ContainsKey(upgrade));
        Debug.Log("Attempting upgrade: " + upgrade);
        if (currency - CalculateCost(upgrade) >= 0)
        {
            currency -= CalculateCost(upgrade);
            upgradeTracker[upgrade] += 1;
        }
        else
        {
            Debug.Log("No more money :(");
            // broadcast event no money lolsies
        }
        UpdateText();
        ShowCost(upgrade);
        SyncStats();
    }

    public void ShowCost(string upgrade)
    {
        costDisplay.text = $"- {CalculateCost(upgrade)}";
    }
    public void HideCost()
    {
        costDisplay.text = "";
    }

    private int CalculateCost(string upgrade)
    {
        return 1 + upgradeTracker[upgrade];
    }

    public void SyncStats()
    {
        statsManager.AttackStrength = baseStats.AttackStrength * (1f + upgradeTracker["Adrenaline"] / 10f); // level 10 = doubled
        statsManager.RollCooldown = baseStats.RollCooldown - (0.05f * upgradeTracker["Instinct"]); // level 10 = halved
        statsManager.BaseSpeed = baseStats.BaseSpeed * (1f + upgradeTracker["Instinct"] / 20f); // level 10 = 1.5x
        statsManager.AttackCooldown = baseStats.AttackCooldown * (1f - upgradeTracker["Instinct"] / 20f); // level 10 = halved (crazy)
        statsManager.MaxHealth = baseStats.MaxHealth + (10 * upgradeTracker["Vital"]); // level 10 = +100
        statsManager.MaxStamina = baseStats.MaxStamina + (10 * upgradeTracker["Harmony"]);

        currentStatsDisplay.text = $"health : {statsManager.MaxHealth}\n" +
                                   $"stamina : {statsManager.MaxStamina}\n" +
                                   $"speed : {statsManager.BaseSpeed / baseStats.BaseSpeed:F2}x\n" +
                                   $"attack : {statsManager.AttackStrength / baseStats.AttackStrength:F2}x\n" +
                                   $"atk spd : {statsManager.AttackCooldown / baseStats.AttackCooldown:F2}x\n" +
                                   $"roll spd : {statsManager.RollCooldown / baseStats.RollCooldown:F2}x";

    }

    public void ShowEffect(string upgrade)
    {
        if (upgrade == "hide") { upgradeEffectDisplay.text = ""; }
        switch (upgrade)
        {
            case "Adrenaline":
                upgradeEffectDisplay.text = "\n\n\n   +0. 1x\n\n\n";
                break;
            case "Instinct":
                upgradeEffectDisplay.text = "\n\n +0. 1x\n\n   -0. 05x\n     -0. 1x";
                break;
            case "Vital":
                upgradeEffectDisplay.text = "+10\n\n\n\n\n\n";
                break;
            case "Harmony":
                upgradeEffectDisplay.text = "\n  +10\n\n\n\n\n";
                break;
        }
    }
    public void ShowEffect() // hide
    {
        upgradeEffectDisplay.text = "";
    }
}

