using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.TextCore.Text;
using Unity.Cinemachine;

public class GameController : Singleton<GameController>
{
    private float freezeGuys = 0;
    public float berrys = 0;
    public float firewoods = 0;
    private Animator animator;
    public TextMeshProUGUI berrytxt;
    public TextMeshProUGUI firewoodtxt;
    public List<ObjetoEstado> estadosSalvos = new List<ObjetoEstado>();
    [Header("Tempo do player")]
    public TextMeshProUGUI timeText;
    private float timeCount = 480f;
    private float totalTime = 480f;
    private bool isTimerRunning = false;
    [Header("Light2D")]
    private Light2D globalLight;
    [Header("Congelamento")]
    public Image congelamentoImage;
    private float congelamentoValue = 0f;
    private float congelamentoRate = 0.02f;
    private float descongelamentoRate = 0.5f;
    [Header("GameOver")]
    public Canvas canvasGameOver;
    [Header("Pause")]
    public Canvas canvasPause;
    public bool isPaused = false;
    public GameObject freezePlayer;
    public GameObject bag;
    private CinemachineImpulseSource impulseSource;
    [Header("HUD")]
    public GameObject colect;
    public GameObject coldConfig;


    void Start()
    {
        DontDestroyOnLoad(this);
        globalLight = GetComponentInChildren<Light2D>();
        animator = GetComponentInChildren<Animator>();
        instantiateFreezePlayer();

        canvasGameOver.gameObject.SetActive(false);
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        Timer();
        AumentarCongelamento();
        Pause(); 
    }

    public void Timer()
    {
        if (isTimerRunning)
        {
            if (timeCount > 0)
            {
                timeCount -= Time.deltaTime;
                UpdateTimeText();
                UpdateLightIntensity();
            }
            else
            {
                timeCount = 0;
                isTimerRunning = false;
                ShowGameOverScreen();
            }
        }
    }

