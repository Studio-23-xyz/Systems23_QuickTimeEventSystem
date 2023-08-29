using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class QTEManager : MonoBehaviour
{
	public QTEventBase CurrentEvent;
	public string LastInputDevice;

	[ContextMenu("Execute QTE")]
	public async void DebugQTEExecution()
	{
		ExecuteEvent(CurrentEvent);
	}

	public async void ExecuteEvent(QTEventBase eventToExecute)
	{
		CurrentEvent = eventToExecute;
		await CurrentEvent.BeginEvent(this);
		//await UniTask.WaitUntil(() => CurrentEvent.BeginEvent() == true);
	}

	public async UniTask<List<string>> CheckForQuickTimeInput(QTEDataSO QTEData)
	{
		int buttonsPressed = 0;
		List<string> quickTimeInputs = new List<string>();
		string quickTimeInput = string.Empty;
		var inputReader = InputSystem.onAnyButtonPress.Call(quickTimePress =>
		{
			if (!quickTimeInputs.Contains($"<{quickTimePress.device.name}>/{quickTimePress.name}"))
			{
				Debug.Log($"Button pressed <color=#f426ff> {quickTimePress.name} </color> from device = {quickTimePress.device}");
				quickTimeInputs.Add($"<{quickTimePress.device.name}>/{quickTimePress.name}");
				LastInputDevice = quickTimePress.device.name;
				buttonsPressed++;
			}
		});
		Debug.Log($"Listening for QTE Input");
		float quickTimer = 0f;
		float concurrentInputTimer = 0f;
		while (quickTimer <= QTEData.EventTimer && buttonsPressed < QTEData.ControlPath.Count)
		{
			if (QTEData.ConcurrentInputBuffer > 0f && buttonsPressed > 0) concurrentInputTimer += Time.deltaTime;

			if (concurrentInputTimer > QTEData.ConcurrentInputBuffer)
			{
				if (buttonsPressed < QTEData.ControlPath.Count)
				{
					inputReader.Dispose();
					Debug.Log($"<color=#75ff9a>Buffer time elapse, QTE failing</color>");
					return quickTimeInputs;
				}
				quickTimeInputs.Clear();
				concurrentInputTimer = 0f;
				buttonsPressed = 0;
			}

			quickTimer += Time.deltaTime;
			await UniTask.Yield();
			await UniTask.NextFrame();
		}

		Debug.Log($"QTE Input found, cross-checking input");
		inputReader.Dispose();

		return quickTimeInputs;
	}
}
