using UnityEngine;
using System.Collections;

public static class FalloffGen {

	public static float[,] GenerateFalloffMap(int size, float a, float b) {
		float[,] map = new float[size,size];

		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				float x = i / (float)size * 2 - 1;
				float y = j / (float)size * 2 - 1;

				float value = Mathf.Max (Mathf.Abs (x), Mathf.Abs (y));

                if (value < a){
                    map[i, j] = 0;
                } else if (value > b){
                    map[i, j] = 1;
                } else {
                    map[i, j] = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(a, b, value));
                }
				// map [i, j] = Evaluate(value, a, b);
			}
		}

		return map;
	}

	static float Evaluate(float value, float a, float b) {

		return Mathf.Pow (value, a) / (Mathf.Pow (value, a) + Mathf.Pow (b - b * value, a));
	}
}