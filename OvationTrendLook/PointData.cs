using System;
using System.Collections.Generic;
using System.Globalization;

namespace OvationTrendLook
{
	public class PointData
	{
		String pointName, pointAlias;
		private List<float> pointValue;
		float maxValue, minValue, coeficient;
		public PointData (String poitName1)
		{
			pointValue = new List<float> ();
			pointName = poitName1;
		}

		public PointData()
		{

		}

		public string PointName
		{
			set { pointName = value; }
			get { return pointName; }
		}

		public void AddValue(float values)
		{
			pointValue.Add (values);
		}

		public void AddValue(String str)
		{
			pointValue.Add(float.Parse(str,CultureInfo.InvariantCulture.NumberFormat));
		}

		public void SetMaxMinValue(float min, float max)
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
			maxValue = pointValue [0];
			//minValue=pointValue [0];
			for (int i = 0; i < pointValue.Count; i++) 
			{
				if (maxValue < pointValue [i])
					maxValue = pointValue [i];
				if (minValue > pointValue [i])
					minValue = pointValue [i];
			}
		}

		public float calcCoeficient(int h)
		{
			return coeficient = h/(maxValue - minValue);
		}

		public float getScaleShift()
		{
			return coeficient * maxValue;
		}

		public List<float> getPoitDataValue()
		{
			return pointValue;
		}

		public float MaxScale
		{
			get {return maxValue;}
			set { maxValue = value;}
		}

		public float MinScale
		{
			get {return minValue;}
			set { minValue = value;}
		}
	}
}

