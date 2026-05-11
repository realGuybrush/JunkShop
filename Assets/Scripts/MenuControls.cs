using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;
    
    [SerializeField]
    private PlayerInput playerInput;

    private InputAction escape;

    private void Awake()
    {
        escape = playerInput.actions.FindAction("Escape");
        escape.performed += Escape;
    }

    private void OnDestroy()
    {
        escape.performed -= Escape;
    }

    private void Escape(InputAction.CallbackContext callbackContext)
    {
        menu.SetActive(!menu.activeSelf);
        Time.timeScale = menu.activeSelf ? 0 : 1;
    }

    public void RestoreTime()
    {
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        RestoreTime();
        SceneManager.LoadScene("MainMenuScene");
    }
}
