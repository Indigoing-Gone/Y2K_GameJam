using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject creditPanel;
    
    public void StartGame() => SceneManager.LoadScene("MainScene");

    public void ToggleCredits(bool _creditsStatus) => creditPanel.SetActive(_creditsStatus);
}
