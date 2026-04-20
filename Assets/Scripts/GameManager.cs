using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI powerText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI livesText;
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;

    [Header("Coins")]
    [SerializeField] private int coins = 0;
    public int Coins => coins;

    [Header("Shop")]
    public int lifeCostCoins = 5;
    public int livesPerPurchase = 1;

    public int ballCostCoins = 5;
    public GameObject ballPrefab;
    public Transform ballSpawnPoint;

    private void Awake()
    {
        Instance = this;
        UpdateCoinUI();
        UpdateLivesUI(null);
    }

    public void UpdatePowerUI(float power)
    {
        powerText.text = $"Power: {power}";
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        coins += amount;
        UpdateCoinUI();
    }

    public bool TrySpendCoins(int amount)
    {
        if (amount <= 0) return true;
        if (coins < amount) return false;

        coins -= amount;
        UpdateCoinUI();
        return true;
    }

    public void UpdateCoinUI()
    {
        if (coinText == null) return;
        coinText.text = $"Coins: {coins}";
    }

    public void UpdateLivesUI(Ball ball)
    {
        if (livesText == null) return;

        if (ball == null)
        {
            livesText.text = "Lives: -";
            return;
        }

        livesText.text = $"Lives: {ball.CurrentLives}/{ball.maxLives}";
    }

    // UI Button?? ????: ???????? ??? ????
    public void BuyLives()
    {
        Ball ball = FindObjectOfType<Ball>();
        if (ball == null) return;
        if (ball.CurrentLives >= ball.maxLives) return;

        if (!TrySpendCoins(lifeCostCoins)) return;

        // ?????? ??? ???????, ?? ?????? ???? ?????? ???? ???? ?????? ??????? ???
        if (!ball.TryAddLives(livesPerPurchase))
        {
            AddCoins(lifeCostCoins);
        }

        UpdateLivesUI(ball);
    }

    // UI Button에 연결: 코인으로 Ball 1개 구매(생성)
    public void BuyBall()
    {
        if (ballPrefab == null) return;
        if (!TrySpendCoins(ballCostCoins)) return;

        Vector3 pos = ballSpawnPoint != null ? ballSpawnPoint.position : Vector3.zero;
        Quaternion rot = ballSpawnPoint != null ? ballSpawnPoint.rotation : Quaternion.identity;

        Instantiate(ballPrefab, pos, rot);
    }


    // ???? ????? Trigger?? ?????? ???
    public void EndSession(bool isWin)
    {
        Time.timeScale = 0f; // ???? ???????
        resultPanel.SetActive(true);
        resultText.text = isWin ? "Gate Destroyed!" : "Ball Lost...";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}