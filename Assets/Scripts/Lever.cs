using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour
{
	public GameObject leverUp;
	public GameObject leverDown;
	public GameObject insertToken;
	public AudioSource knobDownSound;
	// Singelton
	public static Lever Instance{ get; private set; }

	public void Awake ()
	{
		Instance = this;
	}

	private	void OnMouseDown ()
	{  
		if (Input.GetMouseButtonDown (0)) {
			knobDownSound.Play ();
			leverUp.SetActive (false);
			leverDown.SetActive (true); 
			CoinAnimation.Instance.PushCoin (); 
			// switch off choice animation
			for (int i = 0; i < 5; i++) {
				for (int j = 0; j < 3; j++) { 
					SlotMachine.lineList [j].items [i].tf.GetChild (1).transform.GetComponent<SpriteRenderer> ().enabled = false;
				}
			}
			CoinDrop.Instance.DisplayFxTitle (false);
		}
	}

	public void LeverUp ()
	{
		leverUp.SetActive (true);
		leverDown.SetActive (false);
		insertToken.SetActive (true);
		insertToken.transform.position = new Vector3 (2.7f, 1.32f, 0);
	}
}
