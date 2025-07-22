using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;
using System.Threading.Tasks;

public class ChangeInput : MonoBehaviour
{
    EventSystem system;
    public Selectable firstSelected;

    // Get the child game objects of the login panel
    [Header("Input Fields")]
    public TMP_InputField inputUsername;
    public TMP_InputField inputPassword;

    [Header("Buttons")]
    public Button buttonSubmit;
    public Button buttonQuit;

    [Header("Text Fields")]
    public TMP_Text textFeedback;

    void Start()
    {
        system = EventSystem.current;
        firstSelected.Select();

        // Configure the input buttons
        buttonSubmit.onClick.AddListener(OnLoginButtonClick);
        buttonQuit.onClick.AddListener(OnQuitButtonClick);

        // Subscribe to AuthManager events
        AuthManager.Instance.OnLoginSuccess += OnLoginSuccessUI;
        AuthManager.Instance.OnLoginError += OnLoginErrorUI;
    }

    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame && Keyboard.current.leftShiftKey.isPressed)
        {
            Selectable previous = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            if (previous != null)
            {
                previous.Select();
            }
        }
        else if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {
                next.Select();
            }
        }

        else if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.numpadEnterKey.wasPressedThisFrame)
        {
            buttonSubmit.onClick.Invoke();
        }
    }



    // This method is called when the user clicks the Login button
    public async void OnLoginButtonClick()
    {
        // Disable and make the button invisible
        buttonSubmit.interactable = false;
        buttonSubmit.gameObject.SetActive(false);
        textFeedback.text = "Logging in...";
        // Get the username and password from the input fields
        string username = inputUsername.text;
        string password = inputPassword.text;

        // Check if the username and password are not empty
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Username and password cannot be empty");
            return;
        }

        // Call the login method from AuthManager
        await AuthManager.Instance.Login(username, password);
    }

    void OnQuitButtonClick()
    {
        Application.Quit();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; //Stop the game in the editor
        #endif
    }
    
    void OnLoginSuccessUI()
    {
        textFeedback.text = "¡Login correcto! Cargando...";
        // Cambia de escena, por ejemplo:
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_World");
    }

    void OnLoginErrorUI(string error)
    {
        // Re-enable the button and make it visible
        buttonSubmit.interactable = true;
        buttonSubmit.gameObject.SetActive(true);
        // Show the error message in the text field
        textFeedback.text = $"Error de login";
    }
}
