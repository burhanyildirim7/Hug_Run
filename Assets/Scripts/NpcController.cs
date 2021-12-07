using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NpcController : MonoBehaviour
{
	public static NpcController instance;

    public List<Vector3> npcPositions = new List<Vector3>();

    public int armValue= 0;

    public SkinnedMeshRenderer skRenderer;
	public GameObject arms;
	public GameObject player;
	private Vector3 firstNpcFinalPosition;
	private float npcFinalYPosition = -1f;
	private float npcHeight = 1.6f;
	private int npcCount = 0;
	private int maxNpcCount = 1;
	private int maxTotalNpc = 60;


	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this);
	}

	private void Start()
	{
		DOTween.Init();
	}

	private void OnTriggerEnter(Collider other)
	{
		
		if (other.CompareTag("npc"))
		{

			// npc sayýsýný artýracaðýz.. ve içeriye npc atacaðýz...
			if(npcCount < maxNpcCount)
			{
				other.GetComponent<Collider>().enabled = false;
				other.transform.SetParent(arms.transform);
				other.transform.localPosition = npcPositions[npcCount];
				other.transform.rotation = Quaternion.Euler(0,0,0);
				other.GetComponent<Animator>().SetTrigger("walk");
				npcCount++;
				UIController.instance.SetNpcCountText(npcCount, maxNpcCount);
			}
			SetArmValue();
			GameManager.instance.score = npcCount * 10;
			UIController.instance.SetScoreText();
		}
		else if (other.CompareTag("obstacle"))
		{
			// npc sayýsýný azaltýp o sayý kadar npc yi dýþarý atacaz.. kollar da ufalabilir...
			GameManager.instance.disabledObjects.Add(other.gameObject);
			other.gameObject.SetActive(false);

			int fallNpcCount = Random.Range(1,3);
			if(fallNpcCount == 1 && npcCount >= 1)
			{
				FallNpc(arms.transform.GetChild(npcCount - 1).gameObject);
				npcCount -= 1;
			}
			else if(fallNpcCount == 2 && npcCount >= 2)
			{
				FallNpc(arms.transform.GetChild(npcCount - 1).gameObject);
				FallNpc(arms.transform.GetChild(npcCount - 2).gameObject);
				npcCount -= 2;
			}
			else if(npcCount == 1)
			{
				FallNpc(arms.transform.GetChild(npcCount - 1).gameObject);
				npcCount -= 1;
			}
			UIController.instance.SetNpcCountText(npcCount, maxNpcCount);
			SetArmValue();
			GameManager.instance.score = npcCount * 10;
			UIController.instance.SetScoreText();
		}
		else if (other.CompareTag("final"))
		{
			GameManager.instance.disabledObjects.Add(other.gameObject);
			other.GetComponent<Collider>().enabled = false;
			GameManager.instance.isContinue = false;
			if (npcCount > 0)
			{				
				StartCoroutine(NpcFinalArray());
				PlayerController.instance.PlayerIdleAnim();
				PlayerController.instance.SetArmForStart();
				CameraController.instance.SetCameraFinalOffset();
			}
			else
			{
				PlayerController.instance.PlayerIdleAnim();
				UIController.instance.ActivateLooseScreen();
			}
			
		}
	
	}

	private void OnTriggerExit(Collider other)
	{
		 if (other.CompareTag("+3"))
		{
			PositiveDoor(other.gameObject, 3);
		}
		else if (other.CompareTag("+5"))
		{
			PositiveDoor(other.gameObject, 5);
		}
		else if (other.CompareTag("+10"))
		{
			PositiveDoor(other.gameObject,10);
		}
		else if (other.CompareTag("-3"))
		{
			NegativeDoor(other.gameObject, 3);
		}
		else if (other.CompareTag("-5"))
		{
			NegativeDoor(other.gameObject, 5);
		}
		else if (other.CompareTag("-10"))
		{
			NegativeDoor(other.gameObject, 10);
		}
		if (maxNpcCount > maxTotalNpc) maxNpcCount = maxTotalNpc;
		UIController.instance.SetNpcCountText(npcCount, maxNpcCount);
	}


	private void PositiveDoor(GameObject obj , int value)
	{
		GameManager.instance.disabledObjects.Add(obj.gameObject);
		obj.gameObject.SetActive(false);
		maxNpcCount += value;
		SetArmValue();
	}

	private void NegativeDoor(GameObject obj , int value)
	{
		GameManager.instance.disabledObjects.Add(obj.gameObject);
		obj.gameObject.SetActive(false);
		int tempMax = maxNpcCount;
		maxNpcCount -= value;
		bool isLastThrow = false;
		if (value >= tempMax && npcCount != 0) isLastThrow = true;
		if (maxNpcCount <= 0) maxNpcCount = 1;
		if (npcCount > maxNpcCount)
		{
			while (npcCount != maxNpcCount)
			{
				FallNpc(arms.transform.GetChild(npcCount - 1).gameObject);
				npcCount--;
				if (npcCount == 0) break;
			}
		}
		if (isLastThrow)
		{
			FallNpc(arms.transform.GetChild(0).gameObject);
			maxNpcCount = 1;
			npcCount = 0;
		}
		SetArmValue();
		UIController.instance.SetNpcCountText(npcCount, maxNpcCount);
	}

	private void SetArmValue()
	{
		float keyValue = 7 + npcCount*100/65;
		skRenderer.SetBlendShapeWeight(0,keyValue);
		SetCollider();
	}


	private void SetCollider()
	{
		if (armValue < 20)
		{
			GetComponent<SphereCollider>().radius = .4f;
			GetComponent<SphereCollider>().center = new Vector3(0,1,.28f);
		}
		else if (armValue >= 20 && armValue<40)
		{
			GetComponent<SphereCollider>().radius = .6f;
			GetComponent<SphereCollider>().center = new Vector3(0, 1, .5f);
		}
		else if (armValue >= 40 && armValue < 60)
		{
			GetComponent<SphereCollider>().radius = .65f;
			GetComponent<SphereCollider>().center = new Vector3(0, 1, 1f);
		}
		else if (armValue >= 60 && armValue < 80)
		{
			GetComponent<SphereCollider>().radius = .8f;
			GetComponent<SphereCollider>().center = new Vector3(0, 1, 1.3f);
		}
		else if (armValue >= 80 )
		{
			GetComponent<SphereCollider>().radius = 1f;
			GetComponent<SphereCollider>().center = new Vector3(0, 1, 1.8f);
		}

	}

	private void FallNpc(GameObject obj)
	{
		obj.transform.tag = "fNpc";
		obj.GetComponent<Collider>().enabled = true;
		obj.GetComponent<Animator>().SetTrigger("fall");
		obj.transform.SetParent(null);
		StartCoroutine(FallNpcThrow(obj));
	}

	IEnumerator FallNpcThrow(GameObject obj)
	{
		int frame = 0;
		float distanceX;
		if(transform.position.x >= 0) distanceX = Random.Range(-.03f, -.025f);
		else distanceX = Random.Range(.025f,.03f);

		//float distanceX = Random.Range(-.02f,.02f);
		float distanceZ = Random.Range(.03f,.06f);
		while (frame <= 100)
		{
			obj.transform.position = new Vector3(obj.transform.position.x + distanceX,obj.transform.position.y, obj.transform.position.z + distanceZ);
			yield return new WaitForEndOfFrame();
			frame++;
			if (frame == 50)
			{
				obj.GetComponent<Collider>().enabled = true;
				obj.transform.tag = "npc";
			}
		}	
	}

	IEnumerator NpcFinalArray()
	{
		float stackPointZ = GameObject.Find("StackPoint").transform.position.z;
		int count = arms.transform.childCount;
		GameObject[] objects = new GameObject[count];
		for (int i = 0; i < count; i++)
		{
			objects[i] = arms.transform.GetChild(i).gameObject;
		}
		for (int i = 0; i < count; i++)
		{
			objects[i].GetComponent<Animator>().SetTrigger("idle");
			objects[i].transform.tag = "fNpc";
			objects[i].transform.SetParent(null);
		}
		yield return new WaitForSeconds(.5f);
		for (int i = 0; i < count; i++)
		{		
			npcFinalYPosition += npcHeight;
			firstNpcFinalPosition = new Vector3(transform.position.x, npcFinalYPosition, stackPointZ);
			objects[i].transform.DOMove(firstNpcFinalPosition, 1);
			yield return new WaitForSeconds(.1f);
			objects[i].transform.rotation = Quaternion.Euler(0, 180, 0);
			npcCount--;
			UIController.instance.SetNpcCountText(npcCount,maxNpcCount);
		}
		GetComponent<Collider>().enabled = false;
		npcFinalYPosition += npcHeight;
		firstNpcFinalPosition = new Vector3(transform.position.x, npcFinalYPosition, stackPointZ);
		transform.DOMove(firstNpcFinalPosition, 1);
		transform.rotation = Quaternion.Euler(0,180,0);
		PlayerController.instance.PlayerClapAnim();
		CameraController.instance.SetCameraFinalInverse();
		yield return new WaitForSeconds(1.1f);
		GetComponent<Collider>().enabled = true;
		
	}


	internal void StartingEvents()
	{
		npcFinalYPosition = -1f;
		skRenderer.SetBlendShapeWeight(0, 0);
		npcCount = 0;
		maxNpcCount = 1;
		UIController.instance.SetNpcCountText(npcCount, maxNpcCount);
	}


}
