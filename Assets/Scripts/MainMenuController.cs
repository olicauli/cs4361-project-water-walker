using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class MainMenu : MonoBehaviour
{
    private UIDocument _doc;
    private Button _playButton;
    private Button _quitButton;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        _playButton = _doc.rootVisualElement.Q<Button>("PlayButton");
        _quitButton = _doc.rootVisualElement.Q<Button>("QuitButton");

        _playButton.clicked += PlayGame;
        _quitButton.clicked += QuitGame;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1); // loads the outdoor scene
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
