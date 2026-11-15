using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeOption", menuName = "Game Data/Upgrade Option")]
public class UpgradeOption : ScriptableObject
{
    public string upgradeName;
    public string subtitile;

    [System.Serializable]
    public enum UpgradeOptions
    {
        Damage,
        Speed,
        Knockback,
        Cooldown,
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
