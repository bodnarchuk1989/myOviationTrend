using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;


namespace OvationTrendLook
{
	class OvationTrendLookMainForm : Form
	{
		Graphics g;
		TableLayoutPanel tabeleLayout;
		Panel panel1, panel2;
		Button btn1, btnDraw;
		Label labelTime, labelValue;

		Chart chart1;
		ChartArea chartArea1;
		Series[] series;

		Point savePrevioseLocation=new Point();
		//ArrayList dataList=new ArrayList();
		PointData[] pointData;
		int[] date;

		public OvationTrendLookMainForm()
		{

			InitializeInterfase ();
		}

		void InitializeInterfase ()
		{
			this.Text="Ovation Trend Look";
			this.Height = 1024/2;
			this.Width = 1024;


			btn1 = new Button ();
			btn1.Text = "Open";
			btn1.FlatStyle = FlatStyle.System;
			btn1.TabIndex = 0;
			btn1.Click += new EventHandler (button1_Click);

			btnDraw = new Button ();
			btnDraw.Text="Draw";
			btnDraw.FlatStyle = FlatStyle.System;
			btnDraw.Location = new Point (btn1.Width+15,0);
			btnDraw.TabIndex = 1;
			btnDraw.Click += new EventHandler (brnDraw_Click);

			labelTime = new Label ();
			labelTime.Location = new Point (3, 25);
			labelTime.Text = "X: ";

			labelValue = new Label ();
			labelValue.Location = new Point (3, 45);
			labelValue.Text = "V: ";

			panel1 = new Panel ();
			panel1.Dock = DockStyle.Bottom;
			panel1.BorderStyle = BorderStyle.FixedSingle;
			panel1.BackColor = Color.Wheat;
			panel1.Controls.Add (btn1);
			panel1.Controls.Add (labelTime);
			panel1.Controls.Add (btnDraw);
			panel1.Controls.Add (labelValue);


			chart1 = new Chart ();
			chart1.Dock = DockStyle.Fill;
			chart1.Name="MyltyChart";
			chart1.BeginInit ();
			chartArea1 = new ChartArea ();
			chartArea1.Name="Default";
			chartArea1.AxisX.LabelStyle.Format = "hh:mm";
			chartArea1.AxisX.MajorGrid.LineColor = Color.LightGray;
			chartArea1.AxisY.MajorGrid.LineColor = Color.LightGray;
			//chartArea1.BackColor = Color.Black;
			chart1.ChartAreas.Add (chartArea1);
			chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
			chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
			chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
			chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
			chart1.ChartAreas [0].CursorX.Interval = 0.0016;
			//chart1.ChartAreas [0].CursorX.LineColor = Color.Black;


			//chart1.ChartAreas [0].CursorX.IntervalType = (DateTimeIntervalType)IntervalType.Number;
			chart1.MouseMove += new MouseEventHandler (OnMouseMove);
		


			panel2 = new Panel ();
			panel2.BackColor = Color.OrangeRed;
			panel2.Dock = DockStyle.Fill;
			panel2.AutoSize = true;
			//panel2.SuspendLayout();
			panel2.Controls.Add (chart1);
			//panel2.MouseMove += delegate {
		    //		onMouseMove ();
			//};


			tabeleLayout = new TableLayoutPanel ();
			tabeleLayout.ColumnCount = 1;
			tabeleLayout.RowCount = 2;
			tabeleLayout.Dock = DockStyle.Fill;
			tabeleLayout.AutoSize = true;
			tabeleLayout.Controls.Add (panel1);
			tabeleLayout.Controls.Add (panel2);
			this.Controls.Add (tabeleLayout);

			this.Resize += delegate {
				formSizeChanged ();
			};

			g = panel2.CreateGraphics ();
		}

