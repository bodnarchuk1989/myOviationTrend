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
        OvationTrendLookMainForm ovationTrendLookMainForm;
        Button btnClose, btnOK, btnSave;
        PointData[] pointData;
        ListBox listBox;
        Label labelMaxScale, labelMinScale, labelColor;
        TextBox tboxMaxScale, tboxMinScale, tboxColor;
        public OptionsForm(OvationTrendLookMainForm f)
        {
            ovationTrendLookMainForm = f;

            optionsForm=new Form();
            optionsForm.Height = 500;
            optionsForm.Width = 500;

            listBox = new ListBox();
            listBox.Text = "new Points";
            listBox.Size = (Size)new Point(200, 250);
            listBox.HorizontalScrollbar = true;
            listBox.Location = new Point(10, 10);
            listBox.Click += new EventHandler(listBox_Click);
            optionsForm.Controls.Add(listBox);

            int locationX = listBox.Width + 20;
           
            labelMaxScale = new Label();
            labelMaxScale.Text = "Max Scale";
            labelMaxScale.AutoSize = true;
            labelMaxScale.Location = new Point(locationX,10);
            optionsForm.Controls.Add(labelMaxScale);

            tboxMaxScale = new TextBox();
            tboxMaxScale.Location = new Point(locationX,25);
            optionsForm.Controls.Add(tboxMaxScale);

            labelMinScale = new Label();
            labelMinScale.Text = "Mi Scale";
            labelMinScale.AutoSize = true;
            labelMinScale.Location = new Point(locationX,60);
            optionsForm.Controls.Add(labelMinScale);

            tboxMinScale = new TextBox();
            tboxMinScale.Location = new Point(locationX,75);
            optionsForm.Controls.Add(tboxMinScale);

            labelColor = new Label();
            labelColor.Text = "Color";
            labelColor.AutoSize = true;
            labelColor.Location = new Point(locationX,100);
            optionsForm.Controls.Add(labelColor);

            tboxColor = new TextBox();
            tboxColor.Location = new Point(locationX,115);
            optionsForm.Controls.Add(tboxColor);

            btnClose = new Button();
            btnClose.Text="Close";
            btnClose.Location = new Point(optionsForm.Width-btnClose.Width-30, 10);
            btnClose.Click += new EventHandler(btnClose_Click);
            optionsForm.Controls.Add(btnClose);

            btnOK = new Button();
            btnOK.Text="OK";
            btnOK.Location = new Point(optionsForm.Width-btnOK.Width-30, 35);
            btnOK.Click += new EventHandler(btnOK_Click);
            optionsForm.Controls.Add(btnOK);

            btnSave = new Button();
            btnSave.Text="Save";
            btnSave.Location = new Point(optionsForm.Width-btnSave.Width-30, 60);
            btnSave.Click += new EventHandler(btnSave_Click);
            optionsForm.Controls.Add(btnSave);
           
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
                for (int i = 1; i < pointData.Length; i++)
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
            tboxMaxScale.Text = pointData[index].MaxScale.ToString();
            tboxMinScale.Text = pointData[index].MinScale.ToString();
            tboxColor.Text = pointData[index].ColorPoint.ToString();
            //MessageBox.Show("item: "+index+" Point: "+pointData[index].PointName+" maxScale "+pointData[index].MaxScale);
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            int index = listBox.SelectedIndex;
            pointData[index].MaxScale = float.Parse(tboxMaxScale.Text.Trim());
            pointData[index].MinScale = float.Parse(tboxMinScale.Text.Trim());
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            ovationTrendLookMainForm.form1PointDataSet(pointData);
           
        }
	}

}

