using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Tile Line effect.
/// </summary>
public class Line : MonoBehaviour
{
	// singelton
	public static Line Instance{ get; private set; }
	// line order
	public int idx = 0;
	// tile list in line
	public Tile[] items;

	public void Awake ()
	{
		Instance = this;
	}

	// sort time order in line
	public void RollCells ()
	{
		List<Tile> tlist = new List<Tile> ();
		int y = 0, t = 5;
		int totalSymbols = 5; 
		for (int i = 1; i < 5; i++) {
			tlist.Add (items [i]);
			items [i].idx = y++; 
		} 
		for (int i = 0; i < 1; i++) {
			tlist.Add (items [i]);
			items [i].idx = y++;
			items [i].MoveTo (t++);
			int total = totalSymbols;
			if (idx == 0 || idx == 4)
				total--;
			items [i].SetTileType (Random.Range (0, total) % total);
		}
		items = tlist.ToArray ();
		for (int i = 0; i < 5; i++)
			items [i].TweenMoveTo (i);
	}
}
