using UnityEngine;

namespace Library.PhysicsEngine.Data
{
	public struct Mat2
	{
		public float m00, m01;
		public float m10, m11;
		
		public Mat2(float radians)
		{
			m00 = m01 = m10 = m11 = 0f;
			Set(radians);
		}

		public Mat2(float a, float b, float c, float d)
		{
			m00 = m01 = m10 = m11 = 0f;
			Set(a, b, c, d);
		}

		public void Set(float radians)
		{
			float c = Mathf.Cos(radians);
			float s = Mathf.Sin(radians);

			m00 = c;
			m01 = -s;
			m10 = s;
			m11 = c;
		}

		public void Set(float a, float b, float c, float d)
		{
			m00 = a;
			m01 = b;
			m10 = c;
			m11 = d;
		}

		public void Set(Mat2 m)
		{
			m00 = m.m00;
			m01 = m.m01;
			m10 = m.m10;
			m11 = m.m11;
		}

		public Mat2 Abs()
		{
			return new Mat2(Mathf.Abs(m00), Mathf.Abs(m01), Mathf.Abs(m10), Mathf.Abs(m11));
		}

		public Vector2 GetAxisX()
		{
			return new Vector2(m00, m11);
		}

		public Vector2 GetAxisY()
		{
			return new Vector2(m01, m11);
		}

		public Mat2 Transpose()
		{
			return new Mat2(m00, m10, m01, m11);
		}

		public Vector2 Multiply(Vector2 v)
		{
			return Multiply(v.x, v.y);
		}

		public Vector2 Multiply(float x, float y)
		{
			return new Vector2(m00 * x + m01 * y, m10 * x + m11 * y);
		}

		public Mat2 Multiply(Mat2 x)
		{
			return new Mat2(
				m00 * x.m00 + m01 * x.m10,
				m00 * x.m01 + m01 * x.m11,
				m10 * x.m00 + m11 * x.m10,
				m10 * x.m01 + m11 * x.m11);
		}
	}
}