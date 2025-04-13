using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private int currency;
    [SerializeField] private TextMeshProUGUI currencyDisplay;
    private Dictionary<string, int> upgradeTracker = new Dictionary<string, int>();

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

    int CalculateCost(string upgrade)
    {
        return 1 + upgradeTracker[upgrade];
    }
}
