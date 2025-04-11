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
    // set in editor

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
        return;
    }

    private int CalculateCost(string upgrade)
    {
        return 1 + upgradeTracker[upgrade];
    }

    public void SyncStats()
    {
        statsManager.AttackStrength += statsManager.AttackStrength * (1f + upgradeTracker["Adrenaline"] / 10f);
        statsManager.RollCooldown -= 0.05f * upgradeTracker["Instinct"];
        statsManager.BaseSpeed += statsManager.BaseSpeed * (1f + upgradeTracker["Instinct"] / 20f);
        statsManager.AttackCooldown += statsManager.AttackCooldown * (1f - upgradeTracker["Instinct"] / 20f);
        statsManager.MaxHealth += 10 * upgradeTracker["Vital"];
        // stamina
        Debug.LogError("TODO() IMPLEMENT STAMINA STATS & UPGRADE");
    }
}

