using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

/// <summary>
/// Tile cell effect.
/// </summary>
public class Tile : MonoBehaviour
{
	public SlotMachine slotMachine;
	// tile order
	public int idx = 0;
	public float height = 1f;
	// tile type
	private int _type = 0;
	public Transform tf;
	public Sprite[] sprites;
	private SpriteRenderer shapeRenderer;
	// Tile Line Component
	public Line lineScript;

	private void Awake ()
	{
		tf = transform;
		shapeRenderer = tf.FindChild ("Shape").GetComponent<SpriteRenderer> ();
	}

	// setup Tile Type.
	public void SetTileType (int type)
	{
		_type = type;
		shapeRenderer.sprite = sprites [type];
	}

	// Get Tile Type.
	public int GetTileType ()
	{
		return _type;
	}

	// move To Order Position
	public void MoveTo (int seq)
	{
		tf.localPosition = Vector3.forward * (seq * height);
	}

	// Move with Tweening Animation
	public void TweenMoveTo (int seq)
	{
		TweenMove (tf, tf.localPosition, Vector3.down * (seq * height * 0.7f));
	}

	// Move with Tweening Animation
	void TweenMove (Transform tr, Vector3 pos1, Vector3 pos2)
	{
		tr.localPosition = pos1;
		TweenParms parms = new TweenParms ().Prop ("localPosition", pos2).Ease (EaseType.Linear);
		HOTween.To (tr, 0.1f, parms);
	}
}
