using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSCounter : MonoBehaviour
{
	private float frequency = 1.0f;
	private string fps;
	void Start()
	{
		StartCoroutine(FPS());
	}

	private IEnumerator FPS()
	{
		for (; ; )
		{
			// Capture frame-per-second
			int lastFrameCount = Time.frameCount;
			float lastTime = Time.realtimeSinceStartup;
			yield return new WaitForSeconds(frequency);
			float timeSpan = Time.realtimeSinceStartup - lastTime;
			int frameCount = Time.frameCount - lastFrameCount;

			// Display it

			fps = string.Format("FPS: {0}", Mathf.RoundToInt(frameCount / timeSpan));
		}
	}

    private void Update()
    {
        transform.GetComponent<TextMeshProUGUI>().text = fps;
    }
}
