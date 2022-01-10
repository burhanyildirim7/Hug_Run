using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Animator playerAnimator, ghostAnimator, npcCountTextAnim;
    public Renderer armForStart, armForGame;

    public GameObject _countCanvas;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        SetArmForStart();
    }

    public void SetArmForGaming()
    {
        armForGame.enabled = true;
        armForStart.enabled = false;
    }

    public void SetArmForStart()
    {

        armForGame.enabled = false;
        armForStart.enabled = true;
    }

    public void PlayerWalkAnim()
    {
        playerAnimator.ResetTrigger("idle");
        ghostAnimator.ResetTrigger("idle");
        playerAnimator.SetTrigger("walk");
        ghostAnimator.SetTrigger("walk");
    }

    public void PlayerIdleStartAnim()
    {
        playerAnimator.ResetTrigger("walk");
        ghostAnimator.ResetTrigger("walk");
        playerAnimator.SetTrigger("idle");
        ghostAnimator.SetTrigger("idle");
    }

    public void PlayerIdleAnim()
    {
        _countCanvas.SetActive(false);

        playerAnimator.ResetTrigger("walk");
        ghostAnimator.ResetTrigger("walk");
        playerAnimator.SetTrigger("idle");
        ghostAnimator.SetTrigger("idle");
    }

    public void PlayerClapAnim()
    {
        playerAnimator.ResetTrigger("walk");
        ghostAnimator.ResetTrigger("walk");
        playerAnimator.SetTrigger("idle");
        ghostAnimator.SetTrigger("idle");
    }

    public void NpcCountTextAnim()
    {
        npcCountTextAnim.SetTrigger("start");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null)
        {
            if (other.transform.parent.transform.CompareTag("xs"))
            {


                //CameraController.instance.SetCameraFinalInverse();
                GetComponent<Collider>().enabled = false;

                GameManager.instance.FinalScoreMultiply(other.name);
                UIController.instance.ActivateWinScreen();


            }
        }
    }

    public void PlayerStartPosition()
    {
        _countCanvas.SetActive(true);

        PlayerIdleStartAnim();
        GetComponent<Collider>().enabled = true;
        transform.parent.transform.position = new Vector3(0, .5f, 0);
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

}
