using NUnit.Framework;
using Studio23.SS2.QTESystem.Core;
using Studio23.SS2.QTESystem.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class TimedButtonPressQTE_Test
{
	private QTE_TimedButtonPress _timedButtonPressEvent;
	private QTEDataSO _qtDataSO;
	private QTEManager _qtManager;

	[SetUp]
	public void TestSetup()
	{
		_timedButtonPressEvent = ScriptableObject.CreateInstance(typeof(QTE_TimedButtonPress)) as QTE_TimedButtonPress;
		_qtDataSO = ScriptableObject.CreateInstance(typeof(QTEDataSO)) as QTEDataSO;
		_qtManager = new GameObject().AddComponent<QTEManager>();
	}

	// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
	// `yield return null;` to skip a frame.
	[UnityTest]
	public IEnumerator _Wait_For_3_Seconds_Then_Return_True_()
	{
		bool hasDelayed = false;
		_qtDataSO.EventStartDelay = 3f;
		_qtDataSO.EventTimer = 2f;
		_qtDataSO.ControlPath.Add($"<Keyboard>/w");
		_timedButtonPressEvent.QTEData = _qtDataSO;
		_timedButtonPressEvent.OnQTEStart += () =>
		{
			hasDelayed = true;
		};
		_qtManager.ExecuteEvent(_timedButtonPressEvent);

		yield return new WaitForSeconds(_qtDataSO.EventStartDelay);

		Assert.IsTrue(hasDelayed);
		_qtManager.TerminateCurrentQTE();

		// Use the Assert class to test conditions.
		// Use yield to skip a frame.
		yield return null;
	}

	[UnityTest]
	public IEnumerator _Check_For_Action_Timer_Start_()
	{
		_qtDataSO.EventStartDelay = 3f;
		_qtDataSO.EventTimer = 2f;
		_qtDataSO.ControlPath.Add($"<Keyboard>/w");
		_timedButtonPressEvent.QTEData = _qtDataSO;

		_qtManager.ExecuteEvent(_timedButtonPressEvent);
		yield return new WaitForSeconds(_qtDataSO.EventStartDelay);

		Assert.IsTrue(_qtManager.IsListeningForInput);
		_qtManager.TerminateCurrentQTE();

		// Use the Assert class to test conditions.
		// Use yield to skip a frame.
		yield return null;
	}

	[UnityTest]
	public IEnumerator _Wait_For_2_Seconds_Delay_Then_Wait_For_4_Seconds_For_Input_Event_()
	{
		bool actionTimePassed = false;
		_qtDataSO.EventStartDelay = 2f;
		_qtDataSO.EventTimer = 4f;
		_qtDataSO.ControlPath.Add($"<Keyboard>/w");
		_timedButtonPressEvent.QTEData = _qtDataSO;

		_timedButtonPressEvent.OnQTEEnd += () =>
		{
			actionTimePassed = true;
		};
		_qtManager.ExecuteEvent(_timedButtonPressEvent);

		yield return new WaitForSeconds(_qtDataSO.EventStartDelay);
		yield return new WaitForSeconds(_qtDataSO.EventTimer);


		Assert.IsTrue(actionTimePassed);

		_qtManager.TerminateCurrentQTE();

		// Use the Assert class to test conditions.
		// Use yield to skip a frame.
		yield return null;
	}
}
