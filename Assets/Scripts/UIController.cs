using UnityEngine;
using TMPro;


public class UIController : MonoBehaviour
{
	public static UIController instance;
	public GameObject TapToStartPanel,LoosePanel,GamePanel,WinPanel;
	public TextMeshProUGUI npcCountText;
	public Animator playerAnimator;
	public Renderer playerArmForStart, playerArmForGaming;





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
	}

	//public void SetLevelText(int levelNo)
	//{
	//	levelNoText.text = "Level " + levelNo.ToString();
	//}

	// TAPTOSTART TU?UNA BASILDI?INDA  --- G?R?? EKRANINDA VE LEVEL BA?LARINDA
	public void TapToStartButtonClick()
	{
		GameManager.instance.isContinue = true;
		PlayerController.instance.SetArmForGaming();
		TapToStartPanel.SetActive(false);
		GamePanel.SetActive(true);
		playerAnimator.SetTrigger("walk");

	}

	// RESTART TU?UNA BASILDI?INDA  --- LOOSE EKRANINDA
	public void RestartButtonClick()
	{
		TapToStartPanel.SetActive(true);
		LoosePanel.SetActive(false);
		//LevelController.instance.RestartLevelEvents();
	}


	// NEXT LEVEL TU?UNA BASILDI?INDA  --- W?N EKRANINDA
	public void NextLevelButtonClick()
	{		
		TapToStartPanel.SetActive(true);
		WinPanel.SetActive(false);
		GamePanel.SetActive(false);
		//LevelController.instance.NextLevelEvents();
	}

	public void SetNpcCountText(int count, int maxCount)
	{
		npcCountText.text = count + "/" + maxCount;
	}

	//public void SetScoreText()
	//{
	//	scoreText.text =GameManager.instance.score.ToString();
	//}

	//public void SetGemsText()
	//{
	//	gemsText.text =GameManager.instance.gems.ToString();
	//}

	//public void SetTotalScoreText()
	//{
	//	totalScoreTextStartPanel.text = PlayerPrefs.GetInt("totalscore").ToString();
	//	totalScoreTextGamePanel.text = PlayerPrefs.GetInt("totalscore").ToString();
	//}

	//public void SetTotalGemsText()
	//{
	//	totalGemsTextStartPanel.text =PlayerPrefs.GetInt("totalgems").ToString();
	//	totalGemsTextGamePanel.text = PlayerPrefs.GetInt("totalgems").ToString();
	//}


}
