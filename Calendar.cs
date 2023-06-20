using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CalendarForm
{
    public partial class Calendar : Form
    {
        private DateTime selectedDate;
        private DataTable eventsTable;

        public Calendar()
        {
            InitializeComponent();
            selectedDate = DateTime.Now;
            eventsTable = new DataTable();
            UpdateCalendar();
            UpdateGridView();
        }

        private void UpdateCalendar()
        {
            lblMonth.Text = selectedDate.ToString("MMMM");
            lblYear.Text = selectedDate.Year.ToString();

            flowLayoutPanel1.Controls.Clear();

            int daysInMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);

            for (int i = 1; i <= daysInMonth; i++)
            {
                Button dayButton = new Button();
                dayButton.Text = i.ToString();
                dayButton.Tag = new DateTime(selectedDate.Year, selectedDate.Month, i);
                dayButton.Click += DayButton_Click;

                flowLayoutPanel1.Controls.Add(dayButton);
            }
        }

        private void DayButton_Click(object sender, EventArgs e)
        {
            Button dayButton = (Button)sender;
            selectedDate = (DateTime)dayButton.Tag;
            UpdateGridView();
        }

        private void btnAppointment_Click(object sender, EventArgs e)
        {
            ManageAppointment manageAppointment = new ManageAppointment(selectedDate);
            manageAppointment.ShowDialog();
            UpdateGridView();
        }

        private void btnPrevMonth_Click(object sender, EventArgs e)
        {
            selectedDate = selectedDate.AddMonths(-1);
            UpdateCalendar();
            UpdateGridView();
        }

        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            selectedDate = selectedDate.AddMonths(1);
            UpdateCalendar();
            UpdateGridView();
        }

        private void Calendar_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'calendarDataSet.Events' table. You can move, or remove it, as needed.
            this.eventsTableAdapter.Fill(this.calendarDataSet.Events);
            UpdateGridView();
        }

        private void UpdateGridView()
        {
            // Veritabanı bağlantısı oluştur
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Calendar;User ID=sa;Password=Qwerty1;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // SQL sorgusunu oluştur
                string query = "SELECT * FROM Events WHERE CONVERT(DATE, Date) = @SelectedDate";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SelectedDate", selectedDate.Date);

                // Veri adaptörünü oluştur
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                // Veri tablosunu temizle
                eventsTable.Clear();

                // Verileri doldur
                adapter.Fill(eventsTable);

                // DataGridView'i güncelle
                dataGridView1.DataSource = eventsTable;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int eventId = (int)selectedRow.Cells[0].Value;

                // Etkinlik güncelleme formunu aç ve eventId'yi iletebilirsin
                UpdateEventForm updateEventForm = new UpdateEventForm(eventId);
                DialogResult result = updateEventForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // Etkinlik güncellendikten sonra DataGridView'i güncelle
                    UpdateGridView();
                }
            }
            else
            {
                MessageBox.Show("Please select an event to update.");
            }
            dataGridView1.Refresh();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Seçili satırın veri tabanı kimlik değerini al
            int eventId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            // Etkinliği veri tabanından sil
            DeleteEvent(eventId);

            // Verileri güncelle ve DataGridView'i yenile
            UpdateCalendar();
            dataGridView1.Refresh();
        }
        private void DeleteEvent(int eventId)
        {
            // Veritabanından etkinliği sil
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Calendar;User ID=sa;Password=Qwerty1;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // SQL sorgusunu oluştur
                string query = "DELETE FROM Events WHERE ID = @EventID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EventID", eventId);

                // Bağlantıyı aç ve komutu çalıştır
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