		void OnMouseMove (object sender, MouseEventArgs e)
		{	
			if (pointData != null)
			{
			Point mousePositionOnPanel=new Point(e.X, e.Y);
			chart1.ChartAreas [0].CursorX.SetCursorPixelPosition (mousePositionOnPanel, true);
			chart1.ChartAreas [0].CursorY.SetCursorPixelPosition (mousePositionOnPanel, true);

			double pX = chartArea1.CursorX.Position; //X Axis Coordinate of your mouse cursor
			float pY=0;// = chartArea1.CursorY.Position;//Y Axis Coordinate of your mouse cursor
			if ((int)(pX*600)<600)
			pY = pointData [1].GetPointValueData ((int)(pX * 600));
			labelTime.Width = 200;
			labelTime.Text = pointData[1].PointName+":"+pY;
			pY = pointData [2].GetPointValueData ((int)(pX * 600));
			labelValue.Width = 200;
			labelValue.Text = pointData[2].PointName+":"+pY;
			//drawLine (g, mousePositionOnPanel);
			}
		}

		void drawLine (Graphics g1, Point mousePositionOnPanel)
		{
			g1.Clear (Color.Black);
			Pen myPen = new Pen (Color.Red,1);
			g1.DrawLine(myPen,new Point(mousePositionOnPanel.X,0),new Point(mousePositionOnPanel.X,1024));
			savePrevioseLocation = mousePositionOnPanel;

		}

		void formSizeChanged ()
		{
			g = panel2.CreateGraphics ();
		}


		void button1_Click (object sender, EventArgs e)
		{
			//Stream myStream = null;
			OpenFileDialog openFileDialog1 = new OpenFileDialog();
			openFileDialog1.InitialDirectory = "c:\\" ;
			openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*" ;
			openFileDialog1.FilterIndex = 2 ;
			openFileDialog1.RestoreDirectory = true ;
			try
			{
				if(openFileDialog1.ShowDialog()== DialogResult.OK)
				{
					String str=openFileDialog1.FileName;
					readDataFromFile(str);
				}

			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
			}
		}



		void readDataFromFile (string str)
		{
			StreamReader file = new StreamReader (str);
			string[] lines = File.ReadAllLines (str, System.Text.Encoding.Unicode);

			string[] strName = lines [0].Split ('\t');
			//pointData[strName.Length] = new PointData();
			pointData = new PointData[strName.Length];
			for (int i = 0; i < pointData.Length; i++)
			{
				pointData[i]=new PointData(strName[i]);
			}

			date = new int[lines.Length - 1];
			for (int i = 1; i < lines.Length; i++) 
			{
				String[] pointValue = lines [i].Split ('\t');
				//date [i - 1] = DateTime.ParseExact ("17.07.2013 13:47:30.1000", "dd.MM.yyyy HH:mm:ss,ffff", System.Globalization.CultureInfo.InvariantCulture);
				date [i - 1] = i;
				for (int j = 1; j < pointValue.Length; j++)
				{
					pointValue[j]=pointValue[j].Trim ();
					float f = float.Parse (pointValue[j], CultureInfo.InvariantCulture.NumberFormat);
					pointData [j].AddValue (f);
				}
			}

			MessageBox.Show (string.Format ("Data was read. Count is "+ pointData.Length+" "+pointData[1].GetPointValueCount()));

		}

		void brnDraw_Click (object sender, EventArgs e)
		{
			bool drawpress = false;

//			series = null;
//			chart1.Series.Clear ();
//			if (drawpress)
//				chart1.ChartAreas.Clear;

			if (pointData != null & !drawpress) {

				drawpress = true;
				initializeCahrtSeries ();

				// Set custom chart area position
				chart1.ChartAreas["Default"].Position = new ElementPosition(5,5,90,90);
				chart1.ChartAreas["Default"].InnerPlotPosition = new ElementPosition(0,0,100,100);
				// Create extra Y axis for second and third series
				for (int i = 1; i < pointData.Length; i++) {
					chart1.Series ["Series"+i].Points.DataBindY (pointData [i].getPoitDataValue ());
					CreateYAxis(chart1,chart1.ChartAreas["Default"], chart1.Series["Series"+i], 2*i,2,pointData[i]);
				}
				Label[] listOfPoints = new Label[pointData.Length];
				for (int i=0; i<listOfPoints.Length;i++)
				{
					listOfPoints [i] = new Label ();
					listOfPoints[i].Text=pointData[i].PointName;
					listOfPoints [i].Location=new Point(300, (i * 15));
					//listOfPoints [i].Width = 90;
					listOfPoints [i].AutoSize = true;
					panel1.Controls.Add (listOfPoints [i]);
				}

			} else
				MessageBox.Show ("Data was not load");

		}

