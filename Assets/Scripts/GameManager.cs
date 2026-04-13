using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text powerText;
    public GameObject resultPanel;
    public Text resultText;

    private void Awake() => Instance = this;

    public void UpdatePowerUI(float power)
    {
        powerText.text = $"Power: {power}";
    }

    // 공이 바닥의 Trigger에 닿으면 호출
    public void EndSession(bool isWin)
    {
        Time.timeScale = 0f; // 게임 일시정지
        resultPanel.SetActive(true);
        resultText.text = isWin ? "Gate Destroyed!" : "Ball Lost...";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}