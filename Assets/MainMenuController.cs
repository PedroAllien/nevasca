using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Menu")]
    public GameObject canvasMenu;

    [Header("Prologue")]
    public GameObject canvasPrologue;

    [Header("Credits")]
    public GameObject canvasCredits;

    public void PlayProloue()
    {
        canvasMenu.gameObject.SetActive(false);
        canvasPrologue.gameObject.SetActive(true);
    }

    public void activatedCredits()
    {
        canvasMenu.gameObject.SetActive(false);
        canvasCredits.gameObject.SetActive(true);
    }

    public void exitCredits()
    {
        canvasMenu.gameObject.SetActive(true);
        canvasCredits.gameObject.SetActive(false);
    }

    public void PlayGame()
    {
        GameController.Instance.StartGame();
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("O jogo foi encerrado!");
    }
}
