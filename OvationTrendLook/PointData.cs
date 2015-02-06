using System;
using System.Collections.Generic;
using System.Globalization;

namespace OvationTrendLook
{
	public class PointData
	{
		String pointName, pointAlias;
		List<float> pointValue;
		float maxValue, minValue, coeficient;
		public PointData (String poitName1)
		{
			pointValue = new List<float> ();
			pointName = poitName1;
		}

		public PointData()
		{

		}

		public void SetPointName (String name)
		{
			pointName = name;
		}

		public void AddValue(float values)
		{
			pointValue.Add (values);
		}

		public void AddValue(String str)
		{
			pointValue.Add(float.Parse(str,CultureInfo.InvariantCulture.NumberFormat));
		}

		public void SetMaxMinValue(float max, float min)
		{
			maxValue = max;
			minValue = min;
		}

		public int GetPointValueCount()
		{
			return pointValue.Count;
		}

		public float GetPointValueData(int i)
		{	
			return pointValue [i];
		}

		public void calcMaxMin()
		{
			float item = pointValue [0];
			maxValue = item;
			for (int i = 0; i < pointValue.Count; i++) 
			{
				if (maxValue < pointValue [i])
					maxValue = pointValue [i];
				if (minValue > pointValue [i])
					minValue = pointValue [i];
			}
			//maxValue += 1;
			//minValue -= 1;
		}

		public float calcCoeficient(int h)
		{
			return coeficient = h/(maxValue - minValue);
		}

	}
}

