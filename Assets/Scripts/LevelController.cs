using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElephantSDK;

public class LevelController : MonoBehaviour
{

	public static LevelController instance;
	public int levelNo, tempLevelNo, totalLevelNo; // totallevelno tüm leveller bitip random gelmeye baþlayýnca kullanýlýyor
	public List<GameObject> levels = new List<GameObject>();
	private GameObject currentLevelObj;

	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this);
	}

	private void Start()
	{
		PlayerPrefs.DeleteAll();
		totalLevelNo = PlayerPrefs.GetInt("level");
		if (totalLevelNo == 0)
		{
			totalLevelNo = 1;
			levelNo = 1;
		}
		//UIController.instance.SetLevelText(totalLevelNo);
		LevelStartingEvents();
	}

	public void IncreaseLevelNo()
	{
		tempLevelNo = levelNo;
		totalLevelNo++;
		PlayerPrefs.SetInt("level", totalLevelNo);
		//UIController.instance.SetLevelText(totalLevelNo);
	}

	// Bu fonksiyon oyun ilk a??ld???nda ?a?r?lacak..
	public void LevelStartingEvents()
	{
		if (totalLevelNo > levels.Count)
		{
			levelNo = Random.Range(1, levels.Count + 1);
			if (levelNo == tempLevelNo) levelNo = Random.Range(1, levels.Count + 1);
		}
		else
		{
			levelNo = totalLevelNo;
		}
		UIController.instance.SetLevelText(totalLevelNo);
		currentLevelObj = Instantiate(levels[levelNo - 1], Vector3.zero, Quaternion.identity);
		Elephant.LevelStarted(totalLevelNo);
		UIController.instance.npcCountText.color = Color.white;

	}

	// next level tu?una bas?ld???nda UIManager scriptinden ?a?r?lacak..
	public void NextLevelEvents()
	{
		NpcController.instance.StartingEvents();
		Elephant.LevelCompleted(totalLevelNo);
		GameManager.instance.KillAllNpcs();
		Destroy(currentLevelObj);
		IncreaseLevelNo();
		LevelStartingEvents();
		GameManager.instance.ClearLists();
		PlayerController.instance.PlayerStartPosition();
		UIController.instance.npcCountText.color = Color.white;
		CameraController.instance.SetCameraStartOffset();
	}

	// restart level tu?una bas?ld???nda UIManager scriptinden ?a?r?lacak..
	public void RestartLevelEvents()
	{
		Elephant.LevelFailed(totalLevelNo);
		GameManager.instance.SetNCSsPositionAgain();
		GameManager.instance.ActivateAllDisabledObjects();
		PlayerController.instance.PlayerStartPosition();
		UIController.instance.npcCountText.color = Color.white;

	}
}
