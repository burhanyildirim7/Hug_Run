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

	public void PlayerIdleAnim()
	{
		playerAnimator.SetTrigger("idle");
	}

	public void PlayerClapAnim()
	{
		playerAnimator.SetTrigger("clap");
	}

}