		void initializeCahrtSeries ()
		{
			Color[] colors = {Color.Red,Color.Blue,Color.White, Color.Green, Color.Brown, Color.Cyan, Color.Magenta }; 
			int colorIndex = 0;
			series = new Series[pointData.Length];

			for (int i = 0; i < series.Length; i++) 
			{
				series [i] = new Series ();
				series [i].Name = "Series" + i;
				series [i].BorderColor = colors [colorIndex];
				series [i].BorderWidth = 1;
				//series [i].ChartArea = "Default";
				series [i].ChartType = SeriesChartType.Line;
				//series [i].Legend = "Default";
				chart1.Series.Add (series [i]);

				if (colorIndex < colors.Length-1)
					colorIndex++; else colorIndex=0;

			}
		}

		public void CreateYAxis(Chart chart, ChartArea area, Series series, float axisOffset, float labelsSize, PointData pointData1)
		{
			// Create new chart area for original series
			ChartArea areaSeries = chart.ChartAreas.Add("ChartArea_" + series.Name);
			areaSeries.BackColor = Color.Transparent;
			areaSeries.BorderColor = Color.Transparent;
			areaSeries.Position.FromRectangleF(area.Position.ToRectangleF());
			areaSeries.InnerPlotPosition.FromRectangleF(area.InnerPlotPosition.ToRectangleF());
			areaSeries.AxisX.MajorGrid.Enabled = false;
			areaSeries.AxisX.MajorTickMark.Enabled = false;
			areaSeries.AxisX.LabelStyle.Enabled = false;
			areaSeries.AxisY.MajorGrid.Enabled = false;
			areaSeries.AxisY.MajorTickMark.Enabled = false;
			areaSeries.AxisY.LabelStyle.Enabled = false;
			areaSeries.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;
			pointData1.calcMaxMin ();
			//areaSeries.AxisY.Maximum = pointData1.MaxScale;
			//areaSeries.AxisY.Minimum = pointData1.MinScale;


			series.ChartArea = areaSeries.Name;

			// Create new chart area for axis
			ChartArea areaAxis = chart.ChartAreas.Add("AxisY_" + series.ChartArea);
			areaAxis.BackColor = Color.Transparent;
			areaAxis.BorderColor = Color.Transparent;
			areaAxis.Position.FromRectangleF(chart.ChartAreas[series.ChartArea].Position.ToRectangleF());
			areaAxis.InnerPlotPosition.FromRectangleF(chart.ChartAreas[series.ChartArea].InnerPlotPosition.ToRectangleF());

			// Create a copy of specified series
			Series seriesCopy = chart.Series.Add(series.Name + "_Copy");
			seriesCopy.ChartType = series.ChartType;
			foreach(DataPoint point in series.Points)
			{
				seriesCopy.Points.AddXY(point.XValue, point.YValues[0]);
			}

			// Hide copied series
			seriesCopy.IsVisibleInLegend = false;
			seriesCopy.Color = Color.Transparent;
			seriesCopy.BorderColor = Color.Transparent;
			seriesCopy.ChartArea = areaAxis.Name;

			// Disable drid lines & tickmarks
			areaAxis.AxisX.LineWidth = 0;
			areaAxis.AxisX.MajorGrid.Enabled = false;
			areaAxis.AxisX.MajorTickMark.Enabled = false;
			areaAxis.AxisX.LabelStyle.Enabled = false;
			areaAxis.AxisY.MajorGrid.Enabled = false;
			areaAxis.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;
			areaAxis.AxisY.LabelStyle.Font = area.AxisY.LabelStyle.Font;

			// Adjust area position
			//areaAxis.Position.X -= axisOffset;
			//areaAxis.InnerPlotPosition.X += labelsSize;

		}
	}
}

