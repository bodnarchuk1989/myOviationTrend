using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;
using Finisar.SQLite;


namespace OvationTrendLook
{
	class OvationTrendLookMainForm : Form
	{
		Graphics g;
		Panel panel1, panel2;
        List<Label> lisrOfPoints1=new List<Label>();
        StatusBar statusBar;
		Chart chart1;
		ChartArea chartArea1;
		Series[] series;
		PointData[] pointData;
        DateTime[] date;
        int cursorIndex=1;

		public OvationTrendLookMainForm()
		{

			InitializeInterfase ();
		}

		void InitializeInterfase ()
		{
			this.Text="New Window";
			this.Height = 1024/2;
			this.Width = 1024;

            addMainMenu();

            statusBar = new StatusBar();
            statusBar.Text="For start open data file......";
            this.Controls.Add(statusBar);

			panel1 = new Panel ();
			panel1.Dock = DockStyle.Fill;
			panel1.BorderStyle = BorderStyle.FixedSingle;
			panel1.BackColor = Color.Wheat;


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
            chart1.ChartAreas[0].CursorX.IsUserEnabled = false;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = false;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = false;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false;
			chart1.ChartAreas [0].CursorX.Interval = 0.0016;


			panel2 = new Panel ();
			panel2.BackColor = Color.OrangeRed;
			panel2.Dock = DockStyle.Fill;
			panel2.AutoSize = true;
			panel2.Controls.Add (chart1);


			SplitContainer splitContainer = new SplitContainer ();
			splitContainer.Orientation = Orientation.Horizontal;
			splitContainer.Dock = DockStyle.Fill;
			splitContainer.BorderStyle = BorderStyle.Fixed3D;
            splitContainer.SplitterDistance = 1;
			splitContainer.Panel1.Controls.Add (panel1);
			splitContainer.Panel2.Controls.Add (panel2);
			this.Controls.Add (splitContainer);
          
            g = chart1.CreateGraphics ();

		}

        void addMainMenu()
        {
            MainMenu mainMenu = new MainMenu();

            //Menu File
            MenuItem mFile = new MenuItem("File");
            //SubMenu File->Opne
            MenuItem mOpen = new MenuItem("Open");
            mOpen.Click += new EventHandler(btnOpen_Click);
            mFile.MenuItems.Add(mOpen);
            //SubMenu File->Draw
            MenuItem mDraw = new MenuItem("Draw");
            mDraw.Click += new EventHandler(brnDraw_Click);
            mFile.MenuItems.Add(mDraw);
            //SubMenu File->Save as image
            MenuItem mSaveToImage = new MenuItem("Seve as image");
            mSaveToImage.Click += new EventHandler(btnSaveToImage_Click);
            mFile.MenuItems.Add(mSaveToImage);
            //Add menu File to mainMenu
            mainMenu.MenuItems.Add(mFile);

            //Menu Trend
            MenuItem mTrend = new MenuItem("Trend");
            //SubMenu Trend->Options
            MenuItem mTrendOptions = new MenuItem("Options");
            mTrendOptions.Click += new EventHandler(openOptionsForm);
            mTrend.MenuItems.Add(mTrendOptions);
            //SubMenu Trend->EditPointNames
            MenuItem mTrendEditPointNames = new MenuItem("Edit point names");
            mTrend.MenuItems.Add(mTrendEditPointNames);
            //Add menu options to mainMenu
            mainMenu.MenuItems.Add(mTrend);

            this.Menu = mainMenu;
        }

		void OnMouseMove (object sender, MouseEventArgs e)
		{	
        	Point mousePositionOnPanel=new Point(e.X, e.Y);
			chart1.ChartAreas [0].CursorX.SetCursorPixelPosition (mousePositionOnPanel, true);
			chart1.ChartAreas [0].CursorY.SetCursorPixelPosition (mousePositionOnPanel, true);
			
            double pX1=0, pX = chartArea1.CursorX.Position; //X Axis Coordinate of your mouse cursor
//			float pY=0; // = chartArea1.CursorY.Position;//Y Axis Coordinate of your mouse cursor
			if ((int)(pX * 600) < 600)	pX1 = pX * 600;

            lisrOfPoints1[cursorIndex*pointData.Length].Text=""+ date [(int)pX1].ToString("dd.MM.yyyy HH:mm:ss.ffff");
            for (int i = 1; i < pointData.Length; i++)
                lisrOfPoints1[i+cursorIndex*pointData.Length].Text = ""+pointData [i].GetPointValueData ((int)pX1); 
		}


