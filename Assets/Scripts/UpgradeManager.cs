using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private int currency;
    [SerializeField] private TextMeshProUGUI currencyDisplay;
    private Dictionary<string, int> upgradeTracker = new Dictionary<string, int>();

    [SerializeField] private PlayerStats statsManager;
    // set in editor, should affect CurrentStats object
    [SerializeField] private PlayerStats baseStats;
    // set in editor, should be BaseStats object

    private readonly string[] upgradeList = { "Instinct", "Adrenaline", "Vital", "Harmony" };
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Initializing UpgradeManager");
        currency = DataManager.Instance.currency;
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
    }

    public void PurchaseUpgrade(string upgrade)
    {
        Debug.Assert(upgradeTracker.ContainsKey(upgrade));
        Debug.Log("Purchasing upgrade: " + upgrade);
        if (currency - CalculateCost(upgrade) >= 0)
        {
            currency -= CalculateCost(upgrade);
        }
        else
        {
            Debug.Log("No more money :(");
            // broadcast event no money lolsies
        }
        upgradeTracker[upgrade] += 1;
        UpdateText();
        SyncStats();
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
        
    }
}

