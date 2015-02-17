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
    class OptionsForm :Form
	{
        Form optionsForm;
        Button btnClose;
        PointData[] pointData;
        ListBox listBox;
        public OptionsForm()
        {
            optionsForm=new Form();
            optionsForm.Height = 500;
            optionsForm.Width = 500;

            listBox = new ListBox();
            listBox.Text = "new Points";
            listBox.Size = (Size)new Point(200, 200);
            listBox.Location = new Point(10, 10);
            listBox.Click += new EventHandler(listBox_Click);

            optionsForm.Controls.Add(listBox);

            btnClose = new Button();
            btnClose.Text="Close";
            btnClose.Location = new Point(listBox.Location.X + listBox.Width, 10);
            btnClose.Click += new EventHandler(btnClose_Click);
            optionsForm.Controls.Add(btnClose);
           
            optionsForm.Show();
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            optionsForm.Close();
        }

        public void pointDataSet(PointData[] points)
        {
            if (points != null)
            {
            
                pointData = points;
                for (int i = 0; i < pointData.Length; i++)
                {
                    listBox.Items.Add(pointData[i].PointName);
                }
            }
            else
                MessageBox.Show("Data not load");
        }

        void listBox_Click(object sender, EventArgs e)
        {
            int index = listBox.SelectedIndex;
            MessageBox.Show("item: "+index+" Point: "+pointData[index].PointName+" maxScale "+pointData[index].MaxScale);
        }
	}

}

