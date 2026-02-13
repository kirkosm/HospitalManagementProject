using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HospitalClient
{
    public partial class HistoryEditForm : Form
    {
        string cs;       
        int patientId;  

        public HistoryEditForm(string connectionString, int pId)
        {
            InitializeComponent();

            cs = connectionString;
            patientId = pId;

            RefreshGrid();
        }

        private void HistoryEditForm_Load(object sender, EventArgs e)
        {
            this.Text = "Ιστορικό Timeline - Ασθενή #" + patientId;

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();

                string sql = "SELECT hospitalizations, examinations, diagnoses " +
                             "FROM patientHistory " +
                             "WHERE patient_id = @pid " +
                             "ORDER BY patientHistory_id DESC";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                adapter.SelectCommand.Parameters.Add("@pid", SqlDbType.Int).Value = patientId;

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string diagnoses = textBox1.Text.Trim();
            string hospitalizations = textBox2.Text.Trim();
            string examinations = textBox3.Text.Trim();

            if (diagnoses == "" && hospitalizations == "" && examinations == "")
            {
                MessageBox.Show("Παρακαλώ συμπληρώστε τουλάχιστον ένα πεδίο.");
                return;
            }

            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();

                string sql = "INSERT INTO patientHistory " +
                             "(patient_id, hospitalizations, examinations, diagnoses) " +
                             "VALUES (@pid, @h, @e, @d)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add("@pid", SqlDbType.Int).Value = patientId;

                if (hospitalizations == "")
                    cmd.Parameters.Add("@h", SqlDbType.NVarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@h", SqlDbType.NVarChar).Value = hospitalizations;

                if (examinations == "")
                    cmd.Parameters.Add("@e", SqlDbType.NVarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@e", SqlDbType.NVarChar).Value = examinations;

                if (diagnoses == "")
                    cmd.Parameters.Add("@d", SqlDbType.NVarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@d", SqlDbType.NVarChar).Value = diagnoses;

                cmd.ExecuteNonQuery();

                conn.Close();

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();

                RefreshGrid();

                MessageBox.Show("Το νέο γεγονός προστέθηκε στο ιστορικό!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Σφάλμα αποθήκευσης: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
