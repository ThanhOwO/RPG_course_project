using UnityEngine;
using UnityEngine.UI;

public class SavePoint_UI : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject teleportMapPanel;
    [SerializeField] private Player player;
    [SerializeField] private Button[] buttons;
    private bool isTeleporting = false;
    private int selectedIndex = 0;
    private InteractTrigger interactTrigger;
    public bool isOpen = false;
    private float inputCooldown = 0.2f;
    private float lastInputTime = 0f;

    private void Start()
    {
        interactTrigger = GetComponent<InteractTrigger>();
        HighlightButton(selectedIndex);
    }

    void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
            CloseAll();

        if (!isOpen || isTeleporting) return;
        
        NavigateButtons();
    }


    public void OpenMenu()
    {
        isOpen = true;
        UI.isInputBlocked = true;
        MapCameraController.isTeleportMap = true;
        CameraLookAround.isMoving = true;
        menuPanel.SetActive(true);
        teleportMapPanel.SetActive(false);
        selectedIndex = 0;
        HighlightButton(selectedIndex);
    }

    public void OpenTeleportMap()
    {
        isTeleporting = true;
        menuPanel.SetActive(false);
        teleportMapPanel.SetActive(true);
        Time.timeScale = 0; 
    }

    public void CloseAll()
    {
        isOpen = false;
        isTeleporting = false;
        UI.isInputBlocked = false;
        MapCameraController.isTeleportMap = false;
        CameraLookAround.isMoving = false;
        menuPanel.SetActive(false);
        teleportMapPanel.SetActive(false);
        Time.timeScale = 1;

        if (interactTrigger != null)
            interactTrigger.ResetInteraction();
        
        player.stateMachine.ChangeState(player.idleState);
    }

    private void NavigateButtons()
    {
        float verticalInput = Input.GetAxisRaw("UIVertical");

        if (Time.time - lastInputTime > inputCooldown)
        {
            if (verticalInput > 0)
            {
                selectedIndex = Mathf.Max(0, selectedIndex - 1);
                HighlightButton(selectedIndex);
                lastInputTime = Time.time;
            }
            else if (verticalInput < 0)
            {
                selectedIndex = Mathf.Min(buttons.Length - 1, selectedIndex + 1);
                HighlightButton(selectedIndex);
                lastInputTime = Time.time;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttons[selectedIndex].onClick.Invoke();
        }
    }

    private void HighlightButton(int index)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Image buttonImg = buttons[i].GetComponent<Image>();
            if (buttonImg != null)
            {
                Color32 color = buttonImg.color;
                color.a = (i == index) ? (byte)255 : (byte)36;
                buttonImg.color = color;
            }
        }
    }

}