        void onDoubleClick(object sender, MouseEventArgs e)
        {
            Label newLable1 = new Label();
            newLable1.Top = 20;
            newLable1.BorderStyle = BorderStyle.Fixed3D;
            newLable1.Location = new Point(e.X,0);
            newLable1.Width = 2;
            newLable1.Height = chart1.Height;
            newLable1.BackColor = Color.Red;
            chart1.Controls.Add(newLable1);

            Label newLable2 = new Label();
            newLable2.Text = "marker: " + (cursorIndex);
            newLable2.BackColor = Color.FloralWhite;
            newLable2.Location=new Point(e.X+2,10);
            newLable2.AutoSize = true;
            newLable2.BorderStyle = BorderStyle.FixedSingle;
            chart1.Controls.Add(newLable2);
            cursorIndex++;
            addNewLables((cursorIndex+1)*200);
        }


		void btnOpen_Click (object sender, EventArgs e)
		{
            statusBar.Text = "Opening file.";
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
                    this.Text="File "+str;
				}

			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
			}
            statusBar.Text="File open";
		}



		void readDataFromFile (string str)
		{
            statusBar.Text = "Reading file";
			StreamReader file = new StreamReader (str);
			string[] lines = File.ReadAllLines (str, System.Text.Encoding.Unicode);

			string[] strName = lines [0].Split ('\t');
			//pointData[strName.Length] = new PointData();
			pointData = new PointData[strName.Length];
			for (int i = 0; i < pointData.Length; i++)
			{
				pointData[i]=new PointData(strName[i]);
                pointData[i].PointAlias = getPointAliasFromDB(pointData[i].PointName);
			}

            date = new DateTime[lines.Length - 1];
			for (int i = 1; i < lines.Length; i++) 
			{
				String[] pointValue = lines [i].Split ('\t');
                pointValue[0]=pointValue[0].Trim ();
                //date[i - 1] = DateTime.Parse(pointValue[0]);
                switch (pointValue[0].Length)
                {
                    case 21:
                        date[i - 1] = DateTime.ParseExact(pointValue[0], "dd.MM.yyyy HH:mm:ss.f", System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case 22:
                        date[i - 1] = DateTime.ParseExact(pointValue[0], "dd.MM.yyyy HH:mm:ss.ff", System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case 23:
                        date[i - 1] = DateTime.ParseExact(pointValue[0], "dd.MM.yyyy HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case 24:
                        date[i - 1] = DateTime.ParseExact(pointValue[0], "dd.MM.yyyy HH:mm:ss.ffff", System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    default:
                        break;
                }

                //date[i - 1] = DateTime.ParseExact(pointValue[0], "dd.MM.yyyy HH:mm:ss.f", System.Globalization.CultureInfo.InvariantCulture); 
				for (int j = 1; j < pointValue.Length; j++)
				{
					pointValue[j]=pointValue[j].Trim ();
					float f = float.Parse (pointValue[j], CultureInfo.InvariantCulture.NumberFormat);
					pointData [j].AddValue (f);
				}
               

			}
            statusBar.Text="Data was read. Count is "+ pointData.Length+" "+pointData[1].GetPointValueCount();
			//MessageBox.Show (string.Format ("Data was read. Count is "+ pointData.Length+" "+pointData[1].GetPointValueCount()));

		}

        string getPointAliasFromDB(string pointName)
        {
            SQLiteConnection sql_con = new SQLiteConnection(@"Data Source=testDataBase1.db;Version=3;New=False;");
            sql_con.Open();
            SQLiteCommand sql_comand = sql_con.CreateCommand();
            string str = pointName.Replace(".UNIT@UNIT1","");
            string cmd=@"SELECT OvationDescription FROM UNIT1 WHERE ovationPointName LIKE '"+str+@"'";
            sql_comand.CommandText = cmd;

            string tmp=(string)sql_comand.ExecuteScalar();

            sql_con.Close();
            return tmp;
        }

		void brnDraw_Click (object sender, EventArgs e)
		{
			if (pointData != null) {
                statusBar.Text = "Drawing chart";
                MenuItem menu1 = (MenuItem)sender;
                menu1.Enabled = false;
				// Initialize series of chart1 for each point
                initializeCahrtSeries ();

				// Set custom chart area position
				chart1.ChartAreas["Default"].Position = new ElementPosition(5,5,90,90);
				chart1.ChartAreas["Default"].InnerPlotPosition = new ElementPosition(0,0,100,100);
               // chart1.ChartAreas["Default"].AxisX=
				// Create extra Y axis for second and third series
				for (int i = 1; i < pointData.Length; i++) {
					chart1.Series ["Series"+i].Points.DataBindY (pointData [i].getPoitDataValue ());
					CreateYAxis(chart1,chart1.ChartAreas["Default"], chart1.Series["Series"+i], 2*i,2,pointData[i]);
				}

				
               
                for (int i = 0; i < pointData.Length; i++)
                {
                    Label tmpLable = new Label();
                    tmpLable.Location = new Point (5, (i * 15));
                    tmpLable.AutoSize = true;
                    tmpLable.ForeColor = pointData[i].ColorPoint;
                    tmpLable.Text = pointData [i].PointName+" "+pointData[i].PointAlias;
                    panel1.Controls.Add (tmpLable);
                    lisrOfPoints1.Add(tmpLable);
                }
                addNewLables(400);

                chart1.MouseMove += new MouseEventHandler (OnMouseMove); //MouseMove event on Chart1, Panel2 will work after clicked btnDraw
                chart1.MouseDoubleClick += new MouseEventHandler(onDoubleClick);
                statusBar.Text = ".....";
			} else
				MessageBox.Show ("Data was not load");

		}

        void addNewLables(int shift)
        {
            for (int i = 0; i < pointData.Length; i++)
            {
                Label tmpLable = new Label();
                tmpLable.Location = new Point(shift, (i * 15));
                tmpLable.AutoSize = true;
                tmpLable.ForeColor = pointData[i].ColorPoint;
                tmpLable.Text = "0";
                panel1.Controls.Add(tmpLable);
                lisrOfPoints1.Add(tmpLable);
            }

        }			

		void initializeCahrtSeries ()
		{
            Color[] colors = {Color.Red,Color.Blue,Color.Orange, Color.Green, Color.Brown, Color.Cyan, Color.Magenta }; 
			int colorIndex = 0;
			series = new Series[pointData.Length];

			for (int i = 1; i < series.Length; i++) 
			{
				series [i] = new Series ();
				series [i].Name = "Series" + i;
				series [i].BorderColor = colors [colorIndex];
                pointData[i].ColorPoint = colors[colorIndex];
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
            areaSeries.AxisX.MajorTickMark.Enabled = true;
			areaSeries.AxisX.LabelStyle.Enabled = true;
			areaSeries.AxisY.MajorGrid.Enabled = false;
			areaSeries.AxisY.MajorTickMark.Enabled = false;
			areaSeries.AxisY.LabelStyle.Enabled = false;
			areaSeries.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;
            if ((pointData1.MaxScale - pointData1.MaxScale) != 0)
            {
                //pointData1.calcMaxMin ();
                areaSeries.AxisY.Maximum = pointData1.MaxScale;
                areaSeries.AxisY.Minimum = pointData1.MinScale;
            }

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
            seriesCopy.Color = pointData1.ColorPoint; //Color.Transparent  Change color For series
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

        void btnSaveToImage_Click(object sender, EventArgs e)
        {
            statusBar.Text = "Save file";
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.InitialDirectory = @"C:\";
            saveDialog.Title = "Save image";
                         
            string name = @"C:\test";
            if (saveDialog.ShowDialog() == DialogResult.OK)
                name = saveDialog.FileName+".png";
            Bitmap bmp = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bmp,new Rectangle(0,0,this.Width,this.Height));
            bmp.Save(name);
            statusBar.Text = "File was save "+name;
        }

        //For send data to form OptionsForm
        void openOptionsForm(object sender, EventArgs e)
        {
            OptionsForm optionsForm = new OptionsForm(this); 
            optionsForm.pointDataSet(pointData);
        }

        //For get data from form OptionsForm
        public void form1PointDataSet(PointData[] p)
        {
            pointData = p;
        }
	}
}

