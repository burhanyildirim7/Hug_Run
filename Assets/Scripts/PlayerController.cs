using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
	public Animator playerAnimator;
	public Renderer armForStart, armForGame;

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
		playerAnimator.SetTrigger("walk");
	}

	public void PlayerIdleStartAnim()
	{
		playerAnimator.SetTrigger("idlestart");
	}

	public void PlayerIdleAnim()
	{
		playerAnimator.SetTrigger("idle");
	}

	public void PlayerClapAnim()
	{
		playerAnimator.SetTrigger("clap");
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.parent != null)
		{
			if (other.transform.parent.transform.CompareTag("xs"))
			{
				GetComponent<Collider>().enabled = false;
				GameManager.instance.FinalScoreMultiply(other.name);
				UIController.instance.ActivateWinScreen();

			}
		}
		
	}

	public void PlayerStartPosition()
	{
		PlayerIdleStartAnim();
		GetComponent<Collider>().enabled = true;
		transform.parent.transform.position = new Vector3(0, .5f, 0);
		transform.localPosition = Vector3.zero;
		transform.rotation = Quaternion.Euler(0, 0, 0);
	}

}
