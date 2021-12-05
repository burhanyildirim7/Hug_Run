using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NpcController : MonoBehaviour
{
    public List<Vector3> npcPositions = new List<Vector3>();

    public int armValue= 0;

    public SkinnedMeshRenderer skRenderer;
	public GameObject arms;
	public GameObject player;
	private Vector3 firsNpcFinalPosition;
	private float npcFinalYPosition = -1f;
	private float npcHeight = 1.6f;
	private int npcCount = 0;
	private int maxNpcCount = 1;
	private int maxTotalNpc = 60;


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
		}
		else if (other.CompareTag("final"))
		{
			GameManager.instance.isContinue = false;
			StartCoroutine(NpcFinalArray());
			PlayerController.instance.PlayerIdleAnim();
			PlayerController.instance.SetArmForStart();
			CameraController.instance.SetCameraFinalOffset();
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
		maxNpcCount -= value;
		bool isLastThrow = false;
		if (value >= maxNpcCount) isLastThrow = true;
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

	public IEnumerator MakeBlendShape(int value)
	{
		if (value > 0)
		{
			while ((int)skRenderer.GetBlendShapeWeight(0) < value)
			{
				float tempValue = skRenderer.GetBlendShapeWeight(0) + .5f;
				skRenderer.SetBlendShapeWeight(0, tempValue);
				yield return new WaitForSeconds(0.02f);
			}
		}
		else
		{
			while ((int)skRenderer.GetBlendShapeWeight(0) > value)
			{
				float tempValue = skRenderer.GetBlendShapeWeight(0) - .5f;
				skRenderer.SetBlendShapeWeight(0, tempValue);
				yield return new WaitForSeconds(0.02f);
			}
		}	
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
		float distanceX = Random.Range(-.02f,.02f);
		float distanceZ = Random.Range(.05f,.1f);
		while (frame <= 120)
		{
			obj.transform.position = new Vector3(obj.transform.position.x + distanceX,obj.transform.position.y, obj.transform.position.z + distanceZ);
			yield return new WaitForSeconds(.01f);
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
		int count = arms.transform.childCount;
		GameObject[] objects = new GameObject[arms.transform.childCount];
		for (int i = 0; i < arms.transform.childCount; i++)
		{
			objects[i] = arms.transform.GetChild(i).gameObject;
		}
		for (int i = 0; i < count; i++)
		{
			objects[i].GetComponent<Animator>().SetTrigger("idle");
			objects[i].transform.tag = "fNpc";
			objects[i].transform.SetParent(null);
		}
		for (int i = 0; i < count; i++)
		{		
			npcFinalYPosition += npcHeight;
			firsNpcFinalPosition = new Vector3(arms.transform.position.x, npcFinalYPosition, arms.transform.position.z + 10);
			objects[i].transform.DOMove(firsNpcFinalPosition, 1);
			yield return new WaitForSeconds(.1f);
			objects[i].transform.rotation = Quaternion.Euler(0, 180, 0);
			npcCount--;
			UIController.instance.SetNpcCountText(npcCount,maxNpcCount);
		}
		npcFinalYPosition += npcHeight;
		firsNpcFinalPosition = new Vector3(arms.transform.position.x, npcFinalYPosition, arms.transform.position.z + 10);
		player.transform.DOMove(firsNpcFinalPosition, 1);
		player.transform.rotation = Quaternion.Euler(0,180,0);
		PlayerController.instance.PlayerClapAnim();
		CameraController.instance.SetCameraFinalInverse();
	}


}
