using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CurveDrawer : MonoBehaviour
{
	public List<GameObject> ControlPoints = new List<GameObject>();

	public GameObject CurvePointObj;
	GameObject[] CurvePoints = new GameObject[201];

	void Start()
	{
		for (int i = 0; i < CurvePoints.Length; ++i)
		{
			CurvePoints[i] = Instantiate(CurvePointObj);
			CurvePoints[i].transform.parent = this.transform;
		}
	}

	void Update()
	{
		for (int i = 0; i < CurvePoints.Length; ++i)
		{
			float t = (float)i / (CurvePoints.Length - 1);

			Vector3 newPointPosition = CURVE(t);
			CurvePoints[i].transform.position = newPointPosition;
		}
	}

	Vector3 CURVE(float t)
	{
		return Bezier(t);

		// return Segment(t);
	}

	Vector3 Segment(float t)
	{
		if (ControlPoints.Count < 2)
		{
			return Vector3.zero;
		}

		return (1 - t) * ControlPoints[0].transform.position + t * ControlPoints[1].transform.position;
	}

	Vector3 Bezier(float t)
	{
		int N = ControlPoints.Count;

		Vector3 result = Vector3.zero;

		for (int k = 0; k < N; ++k)
		{
			result += Bernstein(N-1, k, t) * ControlPoints[k].transform.position;
		}

		return result;
	}

	float Bernstein(int n, int k, float t)
	{
		return Combinaison(k, n) * IntPow(t, k) * IntPow(1 - t, n - k);
	}

	int Combinaison(int k, int n)
	{
		return Factorielle(n) / (Factorielle(k) * Factorielle(n - k));
	}

	int Factorielle(int n)
	{
		return Enumerable.Range(1, n).Aggregate(1, (p, item) => p * item);
	}

	float IntPow(float number, int pow)
	{
		if (pow == 0)
		{
			return 1;
		}

		if (pow == 1)
		{
			return number;
		}

		float result = number;

		for (int i = 1; i < pow; ++i)
		{
			result *= number;
		}

		return result;
	}
}
