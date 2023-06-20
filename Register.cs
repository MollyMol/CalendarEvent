using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalendarForm
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Register_Load(object sender, EventArgs e)
        {

        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            string name = txtIsim.Text;
            string surname = txtSoyad.Text;
            string username = txtKullaniciAdi.Text;
            string password = txtPassword.Text;
            string tc = txtTC.Text;
            string phone = txtTelefon.Text;
            string email = txtEmail.Text;
            string address = txtAdres.Text;
            string userType = lstKullaniciType.SelectedItem.ToString();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(username) ||
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(tc) || string.IsNullOrEmpty(phone) ||
            string.IsNullOrEmpty(email) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(userType))
            {
                MessageBox.Show("Fill the empty places");
            }
            else
            {
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Calendar;User ID=sa;Password=Qwerty1;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Users (Name, Surname, Username, Password, TC, Phone, Email, Address, UserType) " +
                                   "VALUES(@Name, @Surname, @Username, @Password, @TC, @Phone, @Email, @Address, @UserType)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Surname", surname);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@TC", tc);
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@UserType", userType);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Register Success");

                Login login = new Login();
                login.Show();
                this.Hide();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

       
    }
}
