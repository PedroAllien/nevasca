using UnityEngine;
using UnityEngine.SceneManagement;

public class StateGerenciator : MonoBehaviour
{
    public string id;
    public bool isActive;
    public bool dobleJump;

    void Awake()
    {
        id = this.gameObject.name + "scene" + SceneManager.GetActiveScene().buildIndex.ToString();
        var estado = GameController.Instance.ObterEstado(id);
        if (estado != null)
        {
            transform.position = estado.posicao;
            transform.rotation = estado.rotacao;
            this.gameObject.SetActive(estado.isActivate);
            dobleJump = estado.dobleJump;
        }
        isActive = this.gameObject.activeInHierarchy;
    }
    void OnDestroy()
    {
        GameController.Instance.SalvarEstado(id, transform.position, transform.rotation, isActive, dobleJump);
    }
}
