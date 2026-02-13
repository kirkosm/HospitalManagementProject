using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace PatientTransport
{
    public partial class IncidentForm : Form
    {
        string cs;
        int userId;
        string personalNumber;
        string role;

        public IncidentForm(string connectionString, int uId, string pNumber, string r)
        {
            InitializeComponent();

            cs = connectionString;
            userId = uId;
            personalNumber = pNumber;
            role = r;

            textBox4.Text = personalNumber;
            textBox5.Text = role;
            this.Text = "Σύνδεση: " + personalNumber;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string amka = textBox2.Text;
            string desc = textBox1.Text;
            int triage = (int)numericUpDown1.Value;

            amka = amka.Trim();
            desc = desc.Trim();

            if (desc == "")
            {
                MessageBox.Show("γράψε μια περιγραφή");
                return;
            }

            if (amka == "")
            {
                string ticks = DateTime.Now.Ticks.ToString();
                amka = "TEMP" + ticks.Substring(10);
            }

            string sqlCommand =
                "DECLARE @pid INT; " +
                "SELECT @pid = patient_id FROM patients WHERE amka = @amka; " +
                "IF @pid IS NULL " +
                "BEGIN " +
                "   SELECT @pid = ISNULL(MAX(patient_id), 0) + 1 FROM patients; " +
                "   INSERT INTO patients (patient_id, amka) VALUES (@pid, @amka); " +
                "END " +
                "INSERT INTO DescriptionSituation " +
                "(patient_id, created_by, quick_description, triage_level, amka) " +
                "VALUES (@pid, @uid, @desc, @tr, @amka);";

            try
            {
                SqlConnection conn = new SqlConnection(cs);
                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand(sqlCommand, conn);

                SqlParameter p1 = new SqlParameter("@amka", SqlDbType.NVarChar, 50);
                p1.Value = amka;
                cmd.Parameters.Add(p1);

                SqlParameter p2 = new SqlParameter("@uid", SqlDbType.Int);
                p2.Value = userId;
                cmd.Parameters.Add(p2);

                SqlParameter p3 = new SqlParameter("@desc", SqlDbType.NVarChar);
                p3.Value = desc;
                cmd.Parameters.Add(p3);

                SqlParameter p4 = new SqlParameter("@tr", SqlDbType.Int);
                p4.Value = triage;
                cmd.Parameters.Add(p4);

                await cmd.ExecuteNonQueryAsync();

                conn.Close();

                MessageBox.Show("Το περιστατικό αποθηκεύτηκε!");

                textBox1.Clear();
                textBox2.Clear();
                numericUpDown1.Value = numericUpDown1.Minimum;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Κάτι πήγε λάθος: " + ex.Message);
            }
        }
    }
}