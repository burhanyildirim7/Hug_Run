using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isContinue; // oyun zamaný karakterin ilerlemesi v.s.

    public List<GameObject> disabledObjects = new List<GameObject>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

	private void Start()
	{
        isContinue = true;
	}


}
