using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public TextMeshProUGUI endberrytxt;
    public TextMeshProUGUI endfirewoodtxt;

    void Start()
    {
        endberrytxt.text = GameController.Instance.berrys.ToString() + 'X';
        endfirewoodtxt.text = GameController.Instance.firewoods.ToString() + 'X';
    }
    
    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }
}
