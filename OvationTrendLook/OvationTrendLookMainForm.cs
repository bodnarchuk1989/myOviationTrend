using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Drawing.Drawing2D;

namespace OvationTrendLook
{
	class OvationTrendLookMainForm : Form
	{
		Graphics g;
		TableLayoutPanel tabeleLayout;
		Panel panel1, panel2;
		Button btn1, btnDraw;
		Label labelX;
		Point savePrevioseLocation=new Point();
		ArrayList dataList=new ArrayList();
		PointData[] pointData;
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
			btn1.Click += new EventHandler (button1_Click);

			btnDraw = new Button ();
			btnDraw.Text="Draw";
			btnDraw.FlatStyle = FlatStyle.System;
			btnDraw.Location = new Point (btn1.Width+15,0);
			btnDraw.Click += new EventHandler (brnDraw_Click);

			labelX = new Label ();
			labelX.Location = new Point (3, 25);
			labelX.Text = "X: ";

			panel1 = new Panel ();
			panel1.Dock = DockStyle.Bottom;
			panel1.BorderStyle = BorderStyle.FixedSingle;
			panel1.BackColor = Color.Wheat;
			panel1.Controls.Add (btn1);
			panel1.Controls.Add (labelX);
			panel1.Controls.Add (btnDraw);

			panel2 = new Panel ();
			panel2.BackColor = Color.OrangeRed;
			panel2.Dock = DockStyle.Fill;
			panel2.AutoSize = true;
			panel2.MouseMove += delegate {
				onMouseMove ();
			};


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

		void onMouseMove ()
		{	
			Point mousePositionOnPanel;
			mousePositionOnPanel = tabeleLayout.PointToClient (Cursor.Position);
			labelX.Text = "X Y"+mousePositionOnPanel;
			drawLine (g, mousePositionOnPanel);
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
			string[] lines = File.ReadAllLines (str);

			string[] strName = lines [0].Split (new Char[]{ '\t' });
			//pointData[strName.Length] = new PointData();
			pointData = new PointData[strName.Length];
			for (int i = 0; i < pointData.Length; i++)
			{
				pointData[i]=new PointData(strName[i]);
	
			}

			for (int i = 1; i < lines.Length; i++) 
			{
				String[] pointValue = lines [i].Split (new Char[]{ '\t' });
				for (int j = 1; j < pointValue.Length; j++)
				{
					float f = float.Parse (pointValue[j], CultureInfo.InvariantCulture.NumberFormat);

					pointData [j].AddValue (f);
				}
			}

			MessageBox.Show (string.Format ("Data was read. Count is "+ pointData.Length+" "+pointData[1].GetPointValueCount()));

		}

		void brnDraw_Click (object sender, EventArgs e)
		{
			Graphics g2;
			g2=panel2.CreateGraphics();
			g2.TranslateTransform (20F, 100F);

			Color[] colors = new Color[]{Color.Red,Color.Blue,Color.White, Color.Green, Color.Brown, Color.Cyan, Color.Magenta }; 
			Pen pen1 = new Pen (Color.Red);

			float coef = 2.1F;
			int colorIndex = 0;
			float shiftZero = panel2.Height/2;
			for (int i = 1; i < pointData.Length; i++) 
			{
				pen1.Color=colors[colorIndex];
				if (colorIndex < colors.Length-1)
					colorIndex++; else colorIndex=0;
				pointData [i].calcMaxMin ();
				coef = pointData [i].calcCoeficient (panel2.Height);
				for (int j = 0; j < pointData [1].GetPointValueCount () - 1; j++)
				{

					g2.DrawLine (pen1, new PointF (j, pointData [i].GetPointValueData (j)*coef - shiftZero), new PointF (j + 1, pointData [i].GetPointValueData (j + 1)*coef - shiftZero));
				}
			}

		}
	}

}

