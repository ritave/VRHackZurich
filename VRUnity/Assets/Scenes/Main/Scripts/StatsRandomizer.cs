using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class StatsRandomizer : MonoBehaviour
{
	private const float MIN_HEIGHT = 0.1f;
	private const float MAX_HEIGHT = 0.6f;
	private static readonly Color COLOR_BOTTOM = Color.cyan;
	private static readonly Color COLOR_TOP = Color.red;

	private Renderer m_renderer;
	private float m_heightTop;
	private float m_heightBottom;

	private void Awake()
	{
		m_renderer = GetComponent<Renderer>();
		m_heightTop = Random.Range(MIN_HEIGHT, MAX_HEIGHT);
		m_heightBottom = Random.Range(MIN_HEIGHT, MAX_HEIGHT);
	}

	private void Start()
	{
		SetHeight(m_heightBottom);
	}

	private void OnEnable()
	{
		MyPlayerController.HandPercent += HandleHand;
	}

	private void OnDisable()
	{
		MyPlayerController.HandPercent -= HandleHand;
	}

	private void SetHeight(float height)
	{
		//m_renderer.material.color = new HSBColor(height, 1f, 0.6f).ToColor();

		float heightPercent = (height - MIN_HEIGHT) / (MAX_HEIGHT - MIN_HEIGHT);
		m_renderer.material.color = Color.Lerp(COLOR_BOTTOM, COLOR_TOP, heightPercent);

		transform.localScale = new Vector3(
			transform.localScale.x,
			height,
			transform.localScale.z
		);
		transform.localPosition = new Vector3(
			transform.localPosition.x,
			0.15f + ((height - 0.2f) / 2.0f),
			transform.localPosition.z
		);
	}

	private void HandleHand(float percent)
	{
		SetHeight(Mathf.Lerp(m_heightBottom, m_heightTop, percent));
	}
}
