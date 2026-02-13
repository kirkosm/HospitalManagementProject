using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HospitalClient
{
    public partial class LoginForm : Form
    {
        private string connectionString =
            "workstation id=ErgasiaHospital.mssql.somee.com;" +
            "user id=michael200101_SQLLogin_1;" +
            "pwd=w5umrvg2eb;" +
            "data source=ErgasiaHospital.mssql.somee.com;" +
            "initial catalog=ErgasiaHospital;" +
            "TrustServerCertificate=True";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show("Συμπλήρωσε username και password");
                return;
            }

            bool loginOk = CheckLogin(username, password);

            if (loginOk)
            {
                CasesForm casesForm = new CasesForm();
                casesForm.Show();

                this.Hide();
            }
            else
            {
                MessageBox.Show("Λάθος username ή password");
            }
        }

        private bool CheckLogin(string username, string password)
        {
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                string sql = "SELECT COUNT(*) FROM HospitalUsers " +
                             "WHERE Username = @u AND Password = @p";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);

                object result = cmd.ExecuteScalar();
                int count = 0;

                if (result != null)
                {
                    count = Convert.ToInt32(result);
                }

                con.Close();

                if (count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Σφάλμα σύνδεσης: " + ex.Message);
                return false;
            }
        }
    }
}
