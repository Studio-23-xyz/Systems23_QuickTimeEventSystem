using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class QTEManager : MonoBehaviour
{
	public QTE_TestUI TestUIController;
	public QTEventBase CurrentEvent;
	public string LastInputDevice;
	public IDisposable InputReader;


	public bool IsListeningForInput;

	[ContextMenu("Execute QTE")]
	public async void DebugQTEExecution()
	{
		ExecuteEvent(CurrentEvent);
	}

	/// <summary>
	/// Takes in a QuickTimeEvent Base and starts the event. If an event is ongoing, the event that is passed will be queued for execution upon completion of the current event.
	/// </summary>
	/// <param name="eventToExecute">QTEEvent Base containing QTE parameters and required inputs that will be handled by rest of the components.</param>
	public async void ExecuteEvent(QTEventBase eventToExecute)
	{
		CurrentEvent = eventToExecute;
		//TestUIController.SetQTEPreparation(CurrentEvent.QTEData, this);
		await CurrentEvent.BeginEvent(this);
	}

	/// <summary>
	/// QTE Data is passed in this function, while the function reads inputs from user throughout the QTE timer. 
	/// </summary>
	/// <param name="QTEData">Scriptable Object containing various data regarding the QTE</param>
	/// <returns>If the player presses the number of buttons expected for this QTE the function returns a List of string that is formatted
	/// with the input device name for validating if correct inputs were pressed or not. </returns>
	public async UniTask<List<string>> CheckForQuickTimeInput(QTEDataSO QTEData)
	{
		IsListeningForInput = true;
		int buttonsPressed = 0;
		List<string> quickTimeInputs = new List<string>();
		string quickTimeInput = string.Empty;
		InputReader = InputSystem.onAnyButtonPress.Call(quickTimePress =>
		{
			if (!QTEData.ControlPath.Contains($"<{quickTimePress.device.name}>/{quickTimePress.name}"))
			{
				buttonsPressed = 100;
				Debug.Log($"<color=#ff2626>Mismatched QTE input detected. Failing QTE</color>");
			}
			if (!quickTimeInputs.Contains($"<{quickTimePress.device.name}>/{quickTimePress.name}"))
			{
				Debug.Log($"Button pressed <color=#f426ff> {quickTimePress.name} </color> from device = {quickTimePress.device.name}");
				quickTimeInputs.Add($"<{quickTimePress.device.name}>/{quickTimePress.name}");
				LastInputDevice = quickTimePress.device.name;
				buttonsPressed++;
			}
		});
		Debug.Log($"Listening for QTE Input");
		float quickTimer = 0f;
		float concurrentInputTimer = 0f;
		var startTimer = Time.unscaledTime;
		while (quickTimer < QTEData.EventTimer && buttonsPressed < QTEData.ControlPath.Count)
		{
			if (QTEData.ConcurrentInputBuffer > 0f && buttonsPressed > 0) concurrentInputTimer += Time.deltaTime;
			if (concurrentInputTimer > QTEData.ConcurrentInputBuffer)
			{
				if (buttonsPressed < QTEData.ControlPath.Count)
				{
					InputReader.Dispose();
					IsListeningForInput = false;
					Debug.Log($"<color=#75ff9a>Buffer time elapse, QTE failing</color>");
					//TestUIController.QTEInputReceived = true;
					return quickTimeInputs;
				}
				quickTimeInputs.Clear();
				concurrentInputTimer = 0f;
				buttonsPressed = 0;
			}

			quickTimer = Time.unscaledTime - startTimer;
			//quickTimer += (float) stopwatch.ElapsedMilliseconds / 1000f;
			//Debug.Log($"Event timer : {quickTimer}");
			await UniTask.Yield();
			await UniTask.NextFrame();
		}

		IsListeningForInput = false;

		//TestUIController.QTEInputReceived = true;

		Debug.Log($"QTE Input time window passed, cross-checking input");
		InputReader.Dispose();

		return quickTimeInputs;
	}

	public void TerminateCurrentQTE()
	{
		Debug.Log($"Terminating this {CurrentEvent.name} event");
		InputReader.Dispose();
		IsListeningForInput = false;
		CurrentEvent.EndEvent();
	}
}
