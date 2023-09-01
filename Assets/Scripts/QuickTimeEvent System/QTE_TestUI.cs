using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QTE_TestUI : MonoBehaviour
{
	public TextMeshProUGUI EventNameHeader;
	public TextMeshProUGUI EventDescriptionHeader;
	public TextMeshProUGUI EventPromptHint;

	public Image TimeBar;

	public QTEventBase EventToPlay;

	private QTEManager _qteManager;

	public bool QTEInputReceived;

	public async void SetQTEPreparation(QTEDataSO qtData, QTEManager qteManager)
	{
		_qteManager = qteManager;
		Debug.Log($"Setting prep status to UI");
		EventNameHeader.text = qtData.EventName;
		EventDescriptionHeader.text = qtData.EventDescription;
		EventPromptHint.text = $"Waiting for prep to complete.";

		qteManager.CurrentEvent.OnQTESuccess += OnQteSuccess;
		qteManager.CurrentEvent.OnQTEFailure += OnQteFailure;
		qteManager.CurrentEvent.OnQTECompleted += EndQTE;
		//float timer = 0f;
		//float factor = 0f;
		//TimeBar.fillAmount = 1f;
		//while (timer < qtData.EventStartDelay)
		//{
		//	factor = Mathf.InverseLerp(0f, qtData.EventStartDelay, timer);
		//	TimeBar.fillAmount = (1f - factor);
		//	timer += Time.deltaTime;
		//	await UniTask.Yield();
		//	await UniTask.NextFrame();
		//}

		Debug.Log($"Prep UI functionality completed.");
	}

	private void OnQteFailure()
	{
		EventPromptHint.text = $"<color=#fc0703>QTE Failed!</color>";
	}

	private void OnQteSuccess()
	{
		EventPromptHint.text = $"<color=#03fc07>QTE Successful!</color>";
	}

	public async void StartQTE(QTEDataSO qtData)
	{
		foreach (var controlPath in qtData.ControlPath)
		{
			EventPromptHint.text += $"< {controlPath} >";
		}
	}

	public async void EndQTE()
	{
		await UniTask.Delay(TimeSpan.FromSeconds(2f));

		EventPromptHint.text = $"<color=#03bafc>QTE Completed!</color>";
	}
}
