using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HospitalClient
{
    public partial class PatientEditForm : Form
    {
        string cs;       
        int patientId;   
        public PatientEditForm(string connectionString, int pId)
        {
            InitializeComponent();

            cs = connectionString;
            patientId = pId;

            LoadPatient();
        }

        private void PatientEditForm_Load(object sender, EventArgs e)
        {
            this.Text = "Επεξεργασία Ασθενή #" + patientId;
            LoadPatient();
        }

        private void LoadPatient()
        {
            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();

                string sql = "SELECT amka, firstName, lastName, phoneNumber, address, debts, dateOfBirth " +
                             "FROM patients WHERE patient_id = @pid";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@pid", SqlDbType.Int).Value = patientId;

                SqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    if (r["amka"] != DBNull.Value) txtAmka.Text = r["amka"].ToString(); else txtAmka.Text = "";
                    if (r["firstName"] != DBNull.Value) txtFirstName.Text = r["firstName"].ToString(); else txtFirstName.Text = "";
                    if (r["lastName"] != DBNull.Value) txtLastName.Text = r["lastName"].ToString(); else txtLastName.Text = "";
                    if (r["phoneNumber"] != DBNull.Value) txtPhone.Text = r["phoneNumber"].ToString(); else txtPhone.Text = "";
                    if (r["address"] != DBNull.Value) txtAddress.Text = r["address"].ToString(); else txtAddress.Text = "";
                    if (r["debts"] != DBNull.Value) txtDebts.Text = r["debts"].ToString(); else txtDebts.Text = "0";

                    if (r["dateOfBirth"] != DBNull.Value)
                    {
                        dtpDob.Value = (DateTime)r["dateOfBirth"];
                        dtpDob.Checked = true;
                    }
                    else
                    {
                        dtpDob.Checked = false;
                    }
                }
                else
                {
                    MessageBox.Show("Ο ασθενής δεν βρέθηκε στη βάση.");
                }

                r.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Σφάλμα φόρτωσης: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string amka = txtAmka.Text.Trim();

            if (amka == "")
            {
                MessageBox.Show("Το AMKA είναι υποχρεωτικό!");
                return;
            }

            if (amka == "-")
                amka = "UNKNOWN-" + patientId;

            try
            {
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();

                string sqlPatients = "UPDATE patients SET " +
                                     "amka=@amka, firstName=@f, lastName=@l, " +
                                     "dateOfBirth=@dob, phoneNumber=@p, address=@ad, debts=@d " +
                                     "WHERE patient_id=@pid";

                SqlCommand cmd = new SqlCommand(sqlPatients, conn);
                cmd.Parameters.Add("@amka", SqlDbType.NVarChar, 50).Value = amka;
                cmd.Parameters.Add("@f", SqlDbType.NVarChar, 100).Value = txtFirstName.Text.Trim();
                cmd.Parameters.Add("@l", SqlDbType.NVarChar, 100).Value = txtLastName.Text.Trim();
                cmd.Parameters.Add("@p", SqlDbType.NVarChar, 20).Value = txtPhone.Text.Trim();
                cmd.Parameters.Add("@ad", SqlDbType.NVarChar, 255).Value = txtAddress.Text.Trim();
                cmd.Parameters.Add("@pid", SqlDbType.Int).Value = patientId;

                if (dtpDob.Checked)
                    cmd.Parameters.Add("@dob", SqlDbType.Date).Value = dtpDob.Value.Date;
                else
                    cmd.Parameters.Add("@dob", SqlDbType.Date).Value = DBNull.Value;

                decimal debt = 0;
                if (decimal.TryParse(txtDebts.Text.Trim(), out debt))
                    cmd.Parameters.Add("@d", SqlDbType.Decimal).Value = debt;
                else
                    cmd.Parameters.Add("@d", SqlDbType.Decimal).Value = DBNull.Value;

                cmd.ExecuteNonQuery();

                string sqlMaster = "UPDATE MasterPatients SET amka=@amka WHERE patient_id=@pid";
                SqlCommand cmd2 = new SqlCommand(sqlMaster, conn);
                cmd2.Parameters.Add("@amka", SqlDbType.NVarChar, 50).Value = amka;
                cmd2.Parameters.Add("@pid", SqlDbType.Int).Value = patientId;
                cmd2.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show("Αποθηκεύτηκε επιτυχώς!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Σφάλμα αποθήκευσης: " + ex.Message);
            }
        }

        private void btnDaily_Click(object sender, EventArgs e)
        {
            DailyMonitoringForm f = new DailyMonitoringForm(cs, patientId);
            f.ShowDialog();
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            HistoryEditForm f = new HistoryEditForm(cs, patientId);
            f.ShowDialog();
        }

        private void btnExams_Click(object sender, EventArgs e)
        {
            PatientExamsForm f = new PatientExamsForm(cs, patientId);
            f.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
