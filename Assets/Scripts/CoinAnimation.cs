using UnityEngine;
using System.Collections;

public class CoinAnimation : MonoBehaviour
{
	[SerializeField]
	private Animator ani;
	[SerializeField]
	private Sprite token;
	// singelton
	public static CoinAnimation Instance{ get; private set; }

	public void Awake ()
	{
		Instance = this;
	}

	// use this for initialization
	private	void Start ()
	{
		ani.enabled = false;
	}

	public void PushCoin ()
	{
		ani.enabled = true;
	}

	public void Spin ()
	{
		ani.enabled = false;
		transform.GetComponent<SpriteRenderer> ().sprite = token;
		gameObject.SetActive (false);
		SlotMachine.Instance.DoSpin ();
	}
}
