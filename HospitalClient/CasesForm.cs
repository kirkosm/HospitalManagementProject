using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalClient
{
    public partial class CasesForm : Form
    {
        private readonly string cs =
            "workstation id=ErgasiaHospital.mssql.somee.com;" +
            "user id=michael200101_SQLLogin_1;" +
            "pwd=w5umrvg2eb;" +
            "data source=ErgasiaHospital.mssql.somee.com;" +
            "initial catalog=ErgasiaHospital;" +
            "TrustServerCertificate=True";

        public CasesForm()
        {
            InitializeComponent();
            RefreshCases(); 

            dataGridView1.ReadOnly = true;
        }

        private void RefreshCases()
        {
            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();

                string sql = "SELECT TOP 50 " +
                             "ds.patient_id AS [Patient ID], " +
                             "p.amka AS [AMKA], " +
                             "ds.DescriptionSituation_id AS [Case ID], " +
                             "ds.triage_level AS [Triage], " +
                             "ds.quick_description AS [Description] " +
                             "FROM DescriptionSituation ds " +
                             "LEFT JOIN patients p ON ds.patient_id = p.patient_id " +
                             "ORDER BY ds.triage_level ASC";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                DataTable table = new DataTable();

                adapter.Fill(table);

                dataGridView1.DataSource = table;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private object GetCellValue(DataGridViewRow row, string columnName)
        {
            if (row == null) return null;
            if (!dataGridView1.Columns.Contains(columnName)) return null;
            return row.Cells[columnName].Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshCases();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            object val = GetCellValue(dataGridView1.CurrentRow, "Case ID");
            if (val == null || val == DBNull.Value) return;

            int descId = 0;
            bool parsed = int.TryParse(val.ToString(), out descId);
            if (!parsed) return;

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete Case #" + descId + "?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo);

            if (result != DialogResult.Yes) return;

            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM DescriptionSituation WHERE DescriptionSituation_id = @id", conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = descId;

                cmd.ExecuteNonQuery();

                conn.Close();

                RefreshCases();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete error: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Παρακαλώ επιλέξτε έναν ή περισσότερους ασθενείς!");
                return;
            }

            DataTable sourceTable = dataGridView1.DataSource as DataTable;
            if (sourceTable == null) return;

            List<DataRow> rowsToMove = new List<DataRow>();

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                DataGridViewRow dgvRow = dataGridView1.SelectedRows[i];
                DataRowView drv = dgvRow.DataBoundItem as DataRowView;
                if (drv != null)
                {
                    rowsToMove.Add(drv.Row);
                }
            }

            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();

                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    foreach (DataRow row in rowsToMove)
                    {
                        string cmdText = "INSERT INTO MasterPatients " +
                                         "(patient_id, amka, case_id, triage_level, quick_description) " +
                                         "VALUES (@pid, @amka, @cid, @triage, @desc); " +
                                         "DELETE FROM DescriptionSituation WHERE DescriptionSituation_id = @cid;";

                        SqlCommand cmd = new SqlCommand(cmdText, conn, trans);
                        cmd.Parameters.Add("@pid", SqlDbType.Int).Value = row["Patient ID"];
                        cmd.Parameters.Add("@amka", SqlDbType.NVarChar).Value = row["AMKA"] ?? DBNull.Value;
                        cmd.Parameters.Add("@cid", SqlDbType.Int).Value = row["Case ID"];
                        cmd.Parameters.Add("@triage", SqlDbType.Int).Value = row["Triage"] ?? DBNull.Value;
                        cmd.Parameters.Add("@desc", SqlDbType.NVarChar).Value = row["Description"] ?? DBNull.Value;

                        cmd.ExecuteNonQuery();
                    }

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }

                conn.Close();

                PatientMasterForm masterForm = null;

                foreach (Form f in Application.OpenForms)
                {
                    if (f is PatientMasterForm)
                    {
                        masterForm = f as PatientMasterForm;
                        break;
                    }
                }

                if (masterForm == null)
                {
                    masterForm = new PatientMasterForm(cs, new List<int>());
                }

                masterForm.AddRowsFromSource(sourceTable, rowsToMove, true);
                masterForm.BringToFront();

                RefreshCases();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Transfer failed: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PatientMasterForm masterForm = null;

            foreach (Form f in Application.OpenForms)
            {
                if (f is PatientMasterForm)
                {
                    masterForm = f as PatientMasterForm;
                    break;
                }
            }

            if (masterForm == null)
            {
                masterForm = new PatientMasterForm(cs, new List<int>());
            }

            this.Hide();
            masterForm.Show();
        }
    }
}
