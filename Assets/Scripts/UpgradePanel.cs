using TMPro;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    public UpgradeOption option;

    public TMP_Text titleTxt;

    public TMP_Text subtitleTxt;

    public void UpdateText()
    {
        titleTxt.text = option.title;
        subtitleTxt.text = option.subtitle;
    }
}
