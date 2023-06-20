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
    public partial class UpdateEventForm : Form
    {
        private int eventId;
        public UpdateEventForm(int eventId)
        {
            InitializeComponent();
            this.eventId = eventId;
        }

        private void UpdateEventForm_Load(object sender, EventArgs e)
        {
            LoadEventData();
        }

        private void LoadEventData()
        {
            // Veritabanından etkinlik verilerini getir ve formu doldur
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Calendar;User ID=sa;Password=Qwerty1;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // SQL sorgusunu oluştur
                string query = "SELECT * FROM Events WHERE ID = @EventID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EventID", eventId);

                // Bağlantıyı aç, veriyi getir ve formu doldur
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    // Verileri form kontrollerine yükle
                    txtCustomer.Text = reader["Customer"].ToString();
                    txtAddress.Text = reader["Address"].ToString();
                    txtComment.Text = reader["Comment"].ToString();
                }
                reader.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string customer = txtCustomer.Text;
            string address = txtAddress.Text;
            string comment = txtComment.Text;

            if (string.IsNullOrEmpty(customer) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(comment))
            {
                MessageBox.Show("Fill the empty places");
            }
            else
            {
                UpdateEvent(customer, address, comment);
                MessageBox.Show("Event Updated Successfully");

                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void UpdateEvent(string customer, string address, string comment)
        {
            // Veritabanındaki etkinliği güncelle
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Calendar;User ID=sa;Password=Qwerty1;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // SQL sorgusunu oluştur
                string query = "UPDATE Events SET Customer = @Customer, Address = @Address, Comment = @Comment WHERE ID = @EventID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Customer", customer);
                command.Parameters.AddWithValue("@Address", address);
                command.Parameters.AddWithValue("@Comment", comment);
                command.Parameters.AddWithValue("@EventID", eventId);

                // Bağlantıyı aç ve komutu çalıştır
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
