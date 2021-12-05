using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isContinue; // oyun zamaný karakterin ilerlemesi v.s.

    public List<GameObject> disabledObjects = new List<GameObject>();
    private List<GameObject> npcs = new List<GameObject>();
    private List<Vector3> npcsFirtsPosition = new List<Vector3>();

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
        npcs.Clear();
        GameObject [] Tempnpcs = GameObject.FindGameObjectsWithTag("npc");
		for (int i = 0; i < Tempnpcs.Length; i++)
		{
            npcs.Add(Tempnpcs[i]);
            npcsFirtsPosition.Add(Tempnpcs[i].transform.position);
		}
         
	}


    //  restart eventte npclerin yerlerini tekrar ayarlarkene...
    public void SetNCSsPositionAgain()
	{
		for (int i = 0; i < npcs.Count; i++)
		{
            npcs[i].transform.position = npcsFirtsPosition[i];
		}
	}

    // restart evennte kapýlarýn engellerin v.s. yeniden açýlmasý
    public void ActivateAllDisabledObjects()
	{
		for (int i = 0; i < disabledObjects.Count; i++)
		{
            disabledObjects[i].SetActive(true);
		}
	}

    // bir level bitip diðerine giderken bunlar çaðrýlmadan önce temizlenecek...
    public void ClearLists()
	{
        disabledObjects.Clear();
        npcs.Clear();
        npcsFirtsPosition.Clear();
	}



}
