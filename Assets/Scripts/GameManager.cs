using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isContinue; // oyun zamaný karakterin ilerlemesi v.s.

    
    [HideInInspector]public List<GameObject> disabledObjects = new List<GameObject>();
    private List<GameObject> npcs = new List<GameObject>();
    private List<Vector3> npcsFirtsPosition = new List<Vector3>();

    [HideInInspector]
    public int score, totalScore;


	private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

	private void Start()
	{
        ClearLists();
        TakeNPCsFirstPositions();
	}



	public void TakeNPCsFirstPositions()
	{
        GameObject [] Tempnpcs = GameObject.FindGameObjectsWithTag("npc");
		for (int i = 0; i < Tempnpcs.Length; i++)
		{
            npcs.Add(Tempnpcs[i]);
            npcsFirtsPosition.Add(Tempnpcs[i].transform.position);
		}
         
	}

    public void KillAllNpcs()
	{
        for (int i = 0; i < npcs.Count; i++)
        {
            Destroy(npcs[i].gameObject);

        }
    }

    //  restart eventte npclerin yerlerini tekrar ayarlarkene...
    public void SetNCSsPositionAgain()
	{
		for (int i = 0; i < npcs.Count; i++)
		{
            npcs[i].transform.position = npcsFirtsPosition[i];
            npcs[i].transform.rotation = Quaternion.Euler(0,180,0);
            npcs[i].GetComponent<Collider>().enabled = true;
		}
	}

    // restart evennte kapýlarýn engellerin v.s. yeniden açýlmasý
    public void ActivateAllDisabledObjects()
	{
		for (int i = 0; i < disabledObjects.Count; i++)
		{
            disabledObjects[i].SetActive(true);    
            disabledObjects[i].GetComponent<Collider>().enabled = true;
            if (!disabledObjects[i].transform.CompareTag("final"))
			{
                disabledObjects[i].GetComponent<Renderer>().enabled = true;
            }
		}
	}

    // bir level bitip diðerine giderken bunlar çaðrýlmadan önce temizlenecek...
    public void ClearLists()
	{
        disabledObjects.Clear();
        npcs.Clear();
        npcsFirtsPosition.Clear();
	}


    public void FinalScoreMultiply(string str)
    {
        if (str == "x10") score *= 10;
        else if (str == "x9") score *= 9;
        else if (str == "x8") score *= 8;
        else if (str == "x7") score *= 7;
        else if (str == "x6") score *= 6;
        else if (str == "x5") score *= 5;
        else if (str == "x4") score *= 4;
        else if (str == "x3") score *= 3;
        else if (str == "x2") score *= 2;
        UIController.instance.SetScoreText();
    }

}
