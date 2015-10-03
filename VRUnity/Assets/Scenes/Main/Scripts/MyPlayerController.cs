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

	private OVRCameraRig m_cameraRig;
	private PositionProvider m_positionProvider;

	private Vector3 m_realLifeLHandPos;
	private Vector3 m_realLifeRHandPos;
	private Vector3 m_realLifeBodyPos;
	
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
			ref m_realLifeBodyPos
		);
	}

	private void UpdateBody()
	{
		m_player.transform.position = new Vector3(
			m_realLifeBodyPos.x,
			m_player.transform.position.y,
			m_realLifeBodyPos.z	
		);
	}

	private void UpdateHands()
	{

	}
}
