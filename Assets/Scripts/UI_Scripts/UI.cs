using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour, ISaveManager
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
    public GameObject inGameUi;
    [SerializeField] private CanvasGroup inGameUICanvasGroup;
    [SerializeField] private UI_VolumeSlider[] volumeSettings;
    [SerializeField] private MapCameraController mapCameraController;
    private GameObject[] menus;
    private int currentMenuIndex = 0;

    public UI_ItemTooltip itemTooltip;
    public UI_StatToolTip statTooltip;
    public UI_SkillToolTip skillToolTip;
    public UI_CraftWindow craftWindow;
    public static bool isInputBlocked = false;

    private void Awake() 
    {
        SwitchTo(skillTreeUI); //Need this to assign events on skillTree slots before we assign event on skill scripts.
        fadeScreen.gameObject.SetActive(true);
    }

    void Start()
    {
        menus = new GameObject[] { characterUI, skillTreeUI, craftUI, optionUI };
        SwitchTo(inGameUi);

        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInputBlocked || PlayerManager.instance.player.isCutScene) return;

        if(Input.GetKeyDown(KeyCode.Tab))
            SwitchWithKeyTo(characterUI);
        if(Input.GetKeyDown(KeyCode.M))
            SwitchWithKeyTo(craftUI);
        if(Input.GetKeyDown(KeyCode.O))
            SwitchWithKeyTo(optionUI);
        if(Input.GetKeyDown(KeyCode.I))
            SwitchWithKeyTo(skillTreeUI);
        if(Input.GetKeyDown(KeyCode.Escape))
            HandleEscapeKey();

        if (IsAnyUIOpen())
        {
            if (Input.GetKeyDown(KeyCode.Q))
                SwitchMenu(-1);
            else if (Input.GetKeyDown(KeyCode.E))
                SwitchMenu(1);
        }
    }

    bool IsAnyUIOpen()
    {
        return characterUI.activeSelf || skillTreeUI.activeSelf || craftUI.activeSelf || optionUI.activeSelf;
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
            AudioManager.instance.PlayUISFX(0);
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

        CheckForInGameUI();
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            AudioManager.instance.PlayUISFX(0);
            //Debug.Log($"Deactivating {_menu.name}");
            _menu.SetActive(false);
            CheckForInGameUI();
            if (_menu == craftUI && mapCameraController != null)
                mapCameraController.ResetCamera();
            
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
            if (child != inGameUi && child.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
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

        GameManager.instance.PauseGame(isAnyMenuActive);
    }

    private void HandleEscapeKey()
    {
        bool isAnyMenuClosed = false;
        // Close all active menus except the in-game UI
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child != inGameUi && child.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
            {
                AudioManager.instance.PlayUISFX(3);
                child.SetActive(false);
                isAnyMenuClosed = true;
                if (child == craftUI && mapCameraController != null)
                    mapCameraController.ResetCamera();
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

    //Switch menu with keyboard
    private void SwitchMenu(int direction)
    {
        currentMenuIndex += direction;

        if (currentMenuIndex < 0)
            currentMenuIndex = menus.Length - 1;
        else if (currentMenuIndex >= menus.Length)
            currentMenuIndex = 0;

        SwitchTo(menus[currentMenuIndex]);
    }

    IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(1f);
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();

    public void LoadData(GameData _data)
    {
       foreach(KeyValuePair<string, float> pair in _data.volumeSettings)
       {
        foreach(UI_VolumeSlider item in volumeSettings)
        {
            if(item.parameter == pair.Key)
                item.LoadSlider(pair.Value);
        }
       }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach (UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parameter, item.slider.value);
        }
    }
}
