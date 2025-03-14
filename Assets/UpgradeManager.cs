using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

enum Upgrade
{
    Instinct,
    Adrenaline,
    Vitality,
    Harmony
}

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private uint currency;
    [SerializeField] private TextMeshProUGUI currencyDisplay;
    private Dictionary<Upgrade, uint> upgradeTracker;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Initializing UpgradeManager");
        currency = DataManager.Instance.currency;
        UpdateText();
    }

    void UpdateText()
    {
        currencyDisplay.text = $"{currency}";
    }

    void PurchaseUpgrade(Upgrade upgrade)
    {
        currency -= CalculateCost(upgrade, upgradeTracker[upgrade]);
        upgradeTracker[upgrade] += 1;
        return;
    }

    uint CalculateCost(Upgrade upgrade, uint level)
    {
        return 10;
    }
}
