using UnityEngine;

[System.Serializable]
public class ObjetoEstado
{
    public string id;
    public Vector3 posicao;
    public Quaternion rotacao;
    public bool isActivate = true;
    public bool dobleJump = false;
}
