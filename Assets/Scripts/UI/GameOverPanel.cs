using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public GameObject Board;
    public TextMeshProUGUI WinnerText;
    public GameAudio GameAudio;
    private string color;

    private Animator gameOverPanelAnimator;

    private void Awake()
    {
        gameOverPanelAnimator = GetComponent<Animator>();
    }

    public void SetWinnerText(PawnColor winnerPawnColor)
    {
        // WinnerText.text = winnerPawnColor.ToString().ToUpper() + " ПОБЕДИЛИ";
        color = winnerPawnColor.ToString().ToUpper() == "WHITE" ? "БЕЛЫЕ" : "ЧЕРНЫЕ";
        WinnerText.text = color + " ПОБЕДИЛИ";
    }

    public void DisableBoard()
    {
        Board.SetActive(false);
    }

    public void ReturnToMenu()
    {
        gameOverPanelAnimator.SetTrigger("ReturnToMenu");
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void FadeGameMusic()
    {
        GameAudio.FadeGameMusic();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}