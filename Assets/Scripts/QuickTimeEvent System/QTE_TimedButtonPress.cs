using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Timed Button Press QTE", menuName = "Systems-23/Quick Time Event System/Quick Time Events", order = 0)]
public class QTE_TimedButtonPress : QTEventBase
{
	public override async UniTask BeginEvent(QTEManager qteManager)
	{
		await base.BeginEvent(qteManager);
		//qteManager.TestUIController.StartQTE(QTEData);
		//var quickTimeInput = await qteManager.CheckForQuickTimeInput(QTEData);
		//CheckQTEInput(quickTimeInput);
	}

	/// <summary>
	/// Crosschecks player inputs received as function parameter with the expected inputs found in the QTEData Scriptable Object to determine if the QTE was successful or not.
	/// </summary>
	/// <param name="quickTimeInputs">A list formatted strings with their input device is passed here for validation.</param>
	private void CheckQTEInput(List<string> quickTimeInputs)
	{
		if (quickTimeInputs.Count != QTEData.ControlPath.Count)
		{
			OnQTEFailure?.Invoke();
			base.EndEvent();
			Debug.Log($"<color=#ff2626>QTE failed due to insufficient inputs.</color>");
			return;
		}

		Debug.Log($"Checking QTE input with expected input");
		var expectedInputs = QTEData.ControlPath;
		bool allMatched = true;
		foreach (var quickTimeInput in quickTimeInputs)
		{
			if (!expectedInputs.Contains(quickTimeInput))
			{
				allMatched = false;
				Debug.Log($"<color=#ff2626>QTE failed due to input mismatch on {quickTimeInput}.</color>");
				break;
			}
		}

		if (allMatched)
		{
			OnQTESuccess?.Invoke();
			Debug.Log($"<color=#75ff9a>QTE success</color>");
		}
		base.EndEvent();
	}

	public override void EndEvent()
	{
		//throw new System.NotImplementedException();
	}

	public override void SuccessfulEventCompletion()
	{
		//throw new System.NotImplementedException();
	}

	public override void EventFailure()
	{
		//throw new System.NotImplementedException();
	}
}