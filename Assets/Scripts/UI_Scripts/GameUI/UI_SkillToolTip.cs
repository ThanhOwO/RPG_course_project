using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;

    public override void ShowTooltip(params object[] args)
    {
        if (args.Length < 3 || args[0] is not string _skillDescription || args[1] is not string _skillName || args[2] is not int _price)
            return;

        skillName.text = _skillName;
        skillText.text = _skillDescription;
        skillCost.text = "Cost: " + _price;
        gameObject.SetActive(true);
    }
}
