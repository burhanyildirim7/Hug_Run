using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public List<Vector3> npcPositions = new List<Vector3>();

    public int armValue= 0;

    public SkinnedMeshRenderer skRenderer;
	public GameObject arms;

	private int npcCount = 0;
	private int maxNpcCount = 1;


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

			
		}
		else if (other.CompareTag("obstacle"))
		{
			// npc sayýsýný azaltýp o sayý kadar npc yi dýþarý atacaz.. kollar da ufalabilir...
			GameManager.instance.disabledObjects.Add(other.gameObject);
			other.gameObject.SetActive(false);

			int fallNpcCount = Random.Range(1,3);
			if(fallNpcCount == 1 && npcCount >= 1)
			{
				GameObject fallingNpc = arms.transform.GetChild(npcCount-1).gameObject;
				fallingNpc.transform.tag = "fNpc";
				fallingNpc.GetComponent<Collider>().enabled = true;
				fallingNpc.GetComponent<Animator>().SetTrigger("fall");
				fallingNpc.transform.SetParent(null);
				StartCoroutine(FallNpcThrow(fallingNpc));
				npcCount -= 1;
			}
			else if(fallNpcCount == 2 && npcCount >= 2)
			{
				GameObject fallingNpc1 = arms.transform.GetChild(npcCount-1).gameObject;
				fallingNpc1.transform.tag = "fNpc";
				fallingNpc1.GetComponent<Collider>().enabled = true;
				fallingNpc1.GetComponent<Animator>().SetTrigger("fall");
				fallingNpc1.transform.SetParent(null);
				StartCoroutine(FallNpcThrow(fallingNpc1));

				GameObject fallingNpc2 = arms.transform.GetChild(npcCount-2).gameObject;
				fallingNpc2.transform.tag = "fNpc";
				fallingNpc2.GetComponent<Collider>().enabled = true;
				fallingNpc2.GetComponent<Animator>().SetTrigger("fall");
				fallingNpc2.transform.SetParent(null);
				StartCoroutine(FallNpcThrow(fallingNpc2));
				npcCount -= 2;
			}
			else if(npcCount == 1)
			{
				GameObject fallingNpc = arms.transform.GetChild(npcCount - 1).gameObject;
				fallingNpc.transform.tag = "fNpc";
				fallingNpc.GetComponent<Collider>().enabled = true;
				fallingNpc.GetComponent<Animator>().SetTrigger("fall");
				fallingNpc.transform.SetParent(null);
				StartCoroutine(FallNpcThrow(fallingNpc));
				npcCount -= 1;
			}
			UIController.instance.SetNpcCountText(npcCount, maxNpcCount);
		}
		


	}

	private void OnTriggerExit(Collider other)
	{
		 if (other.CompareTag("+3"))
		{
			GameManager.instance.disabledObjects.Add(other.gameObject);
			other.gameObject.SetActive(false);
			maxNpcCount += 3;
			SetArmValue(3);
		}
		else if (other.CompareTag("+5"))
		{
			GameManager.instance.disabledObjects.Add(other.gameObject);
			other.gameObject.SetActive(false);
			maxNpcCount += 5;
			SetArmValue(5);
		}
		else if (other.CompareTag("+10"))
		{
			GameManager.instance.disabledObjects.Add(other.gameObject);
			other.gameObject.SetActive(false);
			maxNpcCount += 10;
			SetArmValue(10);
		}
		else if (other.CompareTag("-3"))
		{
			GameManager.instance.disabledObjects.Add(other.gameObject);
			other.gameObject.SetActive(false);
			maxNpcCount -= 3;
			SetArmValue(-3);
		}
		else if (other.CompareTag("-5"))
		{
			GameManager.instance.disabledObjects.Add(other.gameObject);
			other.gameObject.SetActive(false);
			maxNpcCount -= 5;
			SetArmValue(-5);
		}
		else if (other.CompareTag("-10"))
		{
			GameManager.instance.disabledObjects.Add(other.gameObject);
			other.gameObject.SetActive(false);
			maxNpcCount -= 10;
			SetArmValue(-10);
		}
		if (maxNpcCount > 34) maxNpcCount = 34;
		UIController.instance.SetNpcCountText(npcCount, maxNpcCount);
	}

	private void SetArmValue(int value)
	{
		if(value == 3)
		{
			if(armValue == 0 && armValue != 100)
			{
				armValue = 20;
				StartCoroutine(MakeBlendShape(armValue));
			}
			else if(armValue != 100)
			{
				armValue += 15;
				StartCoroutine(MakeBlendShape(armValue));
			}
		}
		else if(value == 5)
		{
			if (armValue == 0 && armValue != 100)
			{
				armValue = 20;
				StartCoroutine(MakeBlendShape(armValue));
			}
			else if (armValue != 100)
			{
				armValue += 20;
				StartCoroutine(MakeBlendShape(armValue));
			}
		}
		else if(value == 10)
		{
			if (armValue == 0 && armValue != 100)
			{
				armValue = 40;
				StartCoroutine(MakeBlendShape(armValue));
			}
			else if (armValue != 100)
			{
				armValue += 30;
				StartCoroutine(MakeBlendShape(armValue));
			}
		}
		if (value == -3)
		{
			if (armValue > 20)
			{
				armValue -= 15;
				StartCoroutine(MakeBlendShape(armValue));
			}else if(armValue <= 20)
			{
				armValue = 0;
				StartCoroutine(MakeBlendShape(armValue));
			}

		}
		else if (value == -5)
		{
			if (armValue > 25)
			{
				armValue -= 20;
				StartCoroutine(MakeBlendShape(armValue));
			}
			else if (armValue <= 25)
			{
				armValue = 0;
				StartCoroutine(MakeBlendShape(armValue));
			}
		}
		else if (value == -10)
		{
			if (armValue > 40)
			{
				armValue -= 30;
				StartCoroutine(MakeBlendShape(armValue));
			}
			else if (armValue <= 40)
			{
				armValue = 0;
				StartCoroutine(MakeBlendShape(armValue));
			}
		}
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


}
