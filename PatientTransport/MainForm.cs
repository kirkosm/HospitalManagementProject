using System;
using System.Windows.Forms;

namespace ErgasiaHospital
{
    public partial class MainForm : Form
    {
        private string cs;
        private int userId;
        private string username;
        private string role;

        public MainForm(string connectionString, int uId, string uName, string r)
        {
            InitializeComponent();
            cs = connectionString;
            userId = uId;
            username = uName;
            role = r;

            this.Text = "Hospital Management System - " + username;
            this.WindowState = FormWindowState.Maximized; // Να ανοίγει μεγάλο

            LoadIncidentPage();
        }

        public void LoadIncidentPage()
        {
            // Δημιουργούμε το User Control και το "καρφώνουμε" στο panel
            IncidentUC uc = new IncidentUC(cs, userId, username, role);
            uc.Dock = DockStyle.Fill;

            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(uc);
        }
    }
}