    public void ShowGameOverScreen()
    {
        canvasGameOver.gameObject.SetActive(true);

        Time.timeScale = 0;
    }

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            coldConfig.gameObject.SetActive(false);
            colect.gameObject.SetActive(false);
            Time.timeScale = 0f;
            canvasPause.gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            canvasPause.gameObject.SetActive(false);
            coldConfig.gameObject.SetActive(true);
            colect.gameObject.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        TogglePause();
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        coldConfig.gameObject.SetActive(true);
        colect.gameObject.SetActive(true);
        LoadScene(1);
        StartTimer();
        resetColectables();
    }

    public void StartEndGame()
    {
        canvasPause.gameObject.SetActive(false);
        colect.gameObject.SetActive(false);
        coldConfig.gameObject.SetActive(false);
        LoadScene(11);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;

        ResetVariables();
        resetColectables();
        StartTimer();

        SceneManager.LoadScene("1");
    }

    public void ExitMenu()
    {
        ResumeGame();
        ResetVariables();

        canvasPause.gameObject.SetActive(false);
        canvasGameOver.gameObject.SetActive(false);

        SceneManager.LoadScene("Menu");
    }

    public void resetColectables()
    {
        foreach (var item in estadosSalvos)
        {
            item.isActivate = true;
        }
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    private void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(timeCount / 60F);
        int seconds = Mathf.FloorToInt(timeCount % 60F);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateLightIntensity()
    {
        if (globalLight != null)
        {
            float proportion = (timeCount / totalTime);
            globalLight.intensity = proportion;
        }
    }

    public void GetBerrys(float berry)
    {
        berrys+= berry;
        berrytxt.text = berrys.ToString() + "/15";
    }

    public void GetFirewood(float firewood)
    {
        firewoods+= firewood;
        firewoodtxt.text = firewoods.ToString() + "/15";
    }

    public void SalvarEstado(string id, Vector3 posicao, Quaternion rotacao, bool ativo, bool dobleJump)
    {
        var estadoExistente = ObterEstado(id);
        if (estadoExistente != null)
        {
            estadoExistente.posicao = posicao;
            estadoExistente.rotacao = rotacao;
            estadoExistente.isActivate = ativo;
            estadoExistente.dobleJump = dobleJump;
        }
        else
        {
            estadosSalvos.Add(new ObjetoEstado { id = id, posicao = posicao, rotacao = rotacao, isActivate = ativo, dobleJump = dobleJump });
        }
    }

    public void instantiateFreezePlayer()
    {
        var freezeCharacters = estadosSalvos.FindAll(e => e.id.Contains("IcedCharacter") && e.id.Contains("scene"+SceneManager.GetActiveScene().buildIndex.ToString()));
        if (freezeCharacters != null && freezeCharacters.Count > 0)
        {
            freezeGuys = 0;
            foreach (var character in freezeCharacters)
            {
                freezeGuys++;
                freezePlayer.name = "IcedCharacter" + freezeGuys.ToString();
                GameObject newObject = Instantiate(freezePlayer, character.posicao, character.rotacao);
            }
        }
    }

    public ObjetoEstado ObterEstado(string id)
    {
        return estadosSalvos.Find(e => e.id == id);
    }

    public void LoadScene(int buildIndex)
    {
        StartCoroutine(Fade(buildIndex));
    }

    IEnumerator Fade(int buildIndex)
    {
        animator.SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(buildIndex);
        yield return new WaitForSeconds(0.1f);
        instantiateFreezePlayer();
    }

    public void finalizingGame()
    {
        StartEndGame();
    }

    public void AdicionarFrio(float valor)
    {
        congelamentoValue += valor;
        CameraShakeManager.instance.CameraShake(impulseSource);
    }

    public void AumentarCongelamento()
    {
        if (congelamentoValue < 1f)
        {
            congelamentoValue += congelamentoRate * Time.deltaTime;
            congelamentoImage.fillAmount = congelamentoValue;
        } else
        {
           StartCoroutine(dieFromFreeze());
        }
    }

    public IEnumerator dieFromFreeze()
    {
        congelamentoValue = 0f;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player>().Speed = 0f;
        if (freezePlayer != null && player != null)
        {
            Vector3 spawnPosition = player.transform.position;
            Quaternion spawnRotation = player.transform.localRotation;
            player.SetActive(false);
            freezeGuys++;
            freezePlayer.name = "IcedCharacter" + freezeGuys.ToString();
            Instantiate(freezePlayer, spawnPosition, spawnRotation);
            if (GameObject.FindAnyObjectByType<Bag>() != null)
            {
                GameObject.FindAnyObjectByType<Bag>().retrieveItens();
                yield return new WaitForSeconds(0.35f);
            }
            spawnPosition.y += 1f;
            Instantiate(bag, spawnPosition, spawnRotation);
        }
        SalvarEstado("Player" + "scene1", new Vector3(0, 0, 0), player.transform.rotation, true, player.GetComponent<Player>().dobleJump);
        yield return new WaitForSeconds(0.2f);
        this.LoadScene(1);
        yield return new WaitForSeconds(0.1f);
        berrys = 0;
        firewoods = 0;
        berrytxt.text = "0/15";
        firewoodtxt.text = "0/15";
    }

    public void ReduzirCongelamento()
    {
        if (congelamentoValue > 0f)
        {
            congelamentoValue -= descongelamentoRate * Time.deltaTime;
            congelamentoImage.fillAmount = congelamentoValue;
        }
    }

    private void ResetVariables()
    {
        freezeGuys = 0;
        berrys = 0;
        firewoods = 0;
        congelamentoValue = 0f;
        timeCount = totalTime;
        isTimerRunning = false;

        berrytxt.text = berrys.ToString() + "/15";
        firewoodtxt.text = firewoods.ToString() + "/15";
        UpdateTimeText();

        congelamentoImage.fillAmount = congelamentoValue;

        if (globalLight != null)
        {
            globalLight.intensity = 1f;
        }

        estadosSalvos.Clear();
        canvasGameOver.gameObject.SetActive(false);
    }
}