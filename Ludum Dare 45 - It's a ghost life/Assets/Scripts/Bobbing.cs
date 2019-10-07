using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
	public uint sinCount = 1;

    // Update is called once per frame
    void Update()
    {
		if (Time.timeScale <= 0)
			return;

		float x = Time.unscaledTime;
		for(int i = 0; i < sinCount; ++i)
		{
			x = Sinned(x);
		}
		transform.localPosition -= Vector3.up * x * 0.0005f;
    }

	float Sinned(float x)
	{
		return Mathf.Sin(x);
	}
}
