using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

/// <summary>
/// Scriptable object containing data about the QuickTime Event such as name, dsc, delay, time limit and expected inputs. 
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "QTEData", menuName = "Systems-23/Quick Time Event System/EventData", order = 1)]
public class QTEDataSO : ScriptableObject
{
	public string EventName;
	[TextArea]
	public string EventDescription;

	/// <summary>
	/// Used if the event should be delayed for set amount of time before starting to listen for inputs from player.
	/// </summary>
	public float EventStartDelay;
	/// <summary>
	/// Used to determine how long should the event be alive for and listen to player inputs.
	/// </summary>
	public float EventTimer;

	/// <summary>
	/// A list of expected controls that the player must press in order to succeed in the QTE.
	/// </summary>
	[InputControl(layout = "Button")]
	public List<string> ControlPath;

	/// <summary>
	/// In case this QTE expected multiple player inputs in very quick succession, we set a buffer timer within which the player must press rest of the buttons
	/// after the first button press.
	/// </summary>
	[EnableIf("IsBufferRequired")]
	public float ConcurrentInputBuffer = 0f;

	private bool IsBufferRequired => ControlPath.Count > 1;
}
