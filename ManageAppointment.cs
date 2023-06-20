using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalendarForm
{
    public partial class ManageAppointment : Form
    {
        private DateTime selectedDate;
        private DateTime reminderTime;
        public ManageAppointment(DateTime date)
        {
            InitializeComponent();
            selectedDate = date;
        }

        private void ManageAppointment_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string customer = txtCustomer.Text;
            string address = txtAddress.Text;
            string comment = txtComment.Text;
            DateTime selectedDate = dtpDate.Value;
            if (string.IsNullOrEmpty(customer) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(comment))
            {
                MessageBox.Show("Fill the empty places");
            }
            else
            {
                InsertEvent(selectedDate, customer, address, comment);
                MessageBox.Show("Appointment Saved Successfully");

                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void InsertEvent(DateTime date, string customer, string address, string comment)
        {
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Calendar;User ID=sa;Password=Qwerty1;";
            string query = "INSERT INTO Events (Date, Customer, Address, Comment) VALUES (@Date, @Customer, @Address, @Comment)";

            

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Date", date);
                command.Parameters.AddWithValue("@Customer", customer);
                command.Parameters.AddWithValue("@Address", address);
                command.Parameters.AddWithValue("@Comment", comment);
                reminderTime = date;
                ScheduleReminder(reminderTime);

                connection.Open();
                command.ExecuteNonQuery();
            }
            
        }
        private async void PlayReminderSound()
        {
            string soundFilePath = "reminder.wav";

            using (SoundPlayer player = new SoundPlayer(soundFilePath))
            {
                player.Play();

                await Task.Delay(5000);

                player.Stop();
            }
        }

        private void rbtnReminder_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnReminder.Checked)
            {
                PlayReminderSound();
            }
        }
        private async void ScheduleReminder(DateTime reminderTime)
        {
            // Hatırlatıcı zamanına kalan süreyi hesapla
            TimeSpan timeRemaining = reminderTime - DateTime.Now;

            // Hatırlatıcı zamanından önceki bekleme süresi
            TimeSpan waitTime = timeRemaining.Add(TimeSpan.FromSeconds(-5)); // Örneğin, 5 saniye öncesinden hatırlat

            // Bekleme süresi dolana kadar bekle
            await Task.Delay(waitTime);

            // Hatırlatıcı zamanı geldiğinde sesi çal
            PlayReminderSound();
        }
    }
}
