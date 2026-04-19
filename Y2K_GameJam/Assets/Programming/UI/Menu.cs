using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject creditPanel;
    
    public void StartGame() => SceneManager.LoadScene("MainScene");

    public void ToggleTutorial(bool _creditsStatus) => tutorialPanel.SetActive(_creditsStatus);

    public void ToggleCredits(bool _creditsStatus) => creditPanel.SetActive(_creditsStatus);

}
