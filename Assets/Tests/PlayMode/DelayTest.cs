using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class DelayTest
{
	// A Test behaves as an ordinary method
	[Test]
	public void DelayTestSimplePasses()
	{
		// Use the Assert class to test conditions
	}

	// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
	// `yield return null;` to skip a frame.
	[UnityTest]
	public IEnumerator _Wait_For_2_Seconds_Then_Return_True_()
	{
		var timedPress = ScriptableObject.CreateInstance(typeof(QTE_TimedButtonPress)) as QTE_TimedButtonPress;
		var timedPressData = ScriptableObject.CreateInstance(typeof(QTEDataSO)) as QTEDataSO;
		var manager = new QTEManager();
		bool hasDelayed = false;
		timedPressData.EventStartDelay = 3f;
		timedPress.QTEData = timedPressData;
		timedPress.OnQTEStart += () =>
		{
			hasDelayed = true;
		};

		timedPress.BeginEvent(manager);

		yield return new WaitForSeconds(timedPressData.EventStartDelay);

		Assert.IsTrue(hasDelayed);

		// Use the Assert class to test conditions.
		// Use yield to skip a frame.
		yield return null;
	}
}
