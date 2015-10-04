using UnityEngine;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System;

public class PositionProvider : MonoBehaviour
{
	private Thread m_listenThread;
	private Vector3 m_head;
	private Vector3 m_lHand;
	private Vector3 m_rHand;
	private bool m_rightClosed;
	private System.Object m_threadLock = new System.Object();
	private volatile bool m_shouldRun;

	private UdpClient udpClient;

	public void GetData(ref Vector3 leftHand, ref Vector3 rightHand, ref bool rightClosed, ref Vector3 head)
	{
		lock (m_threadLock)
		{
			leftHand = m_lHand;
			rightHand = m_rHand;
			head = m_head;
			rightClosed = m_rightClosed;
		}
	}

	private void OnEnable()
	{
		StartThread();
	}

	private void OnDisable()
	{
		StopThread();
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
		udpClient.Close();
		m_listenThread.Interrupt();
		m_listenThread.Join();
	}

	private Vector3 FromOculusToUnity(Vector3 point)
	{
		return new Vector3(
			point.x,
			point.y + 0.64f, // Adding magic value because kinect is on the table
			-point.z
		);
	}

	private void Listen()
	{
		System.Random myRandom = new System.Random();
		udpClient = new UdpClient(7272, AddressFamily.InterNetwork);

		var endPoint = default(IPEndPoint);

		byte[] bytes;

		while (m_shouldRun)
		{
			try
			{
				bytes = udpClient.Receive(ref endPoint);
				if (bytes == null || bytes.Length == 0)
					break;

				int offset = 0;
				lock (m_threadLock)
				{
					m_head = FromOculusToUnity(Vector3FromBytes(bytes, ref offset));
					m_rHand = FromOculusToUnity(Vector3FromBytes(bytes, ref offset));
					//m_rHandRotation = Vector4FromBytes(bytes, ref offset);
					m_lHand = FromOculusToUnity(Vector3FromBytes(bytes, ref offset));
					//m_lHandRotation = Vector4FromBytes(bytes, ref offset);
					m_rightClosed = BoolFromBytes(bytes, ref offset);
				}
			} catch (ThreadInterruptedException)
			{
				// Empty on purpose
			}
		}
	}

	private Vector3 Vector3FromBytes(byte[] bytes, ref int offset)
	{
		var result = new Vector3(
			BitConverter.ToSingle(bytes, offset),
			BitConverter.ToSingle(bytes, offset + 4),
			BitConverter.ToSingle(bytes, offset + 8)
		);
		offset += 12;
		return result;
	}

	private Vector4 Vector4FromBytes(byte[] bytes, ref int offset)
	{
		var result = new Vector4(
			BitConverter.ToSingle(bytes, offset),
			BitConverter.ToSingle(bytes, offset + 4),
			BitConverter.ToSingle(bytes, offset + 8),
			BitConverter.ToSingle(bytes, offset + 12)
        );
		offset += 16;
		return result;
	}

	private bool BoolFromBytes(byte[] bytes, ref int offset)
	{
		var result = BitConverter.ToBoolean(bytes, offset);
		offset += 1;
		return result;
	}
}
