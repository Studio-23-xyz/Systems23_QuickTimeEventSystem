using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SingleButtonPressQTEData_Test_Case_01
{
	private QTEDataSO _qteData;

	[SetUp]
	public void LoadCase()
	{
		_qteData = Resources.Load<QTEDataSO>("QTEData_TimedButtonPress_SingleButton_Case01");
	}


	// A Test behaves as an ordinary method
	[Test]
	public void _Single_Button_Press_Delay_Time_SO_Data_Case_01_()
	{
		Assert.AreEqual(3f, _qteData.EventStartDelay);
		// Use the Assert class to test conditions
	}

	[Test]
	public void _Single_Button_Action_Timer_SO_Data_Case_01_()
	{
		Assert.AreEqual(4f, _qteData.EventTimer);
	}

	[Test]
	public void _Single_Button_Press_Control_Path_Case_01_()
	{
		Assert.AreEqual($"<Keyboard>/w", _qteData.ControlPath[0]);
	}
}

public class MultipleButtonPressQTEData_Test_Case_01
{
	private QTEDataSO _qteData;

	[SetUp]
	public void LoadCase()
	{
		_qteData = Resources.Load<QTEDataSO>("QTEData_TimedButtonPress_ConcurrentButtons_Case02");
	}

	[Test]
	public void _Multiple_Button_Press_Delay_Time_SO_Data_Case_01_()
	{
		Assert.AreEqual(2f, _qteData.EventStartDelay);
	}

	[Test]
	public void _Multiple_Button_Press_Event_Timer_SO_Data_Case_01_()
	{
		Assert.AreEqual(5f, _qteData.EventTimer);
	}

	[Test]
	public void _Multiple_Button_Press_Control_Path_SO_Data_Case_01_()
	{
		List<string> _expectedControls = new List<string>();
		_expectedControls.Add($"<Keyboard>/q");
		_expectedControls.Add($"<Keyboard>/e");
		Assert.AreEqual(_expectedControls, _qteData.ControlPath);
	}

	[Test]
	public void _Multiple_Button_Press_Buffer_Timer_SO_Data_Case_01_()
	{
		Assert.AreEqual(0.35f, _qteData.ConcurrentInputBuffer);
	}
}