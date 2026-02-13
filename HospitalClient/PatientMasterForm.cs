using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HospitalClient
{
    public partial class PatientMasterForm : Form
    {
        private string cs; 
        private DataTable masterTable; 

        public PatientMasterForm(string connectionString, List<int> initialPatientIds)
        {
            InitializeComponent();

            cs = connectionString;

            masterTable = new DataTable();
            dataGridView1.DataSource = masterTable;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            txtSearch.TextChanged += txtSearch_TextChanged;

            dataGridView1.ReadOnly = true;

            LoadMasterData();
        }

        private void LoadMasterData()
        {
            if (string.IsNullOrEmpty(cs)) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(cs)) 
                {
                    conn.Open();

                    string sql = "SELECT patient_id, amka, case_id AS DescriptionSituation_id, triage_level, quick_description " +
                                 "FROM MasterPatients " +
                                 "ORDER BY triage_level ASC";

                    SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                    masterTable.Clear();
                    adapter.Fill(masterTable);
                } 

                FormatColumnsForDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load error: " + ex.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            object val = dataGridView1.CurrentRow.Cells["patient_id"].Value;
            if (val == null) return;

            int pid = 0;
            if (int.TryParse(val.ToString(), out pid))
            {
                LoadPatientDashboard(pid);
            }
        }

        private void LoadPatientDashboard(int pid)
        {
            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();

                SqlCommand cmd1 = new SqlCommand("SELECT amka, firstName, lastName, phoneNumber FROM patients WHERE patient_id=@pid", conn);
                cmd1.Parameters.Add("@pid", SqlDbType.Int).Value = pid;

                SqlDataReader r = cmd1.ExecuteReader();
                if (r.Read())
                {
                    txtAmka.Text = r["amka"] != DBNull.Value ? r["amka"].ToString() : "";
                    txtFirstName.Text = r["firstName"] != DBNull.Value ? r["firstName"].ToString() : "";
                    txtLastName.Text = r["lastName"] != DBNull.Value ? r["lastName"].ToString() : "";
                    txtPhone.Text = r["phoneNumber"] != DBNull.Value ? r["phoneNumber"].ToString() : "";
                }
                r.Close();

                SqlCommand cmd2 = new SqlCommand("SELECT note_datetime, note_text FROM DailyMonitoring WHERE patient_id=@pid ORDER BY note_datetime DESC", conn);
                cmd2.Parameters.Add("@pid", SqlDbType.Int).Value = pid;
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dt2 = new DataTable();
                da2.Fill(dt2);
                dgMonitoring.DataSource = dt2;

                SqlCommand cmd3 = new SqlCommand(
                    "SELECT ec.exam_title, pe.exam_datetime, pe.exam_result " +
                    "FROM PatientExaminations pe " +
                    "JOIN examinationsCatalog ec ON ec.examinationsCatalog_id = pe.examinationsCatalog_id " +
                    "WHERE pe.patient_id=@pid", conn);
                cmd3.Parameters.Add("@pid", SqlDbType.Int).Value = pid;
                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                DataTable dt3 = new DataTable();
                da3.Fill(dt3);
                dgExams.DataSource = dt3;

                SqlCommand cmd4 = new SqlCommand(
                    "SELECT history_date, diagnoses, hospitalizations, examinations FROM patientHistory WHERE patient_id=@pid ORDER BY history_date DESC", conn);
                cmd4.Parameters.Add("@pid", SqlDbType.Int).Value = pid;
                SqlDataAdapter da4 = new SqlDataAdapter(cmd4);
                DataTable dt4 = new DataTable();
                da4.Fill(dt4);
                dgvHistoryMaster.DataSource = dt4;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dashboard error: " + ex.Message);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (masterTable == null) return;

            string value = txtSearch.Text.Replace("'", "''");
            if (value.Length > 0 && value.Length < 2) return;

            try
            {
                string filter = "amka LIKE '%" + value + "%' OR quick_description LIKE '%" + value + "%'";
                masterTable.DefaultView.RowFilter = filter;
            }
            catch
            {
                masterTable.DefaultView.RowFilter = "";
            }
        }

        public void AddRowsFromSource(DataTable sourceTable, List<DataRow> rowsToMove, bool removeFromSource = false)
        {
            if (sourceTable == null || rowsToMove == null) return;

            masterTable.BeginLoadData();

            foreach (DataRow row in rowsToMove)
            {
                masterTable.ImportRow(row);

                if (removeFromSource)
                    sourceTable.Rows.Remove(row);
            }

            masterTable.EndLoadData();
            FormatColumnsForDisplay();
        }

        private void FormatColumnsForDisplay()
        {
            if (dataGridView1.Columns.Count == 0) return;

            if (dataGridView1.Columns.Contains("patient_id")) dataGridView1.Columns["patient_id"].HeaderText = "Patient ID";
            if (dataGridView1.Columns.Contains("amka")) dataGridView1.Columns["amka"].HeaderText = "AMKA";
            if (dataGridView1.Columns.Contains("DescriptionSituation_id")) dataGridView1.Columns["DescriptionSituation_id"].HeaderText = "Case ID";
            if (dataGridView1.Columns.Contains("triage_level")) dataGridView1.Columns["triage_level"].HeaderText = "Triage";
            if (dataGridView1.Columns.Contains("quick_description")) dataGridView1.Columns["quick_description"].HeaderText = "Description";
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            if (row.Cells["patient_id"].Value == null) return;

            int pid = 0;
            if (int.TryParse(row.Cells["patient_id"].Value.ToString(), out pid))
            {
                PatientEditForm editForm = new PatientEditForm(cs, pid);
                editForm.ShowDialog();

                LoadMasterData();
            }
        }
    }
}
