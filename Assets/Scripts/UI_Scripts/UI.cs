using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_ItemTooltip itemTooltip;
    public UI_StatToolTip statTooltip;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchTo(GameObject _menu)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if(_menu != null)
            _menu.SetActive(true);
    }
}
