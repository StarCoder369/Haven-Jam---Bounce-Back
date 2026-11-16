using UnityEngine;

public class UpgradeChoose : MonoBehaviour
{
    public UpgradeOption[] options;

    public GameObject[] upgradePanel;

    public GameObject panel;

    public void ResetPanels()
    {
        panel.SetActive(true);
        foreach (GameObject panel in upgradePanel)
        {
            panel.GetComponent<UpgradePanel>().option = options[Random.Range(0, options.Length)];
            panel.GetComponent<UpgradePanel>().UpdateText();
        }
    }
}
