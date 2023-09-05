using UnityEngine;

public class DebugTools : MonoBehaviour
{
	public void SlowDownTimescale()
	{
		Time.timeScale = 0.5f;
	}

	public void RevertTimescale()
	{
		Time.timeScale = 1f;
	}
}
