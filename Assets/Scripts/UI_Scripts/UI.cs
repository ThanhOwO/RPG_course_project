using System.Collections;
using UnityEngine;

public class UI : MonoBehaviour
{
    [Header("End Screen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]
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
        fadeScreen.gameObject.SetActive(true);
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
        if(Input.GetKeyDown(KeyCode.O))
            SwitchWithKeyTo(optionUI);
        if(Input.GetKeyDown(KeyCode.I))
            SwitchWithKeyTo(skillTreeUI);
        if(Input.GetKeyDown(KeyCode.Escape))
            HandleEscapeKey();
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null; //Need this to keep fadeScreen gameobject active
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
                if(fadeScreen == false)
                    child.SetActive(false); // Fallback for non-CanvasGroup objects
            }
        }

        if (_menu != null)
        {
            CanvasGroup menuCanvasGroup = _menu.GetComponent<CanvasGroup>();
            if (menuCanvasGroup != null)
            {
                //Debug.Log($"Show menu: {_menu.name}");
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
            //Debug.Log("Showing InGameUI as fallback.");
            inGameUICanvasGroup.alpha = 1;
            inGameUICanvasGroup.interactable = true;
            inGameUICanvasGroup.blocksRaycasts = true;
        }
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            //Debug.Log($"Deactivating {_menu.name}");
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        //Debug.Log($"Switching to {_menu.name}");
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
                //Debug.Log("Hiding InGameUI");
                inGameUICanvasGroup.alpha = 0;
                inGameUICanvasGroup.interactable = false;
                inGameUICanvasGroup.blocksRaycasts = false;
            }
            else
            {
                //Debug.Log("Showing InGameUI");
                inGameUICanvasGroup.alpha = 1;
                inGameUICanvasGroup.interactable = true;
                inGameUICanvasGroup.blocksRaycasts = true;
            }
        }
    }

    private void HandleEscapeKey()
    {
        bool isAnyMenuClosed = false;
        // Close all active menus except the in-game UI
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child != inGameUi && child.activeSelf)
            {
                child.SetActive(false);
                isAnyMenuClosed = true;
            }
        }
        // If any menu was closed, ensure the in-game UI is shown
        if (isAnyMenuClosed)
            SwitchTo(inGameUi);

    }

    public void SwitchOnEndScreen()
    {
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCoroutine());
    }

    IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(1f);
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();
    
}
