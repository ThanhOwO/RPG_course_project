using UnityEngine;

public class SavePoint_UI : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject teleportMapPanel;
    [SerializeField] private Player player;

    private InteractTrigger interactTrigger;
    public bool isOpen = false;

    private void Start()
    {
        interactTrigger = GetComponent<InteractTrigger>();
    }

    void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
            CloseAll();
    }


    public void OpenMenu()
    {
        isOpen = true;
        UI.isInputBlocked = true;
        menuPanel.SetActive(true);
        teleportMapPanel.SetActive(false);
    }

    public void OpenTeleportMap()
    {
        menuPanel.SetActive(false);
        teleportMapPanel.SetActive(true);
        Time.timeScale = 0; 
    }

    public void CloseAll()
    {
        isOpen = false;
        UI.isInputBlocked = false;
        menuPanel.SetActive(false);
        teleportMapPanel.SetActive(false);
        Time.timeScale = 1;

        if (interactTrigger != null)
            interactTrigger.ResetInteraction();
        
        player.stateMachine.ChangeState(player.idleState);
    }

}
