using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace PatientTransport
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            username = username.Trim();

            string password = textBox2.Text;
            password = password.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show("Παρακαλώ εισάγετε όνομα χρήστη και κωδικό πρόσβασης.");
                return;
            }

            string connectionString =
                "workstation id=ErgasiaHospital.mssql.somee.com;" +
                "packet size=4096;" +
                "user id=michael200101_SQLLogin_1;" +
                "pwd=w5umrvg2eb;" +
                "data source=ErgasiaHospital.mssql.somee.com;" +
                "persist security info=False;" +
                "initial catalog=ErgasiaHospital;" +
                "TrustServerCertificate=True";

            string query =
                "SELECT healthcareProfessionals_id, role " +
                "FROM healthcareProfessionals " +
                "WHERE personal_number = @username AND password = @password";

            try
            {
                SqlConnection conn = new SqlConnection(connectionString);

                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand(query, conn);

                SqlParameter p1 = new SqlParameter("@username", SqlDbType.NVarChar);
                p1.Value = username;
                cmd.Parameters.Add(p1);

                SqlParameter p2 = new SqlParameter("@password", SqlDbType.NVarChar);
                p2.Value = password;
                cmd.Parameters.Add(p2);

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                bool foundUser = await reader.ReadAsync();

                if (foundUser == true)
                {
                    int userId = Convert.ToInt32(reader["healthcareProfessionals_id"]);

                    string role = reader["role"].ToString();

                    IncidentForm f2 = new IncidentForm(
                        connectionString,
                        userId,
                        username,
                        role
                    );

                    this.Hide();

                    f2.ShowDialog();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Λάθος όνομα χρήστη ή κωδικός πρόσβασης.");
                }

                reader.Close();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Σφάλμα: " + ex.Message);
            }
        }
    }
}