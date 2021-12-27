using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    public static UIController instance;
    public GameObject TapToStartPanel, LoosePanel, GamePanel, WinPanel;
    public Text npcCountText, gamePlayScoreText, winScreenScoreText, levelNoText, tapToStartScoreText;



    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        StartUI();
    }

    public void StartUI()
    {
        TapToStartPanel.SetActive(true);
        LoosePanel.SetActive(false);
        GamePanel.SetActive(false);
        tapToStartScoreText.text = GameManager.instance.totalScore.ToString();
    }

    public void SetLevelText(int levelNo)
    {
        levelNoText.text = "Level " + levelNo.ToString();
    }

    // TAPTOSTART TU?UNA BASILDI?INDA  --- G?R?? EKRANINDA VE LEVEL BA?LARINDA
    public void TapToStartButtonClick()
    {
        GameManager.instance.TakeNPCsFirstPositions();
        GameManager.instance.isContinue = true;
        //PlayerController.instance.SetArmForGaming();
        TapToStartPanel.SetActive(false);
        GamePanel.SetActive(true);
        PlayerController.instance.PlayerWalkAnim();
        SetLevelText(LevelController.instance.totalLevelNo);

    }

    // RESTART TU?UNA BASILDI?INDA  --- LOOSE EKRANINDA
    public void RestartButtonClick()
    {
        GamePanel.SetActive(false);
        LoosePanel.SetActive(false);
        TapToStartPanel.SetActive(true);
        LevelController.instance.RestartLevelEvents();
        tapToStartScoreText.text = GameManager.instance.totalScore.ToString();
    }


    // NEXT LEVEL TU?UNA BASILDI?INDA  --- W?N EKRANINDA
    public void NextLevelButtonClick()
    {
        TapToStartPanel.SetActive(true);
        WinPanel.SetActive(false);
        GamePanel.SetActive(false);
        LevelController.instance.NextLevelEvents();
        tapToStartScoreText.text = GameManager.instance.totalScore.ToString();
    }

    public void SetNpcCountText(int count, int maxCount)
    {
        npcCountText.text = count + "/" + maxCount;
    }

    public void SetScoreText()
    {
        gamePlayScoreText.text = GameManager.instance.totalScore.ToString();
    }

    public void WinScreenScore()
    {
        winScreenScoreText.text = GameManager.instance.score.ToString();
    }

    public void ActivateWinScreen()
    {
        GamePanel.SetActive(false);
        WinPanel.SetActive(true);
        WinScreenScore();
    }

    public void ActivateLooseScreen()
    {
        GamePanel.SetActive(false);
        LoosePanel.SetActive(true);
    }

    public void ActivateGameScreen()
    {
        GamePanel.SetActive(true);
        TapToStartPanel.SetActive(false);
    }

    public void ActivateTapToStartScreen()
    {
        TapToStartPanel.SetActive(false);
        WinPanel.SetActive(false);
        LoosePanel.SetActive(false);

    }
}
