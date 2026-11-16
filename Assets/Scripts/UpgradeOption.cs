using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeOption", menuName = "Game Data/Upgrade Option")]
public class UpgradeOption : ScriptableObject
{
    public string title;
    public string subtitle;

    [System.Serializable]
    public enum UpgradeOptions
    {
        Damage,
        Speed,
        Knockback,
        ShieldAmt
    }

    [System.Serializable]
    public enum UpgradeAmountType
    {
        Add,
        Subtract
    }

    public UpgradeOptions upgradeType;
    public UpgradeAmountType upgradeSymbol;
    public float increaseAmt;
}
