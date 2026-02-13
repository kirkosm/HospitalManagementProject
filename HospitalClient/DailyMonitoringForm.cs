using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HospitalClient
{
    public partial class DailyMonitoringForm : Form
    {
        string cs;       
        int patientId;   

        public DailyMonitoringForm(string connectionString, int pId)
        {
            InitializeComponent();
            cs = connectionString;
            patientId = pId;

            LoadNotes();
        }

        private void DailyMonitoringForm_Load(object sender, EventArgs e)
        {
            Text = "Παρακολούθηση Ασθενή #" + patientId;

            LoadNotes();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadNotes();
        }

        private void LoadNotes()
        {
            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT monitoring_id, " +
                                  "note_datetime AS [Ημερομηνία/Ώρα], " +
                                  "note_text AS [Σημείωση] " +
                                  "FROM DailyMonitoring " +
                                  "WHERE patient_id = @pid " +
                                  "ORDER BY note_datetime DESC";

                cmd.Parameters.Add("@pid", SqlDbType.Int).Value = patientId;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns["monitoring_id"] != null)
                    dataGridView1.Columns["monitoring_id"].Visible = false;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Σφάλμα στο φόρτωμα: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string note = textBox1.Text.Trim();

            if (note == "")
            {
                MessageBox.Show("Παρακαλώ γράψτε μια σημείωση.");
                return;
            }

            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO DailyMonitoring (patient_id, note_text) " +
                                  "VALUES (@pid, @text)";

                cmd.Parameters.Add("@pid", SqlDbType.Int).Value = patientId;
                cmd.Parameters.Add("@text", SqlDbType.NVarChar, -1).Value = note;

                cmd.ExecuteNonQuery();

                conn.Close();

                textBox1.Clear();

                LoadNotes();

                MessageBox.Show("Η σημείωση προστέθηκε!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Σφάλμα στην αποθήκευση: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            object val = dataGridView1.CurrentRow.Cells["monitoring_id"].Value;
            if (val == null) return;

            int noteId = 0;
            bool parsed = int.TryParse(val.ToString(), out noteId);
            if (!parsed) return;

            DialogResult result = MessageBox.Show(
                "Διαγραφή σημείωσης;",
                "Επιβεβαίωση",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM DailyMonitoring WHERE monitoring_id = @id";
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = noteId;

                cmd.ExecuteNonQuery();

                conn.Close();

                LoadNotes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Σφάλμα διαγραφής: " + ex.Message);
            }
        }
    }
}
