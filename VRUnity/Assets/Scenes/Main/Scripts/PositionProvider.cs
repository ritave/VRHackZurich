using UnityEngine;
using System.Threading;

public class PositionProvider : MonoBehaviour
{
	private Thread m_listenThread;
	private Vector3 m_body;
	private Vector3 m_lHand;
	private Vector3 m_rHand;
	private Object m_threadLock = new Object();
	private volatile bool m_shouldRun;

	public void GetData(ref Vector3 leftHand, ref Vector3 rightHand, ref Vector3 body)
	{
		lock (m_threadLock)
		{
			leftHand = m_lHand;
			rightHand = m_rHand;
			body = m_body;
		}
	}

	private void Start()
	{
		StartThread();
	}

	private void StartThread()
	{
		m_shouldRun = true;
		m_listenThread = new Thread(this.Listen);
		m_listenThread.Start();
	}

	private void StopThread()
	{
		m_shouldRun = false;
		m_listenThread.Interrupt();
		m_listenThread.Join();
	}


	private void Listen()
	{
		System.Random myRandom = new System.Random();
		while (m_shouldRun)
		{
			try
			{
				lock (m_threadLock)
				{
					m_body = new Vector3(
						m_body.x + (float)myRandom.Next(11) / 100.0f - 0.05f,
						0,
						m_body.z + (float)myRandom.Next(11) / 100.0f - 0.05f
					);
				}
				Thread.Sleep(50);
			} catch (ThreadInterruptedException)
			{
				// Empty on purpose
			}
		}
	}
}
