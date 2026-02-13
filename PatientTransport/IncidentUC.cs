using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ErgasiaHospital
{
    public partial class IncidentUC : UserControl
    {
        string cs;
        int userId;
        string personalNumber;
        string role;

        public IncidentUC(string connectionString, int uId, string pNumber, string r)
        {
            InitializeComponent();
            cs = connectionString;
            userId = uId;
            personalNumber = pNumber;
            role = r;

            // Εμφάνιση στοιχείων χρήστη
            textBox4.Text = personalNumber; // Personal Number
            textBox5.Text = role;           // Role
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string amka = textBox1.Text;
            string desc = textBox2.Text;
            int triage = (int)numericUpDown1.Value;

            if (string.IsNullOrEmpty(desc))
            {
                MessageBox.Show("Παρακαλώ συμπληρώστε την περιγραφή.");
                return;
            }

            // Αν δεν έχει ΑΜΚΑ, φτιάξε ένα προσωρινό
            if (string.IsNullOrEmpty(amka))
                amka = "TEMP" + DateTime.Now.Ticks.ToString().Substring(10);

            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();

                    // 1. Έλεγχος αν ο ασθενής υπάρχει ήδη
                    int patientId = 0;
                    string checkSql = "SELECT patient_id FROM patients WHERE amka = @amka";
                    using (SqlCommand cmdCheck = new SqlCommand(checkSql, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@amka", amka);
                        object result = cmdCheck.ExecuteScalar();

                        if (result != null)
                        {
                            patientId = Convert.ToInt32(result);
                        }
                        else
                        {
                            // 2. Αν δεν υπάρχει, βρες το επόμενο ID και δημιούργησέ τον
                            string idSql = "SELECT ISNULL(MAX(patient_id), 0) + 1 FROM patients";
                            using (SqlCommand cmdId = new SqlCommand(idSql, conn))
                                patientId = Convert.ToInt32(cmdId.ExecuteScalar());

                            string insPatient = "INSERT INTO patients (patient_id, amka) VALUES (@id, @amka)";
                            using (SqlCommand cmdInsP = new SqlCommand(insPatient, conn))
                            {
                                cmdInsP.Parameters.AddWithValue("@id", patientId);
                                cmdInsP.Parameters.AddWithValue("@amka", amka);
                                cmdInsP.ExecuteNonQuery();
                            }
                        }
                    }

                    // 3. Καταχώρηση του Περιστατικού
                    string insIncident = "INSERT INTO DescriptionSituation (patient_id, created_by, quick_description, triage_level, amka) " +
                                         "VALUES (@pid, @uid, @desc, @tr, @amka)";
                    using (SqlCommand cmdInc = new SqlCommand(insIncident, conn))
                    {
                        cmdInc.Parameters.AddWithValue("@pid", patientId);
                        cmdInc.Parameters.AddWithValue("@uid", userId);
                        cmdInc.Parameters.AddWithValue("@desc", desc);
                        cmdInc.Parameters.AddWithValue("@tr", triage);
                        cmdInc.Parameters.AddWithValue("@amka", amka);
                        cmdInc.ExecuteNonQuery();
                    }

                    MessageBox.Show("Το περιστατικό αποθηκεύτηκε επιτυχώς!");
                    textBox1.Clear();
                    textBox2.Clear();
                    numericUpDown1.Value = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Σφάλμα κατά την αποθήκευση: " + ex.Message);
            }
        }
    }
}