using UnityEngine;

public class NextFase : MonoBehaviour
{
    private GameController gameController;
    public int nextFase = 0;
    public Vector3 position;
    public Quaternion rotation;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void GoToNextFase(bool dobleJump)
    {
        GameController.Instance.SalvarEstado("Player" + "scene" + nextFase.ToString(), position, rotation, true, dobleJump);
        gameController.LoadScene(nextFase);
    }
}
