using System;
using System.Windows.Forms;
using System.Drawing;
using Finisar.SQLite;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace OvationTrendLook
{
    public class ImportToDBForm:Form
    {
        Form importToDbForm;
        string fileName;
        string saveFolder;
        TextBox tFileName, tSaveFolder;
        ProgressBar progress;
        public List<OvationPoint> data=new List<OvationPoint>();

        public ImportToDBForm()
        {
           // importToDbForm.Size = new Size(100,200);
            importToDbForm = new Form();
            importToDbForm.Width = 550;
            importToDbForm.Height = 200;


            Label lFileName = new Label();
            lFileName.Text = "Select file for import";
            lFileName.Location = new Point(10, 10);
            importToDbForm.Controls.Add(lFileName);
         
            tFileName = new TextBox();
            tFileName.Width = 200;
            tFileName.Location = new Point(lFileName.Width+10,10);
            importToDbForm.Controls.Add(tFileName);

            Button btnSelectFile = new Button();
            btnSelectFile.Text = "Select file...";
            btnSelectFile.Location = new Point(350, 10);
            btnSelectFile.Click += new EventHandler(btnSelectFile_Click);
            importToDbForm.Controls.Add(btnSelectFile);

            Label lSaveFolder = new Label();
            lSaveFolder.Text = "Import folder";
            lSaveFolder.Location = new Point(10, 40);
            importToDbForm.Controls.Add(lSaveFolder);

            tSaveFolder = new TextBox();
            tSaveFolder.Width = 200;
            tSaveFolder.Location = new Point(lSaveFolder.Width+10,40);
            importToDbForm.Controls.Add(tSaveFolder);

            Button btnSelectSaveFolder = new Button();
            btnSelectSaveFolder.Text = "Select";
            btnSelectSaveFolder.Location = new Point(350, 40);
            btnSelectSaveFolder.Click += new EventHandler(btnSelectSaveFolder_Click);
            importToDbForm.Controls.Add(btnSelectSaveFolder);

            Button btnStartImport = new Button();
            btnStartImport.Text = "Import";
            btnStartImport.Location = new Point(importToDbForm.Width/2,80);
            btnStartImport.Click += new EventHandler(dtnStartImport_Click);
            importToDbForm.Controls.Add(btnStartImport);

            progress = new ProgressBar();
            progress.Minimum = 0;
            progress.Maximum = 200;
            progress.Width = 480;
            progress.Location = new Point(10, 130);
            progress.Style = ProgressBarStyle.Blocks;
            progress.Hide();
            importToDbForm.Controls.Add(progress);

            importToDbForm.Show();
        }


        void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = @"C:\";
            openFile.Title = "Select file for import";
            try
            {
                if (openFile.ShowDialog()==DialogResult.OK)
                {
                    fileName=openFile.FileName;
                    tFileName.Text=fileName;
                }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }


        void btnSelectSaveFolder_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory=@"C:\";
            saveFileDialog.Title="select file for save";
            try
            {
                if (saveFileDialog.ShowDialog()==DialogResult.OK)
                    saveFolder=saveFileDialog.FileName;
                tSaveFolder.Text=saveFolder;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void dtnStartImport_Click(object sender, EventArgs e)
        {
            progress.Show();

            string[] buffer;
            //Console.WriteLine ("Start reading file: "+fileName);

            buffer = File.ReadAllLines (fileName, System.Text.Encoding.Default);
            int bufferLength = buffer.Length;
            int bufferIndex = 0;
            int analogPointCount=0, digitalPointCount=0;

            SQLiteConnection conn = new SQLiteConnection ("Data Source="+saveFolder+"; Version=3; New=True");
            SQLiteCommand cmd = conn.CreateCommand ();
            string sql_command = "CREATE TABLE pointBase (id INTEGER PRIMARY KEY UNIQUE, " +
                "PointName CHAR (15)," +
                "PointAlias CHAR (80)," +
                "Description CHAR(80)," +
                "IO_location CHAR(10)," +
                "IO_chanel CHAR(2)," +
                "MAX_SCALE REAL," +
                "MIN_SCALE REAL );";
            cmd.CommandText = sql_command;
            conn.Open ();
            cmd.ExecuteNonQuery ();


            while (bufferIndex < bufferLength) 
            {
                if (buffer [bufferIndex].Contains ("TYPE=\"AnalogPoint\"")) {
                    //Console.WriteLine ("Analog point");
                    bufferIndex=readAnalogPointStructure (buffer,bufferIndex);
                    analogPointCount++;
                }
                if (buffer [bufferIndex].Contains ("TYPE=\"DigitalPoint\"")) {
                    //Console.WriteLine ("Digital point");
                    bufferIndex=readDigitalPointStructure (buffer,bufferIndex);
                    digitalPointCount++;
                }


                bufferIndex++;
            }

            progress.Maximum = data.Count;

            for(int i=0; i<data.Count; i++)
            {
                sql_command = "INSERT INTO pointBase (PointName, PointAlias,Description,IO_location,IO_chanel,MAX_SCALE,MIN_SCALE) " +
                    "VALUES(\'"+data[i].PoinName+"\'," +
                    "\'"+data[i].PoinAlias+"\'," +
                    "\'"+data[i].DESCRIPTION+"\'," +
                    "\'"+data[i].IO_location+"\',"+
                    "\'"+data[i].IO_chanel+"\'," +
                    "\'"+data[i].maxValue+"\'," +
                    "\'"+data[i].minValue+"\');";
                cmd.CommandText = sql_command;
                cmd.ExecuteNonQuery ();
                //Console.CursorLeft = 1;
                int persent = i * 100 / data.Count;
                //Console.Write (string.Format ("{0}/{1}\t{2}%", i, data.Count, persent));
                progress.Increment(1);
            }

            //Console.WriteLine ("\n\n Analog points "+analogPointCount+" | digital points "+digitalPointCount);
            conn.Close ();
            //Console.ReadLine ();
           
        }

        int readDigitalPointStructure (string[] buffer, int index)
        {
            OvationPoint newPoint = new OvationPoint ();
            while (!buffer [index].Contains("]")) 
            {
                if (buffer [index].Contains ("(TYPE=\"DigitalPoint\" NAME=\""))
                {
                    newPoint.PoinName = trimBuffer (buffer [index], "(TYPE=\"DigitalPoint\" NAME=\"");
                }
                if (buffer [index].Contains ("POINT_ALIAS=\""))
                {
                    newPoint.PoinAlias = trimBuffer (buffer [index], "POINT_ALIAS=\"");
                }
                if (buffer [index].Contains ("IO_LOCATION=\""))
                {
                    newPoint.IO_location = trimBuffer (buffer [index], "IO_LOCATION=\"");
                }
                if (buffer [index].Contains ("[DESCRIPTION=\""))
                {
                    newPoint.DESCRIPTION = trimBuffer (buffer [index], "[DESCRIPTION=\"");
                }
                if (buffer [index].Contains ("IO_CHANNEL=\""))
                {
                    newPoint.IO_chanel = trimBuffer (buffer [index], "IO_CHANNEL=\"");
                }
                newPoint.minValue = -1;
                newPoint.maxValue = 3;
                index++;

            }
            data.Add (newPoint);
            return index;
        }

        int readAnalogPointStructure (string[] buffer, int index)
        {
            OvationPoint newPoint = new OvationPoint ();
            while (!buffer [index].Contains("]")) 
            {
                if (buffer [index].Contains ("(TYPE=\"DigitalPoint\" NAME=\""))
                {
                    newPoint.PoinName = trimBuffer (buffer [index], "(TYPE=\"DigitalPoint\" NAME=\"");
                }
                if (buffer [index].Contains ("POINT_ALIAS=\""))
                {
                    newPoint.PoinAlias = trimBuffer (buffer [index], "POINT_ALIAS=\"");
                }
                if (buffer [index].Contains ("IO_LOCATION=\""))
                {
                    newPoint.IO_location = trimBuffer (buffer [index], "IO_LOCATION=\"");
                }
                if (buffer [index].Contains ("[DESCRIPTION=\""))
                {
                    newPoint.DESCRIPTION = trimBuffer (buffer [index], "[DESCRIPTION=\"");
                }
                if (buffer [index].Contains ("IO_CHANNEL=\""))
                {
                    newPoint.IO_chanel = trimBuffer (buffer [index], "IO_CHANNEL=\"");
                }
                if (buffer [index].Contains ("MAXIMUM_SCALE=\""))
                {
                    //newPoint.maxValue =float.Parse(trimBuffer (buffer [index], "MAXIMUM_SCALE=\""),new CultureInfo("en-US"));
                    buffer[index]=buffer[index].Replace(".",",");
                    newPoint.maxValue =float.Parse(trimBuffer (buffer [index], "MAXIMUM_SCALE=\""));
                }
                if (buffer [index].Contains ("MINIMUM_SCALE=\""))
                {
                    //newPoint.minValue =float.Parse(trimBuffer (buffer [index], "MINIMUM_SCALE=\""),new CultureInfo("en-US"));
                    buffer[index]=buffer[index].Replace(".",",");
                    newPoint.minValue =float.Parse(trimBuffer (buffer [index], "MINIMUM_SCALE=\""));
                }
                index++;

            }
            data.Add (newPoint);
            return index;
        }

        string trimBuffer (string str, string replace)
        {
            str =str.Trim();
            str=str .Replace (replace, "");
            str=str .TrimEnd ('\"');
            return str;
        }

    }
}

