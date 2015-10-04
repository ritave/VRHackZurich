using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatsDrag : MonoBehaviour
{
	public RectTransform m_handleBgTransform;
	public RectTransform m_handle;

	public float m_padding = 7.5f;

	private void OnEnable()
	{
		MyPlayerController.HandPercent += HandleHand;
		HandleHand(0);
	}

	private void OnDisable()
	{
		MyPlayerController.HandPercent -= HandleHand;
	}

	private void HandleHand(float percent)
	{
		float bgHeight = m_handleBgTransform.rect.height;
		bgHeight -= 2.0f * m_padding;
		m_handle.anchoredPosition = new Vector2(
			m_handle.anchoredPosition.x,
			m_padding + bgHeight * percent
		);
	}
}
