using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HospitalClient
{
    public partial class PatientExamsForm : Form
    {
        string cs;       
        int patientId;

        public PatientExamsForm(string connectionString, int pId)
        {
            InitializeComponent();

            cs = connectionString;
            patientId = pId;

            cmbExam.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbExam.SelectedIndexChanged += cmbExam_SelectedIndexChanged;

            LoadExamTitles();
            LoadExams();
        }

        private void PatientExamsForm_Load(object sender, EventArgs e)
        {
            this.Text = "Εξετάσεις Ασθενή #" + patientId;

            LoadExamTitles();
            LoadExams();
        }

        private void LoadExamTitles()
        {
            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();

                string sql = "SELECT DISTINCT exam_title FROM examinationsCatalog ORDER BY exam_title";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbExam.DisplayMember = "exam_title";
                cmbExam.ValueMember = "exam_title";
                cmbExam.DataSource = dt;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Σφάλμα τίτλων: " + ex.Message);
            }
        }

        private void cmbExam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbExam.SelectedValue == null) return;

            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();

                string sql = "SELECT examinationsCatalog_id, exam_category " +
                             "FROM examinationsCatalog " +
                             "WHERE exam_title = @title " +
                             "ORDER BY exam_category";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@title", SqlDbType.NVarChar).Value = cmbExam.SelectedValue.ToString();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbCategory.DisplayMember = "exam_category";
                cmbCategory.ValueMember = "examinationsCatalog_id";
                cmbCategory.DataSource = dt;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Σφάλμα κατηγοριών: " + ex.Message);
            }
        }

        private void btnAddExam_Click(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedValue == null)
            {
                MessageBox.Show("Επιλέξτε πρώτα μια κατηγορία!");
                return;
            }

            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();

                string sql = "INSERT INTO PatientExaminations " +
                             "(patient_id, examinationsCatalog_id, exam_result) " +
                             "VALUES (@pid, @eid, @res)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@pid", SqlDbType.Int).Value = patientId;
                cmd.Parameters.Add("@eid", SqlDbType.Int).Value = cmbCategory.SelectedValue;

                if (string.IsNullOrWhiteSpace(txtExamResult.Text))
                    cmd.Parameters.Add("@res", SqlDbType.NVarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@res", SqlDbType.NVarChar).Value = txtExamResult.Text.Trim();

                cmd.ExecuteNonQuery();

                conn.Close();

                txtExamResult.Clear();
                LoadExams();

                MessageBox.Show("Η εξέταση προστέθηκε!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Σφάλμα προσθήκης: " + ex.Message);
            }
        }

        private void LoadExams()
        {
            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();

                string sql = "SELECT pe.patientExam_id, pe.exam_datetime, " +
                             "ec.exam_title, ec.exam_category, pe.exam_result " +
                             "FROM PatientExaminations pe " +
                             "JOIN examinationsCatalog ec ON ec.examinationsCatalog_id = pe.examinationsCatalog_id " +
                             "WHERE pe.patient_id = @pid " +
                             "ORDER BY pe.exam_datetime DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@pid", SqlDbType.Int).Value = patientId;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView2.DataSource = dt;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Σφάλμα λίστας: " + ex.Message);
            }
        }

        private void btnRefreshExams_Click(object sender, EventArgs e)
        {
            LoadExams();
        }
    }
}