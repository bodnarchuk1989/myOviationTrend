using System;
using System.Windows.Forms;

namespace OvationTrendLook
{
    public class MDIMainForm:Form
    {

        public MDIMainForm()
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            //MDIMainForm mainWindow = new MDIMainForm();
            this.Height = 500;
            this.Width = 700;
            this.Text = "Trend look";
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;            

            MainMenu mainMenu = new MainMenu();
            MenuItem mNewWindow = new MenuItem("New window");
            mNewWindow.Click += new EventHandler(mNewWindow_Click);
            mainMenu.MenuItems.Add(mNewWindow);

            MenuItem mWindow = new MenuItem("Window");

            MenuItem mWindowCascade = new MenuItem("Cascade");
            mWindowCascade.Click += new EventHandler(mWindowsCascade_Click);
            mWindow.MenuItems.Add(mWindowCascade);

            MenuItem mWindowTileH = new MenuItem("Tile Horizontal");
            mWindowTileH.Click += new EventHandler(mWindowTileH_Click);
            mWindow.MenuItems.Add(mWindowTileH);

            MenuItem mWindowTileV = new MenuItem("Tile Vertical");
            mWindowTileV.Click += new EventHandler(mWindowTileV_Click);
            mWindow.MenuItems.Add(mWindowTileV);
                       
            mainMenu.MenuItems.Add(mWindow);

            MenuItem mWorkWithDB = new MenuItem("Work with DB");

            MenuItem mImportToDb = new MenuItem("Import to Data Base");
            mImportToDb.Click += new EventHandler(mIbortToDB_Click);
            mWorkWithDB.MenuItems.Add(mImportToDb);

            mainMenu.MenuItems.Add(mWorkWithDB);

         
            this.Menu = mainMenu;

            this.Show();
            this.mNewWindow_Click(this, new EventArgs());
        }

        void mNewWindow_Click(object sender, EventArgs e)
        {
            OvationTrendLookMainForm window = new OvationTrendLookMainForm();
            window.MdiParent = this;
            window.WindowState = FormWindowState.Maximized;
            window.Show();
        }

        void mWindowsCascade_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        void mWindowTileH_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        void mWindowTileV_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }
            
        void mIbortToDB_Click(object sender, EventArgs e)
        {
            ImportToDBForm importForm = new ImportToDBForm();
        }
    }
}

    