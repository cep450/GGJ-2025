using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLines : MonoBehaviour
{

	[SerializeField] RectTransform lines;
	[SerializeField] float durationFlip = 1f;
	float timerFlip = 0f;
	[SerializeField] float durationScale = 0.3f;
	float timerScale = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timerFlip -= Time.deltaTime;
		if(timerFlip <= 0f) {
			timerFlip = durationFlip;
			lines.transform.Rotate(0f, 180f, 0f, Space.Self);
		}

		timerScale -= Time.deltaTime;
		if(timerScale <= 0f) {
			timerScale = durationScale;
			if(lines.transform.localScale.y == 1f) {
				lines.transform.localScale = new Vector3(1f, 1.1f, 1f);
			} else {
				lines.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
    }
}
