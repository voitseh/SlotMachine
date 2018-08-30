using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// manage tile grid
/// </summary>
public class SlotMachine : MonoBehaviour
{
	// Singelton
	public static SlotMachine Instance{ get; private set; }
	// tile prefab
	public GameObject cellItemPrefab;
	// tile lines
	static Transform[] lines;
	// tile items
	public Tile[] items;
	// tile size
	public Vector3 cellSize;
	// tile scale
	public Vector3 cellScale;
	public AudioSource spinSound, stopSound, cashSound;
	public static Line[] lineList;
	private int totalLine = 3;
	private int totalCell;
	private int totalSymbols;

	public void Awake ()
	{ 
		Instance = this;
	}

	private void Start ()
	{   
		cellSize = new Vector3 (2.0f, 1.4f, 1f);
		totalCell = 5;
		totalSymbols = 5; 
		InitArena ();
	}

	// init game arena, draw tile grid
	public  void InitArena ()
	{
		lines = new Transform[totalLine];
		lineList = new Line[totalLine];
		// tile line loop
		for (int i = 0; i < totalLine; i++) {
			GameObject pgo = new GameObject ();
			pgo.name = "Base" + (i + 1).ToString ("000");
			pgo.transform.parent = transform;
			GameObject go = new GameObject ();
			go.transform.parent = pgo.transform;
			Line script = go.AddComponent<Line> ();
			script.idx = i;
			Transform tf = go.transform;  
			lines [i] = tf;  
			tf.parent = pgo.transform;
			tf.localPosition = (i - 1.55f) * Vector3.right * cellSize.x + Vector3.up * cellSize.y * 1.45f; 
			go.name = "Line" + (i + 1).ToString ("000");
			script.items = new Tile[totalCell];
			// tile loop in some line
			for (int j = 0; j < totalCell; j++) {                                                                     
				GameObject g = Instantiate (cellItemPrefab) as GameObject;
				g.name = "Tile" + (j + 1).ToString ("000");
				Transform t = g.transform;
				Tile c = g.GetComponent<Tile> ();
				c.height = cellSize.y;
				c.slotMachine = this;
				c.SetTileType (Random.Range (0, totalSymbols) % totalSymbols);
				c.lineScript = script;
				script.items [j] = c;
				c.idx = j;
				t.parent = tf;
				t.localPosition = Vector3.down * j * cellSize.y * 0.7f;
				t.localRotation = Quaternion.identity; 
			}
			script.idx = i;
			lineList [i] = script;
		}
		items = GetComponentsInChildren<Tile> (); 
	}

	// delay method coroutine
	IEnumerator DelayAction (float dTime, System.Action callback)
	{
		yield return new WaitForSeconds (dTime);
		callback ();
	}

	IEnumerator RepeatAction (float dTime, int count, System.Action callback1, System.Action callback2, System.Action callback3)
	{ 
		if (count > 1)
			callback1 ();
		else
			callback3 ();
		if (--count > 0) {
			if (count == 1)
				callback2 ();
			yield return new WaitForSeconds (dTime);
			StartCoroutine (RepeatAction (dTime, count, callback1, callback2, callback3));
		}
	}

	// auto clear match tiles and go next turn
	private	void DoRoll ()
	{ 
		for (int i = 0; i < totalLine; i++) {    
			Transform line = lines [i]; 
			StartCoroutine (RepeatAction (0.1f, 8 + i * 3, () => { 
				line.SendMessage ("RollCells", SendMessageOptions.DontRequireReceiver);
			}, () => {
				stopSound.Play ();
			}, () => {	
				line.SendMessage ("RollCells", SendMessageOptions.DontRequireReceiver);
			}));  
		}

		StartCoroutine (DelayAction (2.2f, () => {   
			FindMatch (); 
			Lever.Instance.LeverUp (); 
		}));
	}

	private	void FindMatch ()
	{
		int count = 0;
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 3; j++) { 
				Tile tile = lineList [j].items [i]; 
				int type = tile.GetTileType (); 
				if (tile.tf.position.y == lineList [0].items [i].tf.position.y && type == lineList [0].items [i].GetTileType () &&
				    tile.tf.position.y != 2.03f && tile.tf.position.y > -1.8f)
					count++;
				if (count == 3) {
					// Turn on choice animation
					for (int k = 0; k < 3; k++) { 
						lineList [k].items [i].tf.GetChild (1).transform.GetComponent<SpriteRenderer> ().enabled = true;
					}
					CoinDrop.Instance.ShowMotion ();
					cashSound.Play ();
				}
			}
			count = 0;
		}
	}

	private void Spin ()
	{
		DoRoll ();
		spinSound.Play ();
	}

	public void DoSpin ()
	{                       
		Spin ();
	}

	private	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit ();
	}
}
