using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElephantSDK;

public class LevelController : MonoBehaviour
{

	public static LevelController instance;
	public int levelNo, tempLevelNo, totalLevelNo;
	public List<GameObject> levels = new List<GameObject>();
	private GameObject currentLevelObj;

	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this.gameObject);
	}

	private void Start()
	{
		//PlayerPrefs.DeleteAll();
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
		//UIController.instance.levelNoText.text = "Level " + totalLevelNo.ToString();
		currentLevelObj = Instantiate(levels[levelNo - 1], Vector3.zero, Quaternion.identity);
		Elephant.LevelStarted(totalLevelNo);
	}

	// next level tu?una bas?ld???nda UIManager scriptinden ?a?r?lacak..
	public void NextLevelEvents()
	{
		Elephant.LevelCompleted(totalLevelNo);
		Destroy(currentLevelObj);
		IncreaseLevelNo();
		LevelStartingEvents();
	}

	// restart level tu?una bas?ld???nda UIManager scriptinden ?a?r?lacak..
	public void RestartLevelEvents()
	{
		Elephant.LevelFailed(totalLevelNo);
		// DEAKT?F ED?LEN OBSTACLELARIN TEKRAR A?ILMASI ???N..
		//GameObject[] obstacles;
		//obstacles = GameObject.FindGameObjectsWithTag("obstacle");
		//for (int i = 0; i < obstacles.Length; i++)
		//{
		//	obstacles[i].GetComponent<MeshRenderer>().enabled = true;
		//}
		GameObject[] collectibles;
		collectibles = GameObject.FindGameObjectsWithTag("collectible");
		for (int i = 0; i < collectibles.Length; i++)
		{
			collectibles[i].GetComponent<MeshRenderer>().enabled = true;
		}
	}
}
