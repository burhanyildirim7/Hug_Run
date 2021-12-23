using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
//using TMPro;

public class NpcController : MonoBehaviour
{
    public static NpcController instance;

    public List<Vector3> npcPositions = new List<Vector3>();

    public int armValue = 0;

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

            // npc say?s?n? art?raca??z.. ve i?eriye npc ataca??z...
            if (npcCount < maxNpcCount)
            {
                other.GetComponent<Collider>().enabled = false;
                other.transform.SetParent(arms.transform);
                other.transform.localPosition = npcPositions[npcCount];
                other.transform.rotation = Quaternion.Euler(0, 0, 0);
                other.GetComponent<Animator>().SetTrigger("walk");
                npcCount++;
                UIController.instance.SetNpcCountText(npcCount, maxNpcCount);
            }
            if (npcCount >= 1) PlayerController.instance.SetArmForGaming();
            SetArmValue();
            GameManager.instance.score = npcCount * 10;
            UIController.instance.SetScoreText();
            if (npcCount == maxNpcCount) PlayerController.instance.NpcCountTextAnim();

        }
        else if (other.CompareTag("obstacle"))
        {
            // npc say?s?n? azalt?p o say? kadar npc yi d??ar? atacaz.. kollar da ufalabilir...
            GameManager.instance.disabledObjects.Add(other.gameObject);
            //other.gameObject.SetActive(false);

            int fallNpcCount = Random.Range(1, 3);
            if (fallNpcCount == 1 && npcCount >= 1)
            {
                FallNpc(arms.transform.GetChild(npcCount - 1).gameObject);
                npcCount -= 1;
            }
            else if (fallNpcCount == 2 && npcCount >= 2)
            {
                FallNpc(arms.transform.GetChild(npcCount - 1).gameObject);
                FallNpc(arms.transform.GetChild(npcCount - 2).gameObject);
                npcCount -= 2;
            }
            else if (npcCount == 1)
            {
                FallNpc(arms.transform.GetChild(npcCount - 1).gameObject);
                npcCount -= 1;
            }
            if (npcCount == 0) PlayerController.instance.SetArmForStart();
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
        int count = other.transform.childCount;

        if (other.CompareTag("door"))
        {
            if (other.GetComponent<DoorScript>()._kapiDeger > 0)
            {
                PositiveDoor(other.transform.parent.transform.parent.gameObject, other.GetComponent<DoorScript>()._kapiDeger);
            }
            else if (other.GetComponent<DoorScript>()._kapiDeger < 0)
            {
                NegativeDoor(other.transform.parent.transform.parent.gameObject, -(other.GetComponent<DoorScript>()._kapiDeger));
            }

            if (maxNpcCount > maxTotalNpc) maxNpcCount = maxTotalNpc;
            UIController.instance.SetNpcCountText(npcCount, maxNpcCount);
        }

    }


    private void PositiveDoor(GameObject obj, int value)
    {
        GameManager.instance.disabledObjects.Add(obj);
        obj.SetActive(false);
        maxNpcCount += value;
        SetArmValue();
    }

    private void NegativeDoor(GameObject obj, int value)
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
        if (npcCount == 0) PlayerController.instance.SetArmForStart();
        SetArmValue();
        UIController.instance.SetNpcCountText(npcCount, maxNpcCount);
    }

    private void SetArmValue()
    {
        float keyValue;// = 7 + npcCount*100/65;
        if (maxNpcCount < 4) keyValue = 5;
        else if (maxNpcCount >= 4 && maxNpcCount < 6)
        {
            keyValue = 15;
        }
        else if (maxNpcCount >= 6 && maxNpcCount < 9)
        {
            keyValue = 20;
        }
        else if (maxNpcCount >= 9 && maxNpcCount < 11)
        {
            keyValue = 30;
        }
        else if (maxNpcCount >= 11 && maxNpcCount < 13)
        {
            keyValue = 35;
        }
        else if (maxNpcCount >= 13 && maxNpcCount < 17)
        {
            keyValue = 40;
        }
        else if (maxNpcCount >= 17 && maxNpcCount < 22)
        {
            keyValue = 45;
        }
        else if (maxNpcCount >= 22 && maxNpcCount < 29)
        {
            keyValue = 50;
        }
        else if (maxNpcCount >= 29 && maxNpcCount < 32)
        {
            keyValue = 55;
        }
        else if (maxNpcCount >= 32 && maxNpcCount < 40)
        {
            keyValue = 60;
        }
        else if (maxNpcCount >= 40 && maxNpcCount < 51)
        {
            keyValue = 70;
        }
        else if (maxNpcCount >= 51 && maxNpcCount < 57)
        {
            keyValue = 80;
        }
        else if (maxNpcCount >= 57 && maxNpcCount < 71)
        {
            keyValue = 90;
        }
        else
        {
            keyValue = 100;
        }
        skRenderer.SetBlendShapeWeight(0, keyValue);
        SetCollider();

    }


    private void SetCollider()
    {
        if (armValue < 20)
        {
            GetComponent<SphereCollider>().radius = .5f;
            GetComponent<SphereCollider>().center = new Vector3(0, 1, .28f);
        }
        else if (armValue >= 20 && armValue < 40)
        {
            GetComponent<SphereCollider>().radius = .7f;
            GetComponent<SphereCollider>().center = new Vector3(0, 1, .5f);
        }
        else if (armValue >= 40 && armValue < 60)
        {
            GetComponent<SphereCollider>().radius = .8f;
            GetComponent<SphereCollider>().center = new Vector3(0, 1, 1f);
        }
        else if (armValue >= 60 && armValue < 80)
        {
            GetComponent<SphereCollider>().radius = .9f;
            GetComponent<SphereCollider>().center = new Vector3(0, 1, 1.3f);
        }
        else if (armValue >= 80)
        {
            GetComponent<SphereCollider>().radius = 1.1f;
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
        if (transform.position.x >= 0) distanceX = Random.Range(-.015f, -.01f);
        else distanceX = Random.Range(.01f, .015f);

        //float distanceX = Random.Range(-.02f,.02f);
        float distanceZ = Random.Range(.03f, .06f);
        while (frame <= 100)
        {
            obj.transform.position = new Vector3(obj.transform.position.x + distanceX, obj.transform.position.y, obj.transform.position.z + distanceZ);
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
        GameObject stackPoint = GameObject.Find("StackPoint");
        float stackPointZ = stackPoint.transform.position.z;
        CameraController.instance.DeactivateCinemachineBrain();
        CameraController.instance.gameObject.transform.DOMove(new Vector3(0, 3, stackPointZ - 7f), .8f);
        int count = arms.transform.childCount;
        GameObject[] objects = new GameObject[count];
        float maxHeigt = count * npcHeight;
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
            objects[i].GetComponentInChildren<Renderer>().enabled = false;
            npcFinalYPosition += npcHeight;
            firstNpcFinalPosition = new Vector3(0, npcFinalYPosition, stackPointZ);
            objects[i].transform.DOMove(firstNpcFinalPosition, .2f);
            objects[i].transform.rotation = Quaternion.Euler(0, 180, 0);
            yield return new WaitForSeconds(.22f);
            objects[i].GetComponentInChildren<Renderer>().enabled = true;
            if (i == 0) CameraController.instance.gameObject.transform.DOMove(new Vector3(
                 0, maxHeigt + 2, objects[i].transform.position.z - 7f), count * .221f).SetEase(Ease.Linear);

            npcCount--;
            UIController.instance.SetNpcCountText(npcCount, maxNpcCount);
        }
        GetComponent<Collider>().enabled = false;
        npcFinalYPosition += npcHeight;
        firstNpcFinalPosition = new Vector3(0, npcFinalYPosition, stackPointZ);
        transform.DOMove(firstNpcFinalPosition, 1);
        transform.rotation = Quaternion.Euler(0, 180, 0);
        //PlayerController.instance.PlayerClapAnim();
        //CameraController.instance.SetCameraFinalInverse();
        yield return new WaitForSeconds(1.5f);
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
