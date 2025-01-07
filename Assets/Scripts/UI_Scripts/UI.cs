using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject playerHealthUI;
    public UI_ItemTooltip itemTooltip;
    public UI_StatToolTip statTooltip;
    public UI_SkillToolTip skillToolTip;
    public UI_CraftWindow craftWindow;

    private void Awake() 
    {
        SwitchTo(skillTreeUI); //Need this to assign events on skillTree slots before we assign event on skill scripts.
    }

    void Start()
    {
        SwitchTo(null);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
            SwitchWithKeyTo(characterUI);
        if(Input.GetKeyDown(KeyCode.C))
            SwitchWithKeyTo(craftUI);
        if(Input.GetKeyDown(KeyCode.Escape))
            SwitchWithKeyTo(optionUI);
        if(Input.GetKeyDown(KeyCode.I))
            SwitchWithKeyTo(skillTreeUI);
    }

    public void SwitchTo(GameObject _menu)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

            if(child != playerHealthUI)
                child.SetActive(false);
        }

        if(_menu != null)
            _menu.SetActive(true);
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if(_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            return;
        }

        SwitchTo(_menu);
    }
}
