using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject inGameUi;
    [SerializeField] private CanvasGroup inGameUICanvasGroup;
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
        SwitchTo(inGameUi);

        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
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
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

            CanvasGroup canvasGroup = child.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                // Hide all CanvasGroups, including InGameUI
                canvasGroup.alpha = 0; // Hide
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            else
            {
                child.SetActive(false); // Fallback for non-CanvasGroup objects
            }
        }

        if (_menu != null)
        {
            CanvasGroup menuCanvasGroup = _menu.GetComponent<CanvasGroup>();
            if (menuCanvasGroup != null)
            {
                Debug.Log($"Show menu: {_menu.name}");
                menuCanvasGroup.alpha = 1; // Show
                menuCanvasGroup.interactable = true;
                menuCanvasGroup.blocksRaycasts = true;
            }
            else
            {
                _menu.SetActive(true); // Fallback for non-CanvasGroup objects
            }
        }

        // If no menu is passed, show the InGameUI
        if (_menu == null && inGameUICanvasGroup != null)
        {
            Debug.Log("Showing InGameUI as fallback.");
            inGameUICanvasGroup.alpha = 1;
            inGameUICanvasGroup.interactable = true;
            inGameUICanvasGroup.blocksRaycasts = true;
        }
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            Debug.Log($"Deactivating {_menu.name}");
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        Debug.Log($"Switching to {_menu.name}");
        SwitchTo(_menu);
    }


    private void CheckForInGameUI()
    {
        bool isAnyMenuActive = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child != inGameUi && child.activeSelf)
            {
                isAnyMenuActive = true;
                break;
            }
        }

        if (inGameUICanvasGroup != null)
        {
            if (isAnyMenuActive)
            {
                Debug.Log("Hiding InGameUI");
                inGameUICanvasGroup.alpha = 0;
                inGameUICanvasGroup.interactable = false;
                inGameUICanvasGroup.blocksRaycasts = false;
            }
            else
            {
                Debug.Log("Showing InGameUI");
                inGameUICanvasGroup.alpha = 1;
                inGameUICanvasGroup.interactable = true;
                inGameUICanvasGroup.blocksRaycasts = true;
            }
        }
    }

    
}
