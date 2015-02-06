using System;
using System.Collections.Generic;
using System.Globalization;

namespace OvationTrendLook
{
	public class PointData
	{
		String pointName, pointAlias;
		List<float> pointValue;
		float maxValue, minValue;
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
	}
}

