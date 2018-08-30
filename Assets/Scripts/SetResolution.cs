using UnityEngine;
using System.Collections;

public class SetResolution : MonoBehaviour
{
	public int width = 800;
	public int height = 600;

	void Start ()
	{
#if UNITY_STANDALONE || UNITY_WEBPLAYER
		if (Screen.width != width || Screen.height != height)
			Screen.SetResolution (width, height, false);
#endif
		Destroy (this);
	}
	
}
