using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public CinemachineVirtualCamera cmVcam;
    public Vector3 cameraFinalOffset = new Vector3(0, 4, -4);
    public Vector3 cameraFinalInverseOffset = new Vector3(0, 4, -4);
    public Vector3 cameraStartOffset = new Vector3(0, 3.8f, -3);
    public GameObject cameraLookAtObj, player;



    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        SetCameraStartOffset();
    }

    public void DeactivateCMVcam()
    {
        GetComponent<CinemachineBrain>().enabled = false;
    }

    public void ActivateCMVcam()
    {
        GetComponent<CinemachineBrain>().enabled = true;
    }


    public void SetCameraFinalOffset()
    {
        StartCoroutine(CameraFinalEase());
    }

    public void SetCameraFinalInverse()
    {
        cmVcam.LookAt = player.transform;
        cmVcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = cameraFinalInverseOffset;

    }

    public void SetCameraStartOffset()
    {
        GetComponent<CinemachineBrain>().enabled = true;
        cmVcam.LookAt = cameraLookAtObj.transform;
        cmVcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = cameraStartOffset;
    }

    IEnumerator CameraFinalEase()
    {
        float count = 0;
        while (Vector3.Distance(cmVcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, cameraFinalOffset) == 0)
        {
            cmVcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(
                cmVcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset,
                cameraFinalOffset, count);
            count += .005f;
            yield return new WaitForEndOfFrame();
        }
    }


    public void DeactivateCinemachineBrain()
    {
        GetComponent<CinemachineBrain>().enabled = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void ActivateCinemachineBrain()
    {
        GetComponent<CinemachineBrain>().enabled = false;
    }



}
