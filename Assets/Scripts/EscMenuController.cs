using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EscMenuController : MonoBehaviour
{
    private UIDocument _doc;
    private Button _returnButton;
    private Button _quitButton;
    public bool menuActive = false;
    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        setButtons(_doc);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            menuActive = !menuActive;
            VisualElement _root = _doc.rootVisualElement.Q<VisualElement>("Root");
            _root.visible = menuActive;
        }
    }

    private void setButtons(UIDocument _doc)
    {
        _returnButton = _doc.rootVisualElement.Q<Button>("ReturnToMenuButton");
        _quitButton = _doc.rootVisualElement.Q<Button>("QuitButton");
        _returnButton.clicked += ReturnToMenu;
        _quitButton.clicked += QuitGame;
    }

    public void ReturnToMenu() 
    {
        SceneManager.LoadScene(0); // loads the main menu
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
