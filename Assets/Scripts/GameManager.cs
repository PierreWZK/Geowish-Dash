using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_Text PourcentageText;
    public TMP_Text TryCountText;
    public UnityEngine.UI.RawImage Win;
    public TMP_Text TextMenu;

    private int _tryCount = 1;
    private float _pourcentage = 0;
    private bool _cheatCodeWin = false;
    private bool _cheatCode = false;

    private void Awake()
    {
        // Si jamais on charge une 2e scene
        // avec un autre GameManager
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start ()
    {
        // Au bout de 3 secondes, on cache le texte "TryCountText"
        StartCoroutine(HideTryCountTextAfterDelay(3f));
        
        PlayerMovementBehavior pmb = FindObjectOfType<PlayerMovementBehavior>();
    }

    void Update ()
    {
        UpdatePourcentage();

        // Cheat code
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            _cheatCodeWin = !_cheatCodeWin;
            if (_cheatCodeWin)
            {
                _pourcentage = 10000;
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            _cheatCode = !_cheatCode;
        }
        if (_cheatCode)
        {
            CheatCodeJump();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            RestartGame();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            ChooseLevel(2);
        }
    }

    public void NewTry()
    {
        TryCountText.text = $"ATTEMPT : {++_tryCount}";
        _pourcentage = 0;
    }

    // Update all seconds the pourcentage of the player progression
    // 60s => 100%
    public void UpdatePourcentage()
    {
        _pourcentage += Time.deltaTime * 400; // Pourcentage incrémenté en fonction du temps
        if (_pourcentage > 10000)
        {
            WinLevel();
        }
        if (_pourcentage <= 10001)
            PourcentageText.text = $"{_pourcentage/100:0.00}%";
    }

    IEnumerator HideTryCountTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        TryCountText.gameObject.SetActive(false);
    }

    public void PlayerDie()
    {
        Debug.Log("Player die !");
        PlayerMovementBehavior pmb = FindObjectOfType<PlayerMovementBehavior>();
        // On redémarre la game (en tp le joueur au début du niveau (0, 0, 0))
        pmb.transform.position = Vector3.zero;

        // Incrementer le nombre d'essais
        NewTry();

        // Remettre le pourcentage à 0
        _pourcentage = 0;

        // Afficher pendant 3 secondes "ATTEMPT : x"
        TryCountText.gameObject.SetActive(true);
        StartCoroutine(HideGameOverAfterDelay(5f));
    }

    public void CheatCodeJump()
    {
        PlayerMovementBehavior _pmb = FindObjectOfType<PlayerMovementBehavior>();
        _pmb.setIsGrounded(true);
    }

    IEnumerator HideGameOverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        TryCountText.gameObject.SetActive(false);
    }

    public void WinLevel() 
    {
        Debug.Log("Player win !");
        Win.gameObject.SetActive(true);
        TextMenu.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        // Recharge la scene actuelle
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void ChooseLevel(int numLevel)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level"+numLevel);
    }
}
