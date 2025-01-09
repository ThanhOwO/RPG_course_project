using TMPro;
using UnityEngine;

public class UI_StatToolTip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI description;

    public override void ShowTooltip(params object[] args)
    {
        if(args.Length == 0 || args[0] is not string _text || string.IsNullOrEmpty(_text))
            return; 
        
        description.text = _text;

        gameObject.SetActive(true);
    }

}
