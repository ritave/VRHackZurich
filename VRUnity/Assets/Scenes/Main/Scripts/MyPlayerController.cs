using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PositionProvider))]
public class MyPlayerController : MonoBehaviour
{
	private static readonly Vector3 FOWARD_VEC = new Vector3(0, 0, 1);
	private static readonly Vector3 BACKWARDS_VEC = new Vector3(0, 0, -1);
	private static readonly Vector3 LEFT_VEC = new Vector3(-1, 0, 0);
	private static readonly Vector3 RIGHT_VEC = new Vector3(1, 0, 0);

	public GameObject m_player;
	public GameObject m_leftHand;
	public GameObject m_rightHand;
	public Transform m_forwardVec;

	public Transform m_dragTop;
	public Transform m_dragBottom;

	private Material m_lHandMaterial;
	private Material m_rHandMaterial;

	private OVRCameraRig m_cameraRig;
	private PositionProvider m_positionProvider;

	private Vector3 m_realLifeLHandPos;
	private Vector4 m_lHandRot;
	private Vector3 m_realLifeRHandPos;
	private Vector4 m_rHandRot;
	private Vector3 m_realLifeBodyPos;

	private bool m_rClosed;

	static public System.Action<float> HandPercent;

	private void Awake()
	{
		m_cameraRig = GetComponentInChildren<OVRCameraRig>();
		m_positionProvider = GetComponent<PositionProvider>();
	}

	private void FixedUpdate()
	{
		GetData();
		UpdateBody();
		UpdateHands();
	}

	private void GetData()
	{
		m_positionProvider.GetData(
			ref m_realLifeLHandPos,
			ref m_realLifeRHandPos,
			ref m_rClosed,
			ref m_realLifeBodyPos
		);
	}

	private void UpdateBody()
	{
		m_player.transform.position = m_realLifeBodyPos;
	}

	private void UpdateHands()
	{
		m_rightHand.transform.position = m_realLifeRHandPos;
		m_leftHand.transform.position = m_realLifeLHandPos;

		var fromLTarget = new Vector3(
			m_realLifeBodyPos.x,
			m_realLifeLHandPos.y,
			m_realLifeBodyPos.z
		);

		var fromRTarget = new Vector3(
			m_realLifeBodyPos.x,
			m_realLifeRHandPos.y,
			m_realLifeBodyPos.z
		);
		
		m_rightHand.transform.rotation = Quaternion.FromToRotation(Vector3.forward, m_realLifeRHandPos - m_realLifeBodyPos);
		m_leftHand.transform.rotation = Quaternion.FromToRotation(Vector3.forward, m_realLifeLHandPos - m_realLifeBodyPos);
		
		if (m_rClosed)
		{
			float percent = 0.0f;
			float lowest = m_dragBottom.position.y;
			float highest = m_dragTop.position.y;
			if (m_realLifeRHandPos.y > highest)
				percent = 1.0f;
			else if (m_realLifeRHandPos.y < lowest)
				percent = 0.0f;
			else
				percent = Mathf.Clamp01((m_realLifeRHandPos.y - lowest) / (highest - lowest));
			
			HandPercent(percent);
		}
	}
}
