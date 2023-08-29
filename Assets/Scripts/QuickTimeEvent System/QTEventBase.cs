using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using System;
using UnityEngine;

public class QTEventBase : ScriptableObject
{
	public delegate void QTEvent();
	/// <summary>
	/// This is the prep event call for any QuickTimeEvent. This is called when a QT-Event is started. It handles the delays and the initial setups related to that QT.
	/// </summary>
	public QTEvent OnQTEPreparation;
	/// <summary>
	/// Invoked when the timeframe for keypress check is started.
	/// </summary>
	public QTEvent OnQTEStart;
	/// <summary>
	/// Invoked when a button is pressed or timeframe for button press has passed.
	/// </summary>
	public QTEvent OnQTEEnd;
	/// <summary>
	/// Invoked when the entire QTE Sequence has completed with a success or failure result.
	/// </summary>
	public QTEvent OnQTECompleted;
	/// <summary>
	/// Invoked upon being successful on the QTE
	/// </summary>
	public QTEvent OnQTESuccess;
	/// <summary>
	/// Invoked upon failing the QTE
	/// </summary>
	public QTEvent OnQTEFailure;

	[Expandable] public QTEDataSO QTEData;

	/// <summary>
	/// Responsible for initiating and setting up the QTE.
	/// </summary>
	/// <param name="qteManager">QTEManager is passed in here to do necessary function runs and changes down time line.</param>
	/// <returns></returns>
	public virtual async UniTask BeginEvent(QTEManager qteManager)
	{
		if (QTEData == null)
		{
			Debug.Log($"QTE Data not set for {name}");
			await UniTask.Yield();
			await UniTask.NextFrame();
		}

		Debug.Log($"QTE Data found, starting QTE");

		if (QTEData.EventStartDelay != 0f)
		{
			Debug.Log($"Delay found, waiting till {QTEData.EventStartDelay}");
			await UniTask.Delay(TimeSpan.FromSeconds(QTEData.EventStartDelay));
		}

		Debug.Log($"Delay time passed");
	}

	public virtual void EndEvent()
	{

	}

	public virtual void SuccessfulEventCompletion()
	{

	}

	public virtual void EventFailure()
	{
		throw new NotImplementedException();
	}

	// Use this for initialization
	//protected override void Init()
	//{
	//	base.Init();

	//}

	//// Return the correct value of an output port when requested
	//public override object GetValue(NodePort port)
	//{
	//	return null; // Replace this
	//}
}
