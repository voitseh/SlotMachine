using UnityEngine;
using System.Collections;

public class CoinDrop : MonoBehaviour
{
	public float delayTime = 1f;
	private SpriteRenderer sr;
	private Animator animator;
	public GameObject fxTitleObject;
	// Singelton
	public static CoinDrop Instance{ get; private set; }

	public void Awake ()
	{
		Instance = this;
	}

	private	void Start ()
	{
		sr = GetComponent<SpriteRenderer> ();
		animator = GetComponent<Animator> ();
		DisplayFxTitle (false);
	}

	public void ShowMotion ()
	{
		sr.enabled = true;
		animator.Play ("CoinDrop");
		DisplayFxTitle (true);
		StartCoroutine (DelayAction (delayTime, () => {
			StopMotion (); 
		}));
	}

	public void StopMotion ()
	{
		sr.enabled = false;
		animator.Play ("Stop");
	}

	public void DisplayFxTitle (bool isON)
	{
		fxTitleObject.SetActive (isON);
	}

	IEnumerator DelayAction (float dTime, System.Action callback)
	{
		yield return new WaitForSeconds (dTime);
		callback ();
	}
}
