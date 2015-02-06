using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections;

namespace OvationTrendLook
{
	class OvationTrendLookMainForm : Form
	{
		Graphics g;
		TableLayoutPanel tabeleLayout;
		Panel panel1, panel2;
		Button btn1;
		Label labelX;
		Point savePrevioseLocation=new Point();
		ArrayList dataList=new ArrayList();
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
//			btn1.Click += delegate {
//				onButtonClicked ();
//			};
			btn1.Click += new EventHandler (button1_Click);

			labelX = new Label ();
			labelX.Location = new Point (5, 25);
			labelX.Text = "X: ";

			panel1 = new Panel ();
			panel1.Dock = DockStyle.Bottom;
			panel1.BorderStyle = BorderStyle.FixedSingle;
			panel1.BackColor = Color.Wheat;
			panel1.Controls.Add (btn1);
			panel1.Controls.Add (labelX);

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
			//tabeleLayout.SetRow (panel1, 1);
			tabeleLayout.Controls.Add (panel1);
			tabeleLayout.Controls.Add (panel2);
			//g=panel2.CreateGraphics ();
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
			//string[] str1 = lines [0].Split (new Char[]{'\t'});

			for (int i = 0; i < lines.Length; i++) 
			{
				dataList.Add((object)lines[i].Split(new Char[]{'\t'}));
			}

			MessageBox.Show ("Data was read. Count is "+lines.Length+" "+dataList.Capacity);

		}
	}

}

