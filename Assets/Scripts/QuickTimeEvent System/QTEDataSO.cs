using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

[Serializable]
[CreateAssetMenu(fileName = "QTEData", menuName = "Systems-23/Quick Time Event System/EventData", order = 1)]
public class QTEDataSO : ScriptableObject
{
	public string EventName;
	[TextArea]
	public string EventDescription;

	/// <summary>
	/// Used if the event should be delayed
	/// </summary>
	public float EventStartDelay;
	/// <summary>
	/// Used to determine how long should the event be alive for
	/// </summary>
	public float EventTimer;

	[InputControl(layout = "Button")]
	public List<string> ControlPath;

	public float ConcurrentInputBuffer;
}
