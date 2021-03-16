using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace VitalsApp
{
    public partial class AppointmentForm : Form
    {
        public AppointmentForm()
        {
            InitializeComponent();
        }

        private void AppointmentForm_Load(object sender, EventArgs e)
        {
            // Test Values
            txtName.Text = "Boluwaji";
            txtPhoneNumber.Text = "123456";
            txtEmail.Text = "Bolu@test.com";
            txtAge.Text = "20";
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if(ValidateDataInput() == true)
            {
                StoreInformation();
            }
        }

        private bool ValidateDataInput()
        {
            // Declare the inputs to validate
            bool isValid = true;
            string msg = "Input Error\n\n";
            long phoneNumber;
            int age;

            // Validate Missing Input
            if(cbbHospital.Text == "" || cbbGender.Text == "" || cbbTime.Text == "" ||
                txtEmail.Text == "" || txtName.Text == "" || dtDate.Text == "")
            {
                msg += "Please enter the missing information\n\n";
                isValid = false;
            }

            // Validate Age input
            if ((!int.TryParse(txtAge.Text, out age)))
            {
                msg += "Please enter a valid age\n\n";
                txtAge.BackColor = Color.Pink;
                isValid = false;
            }
            else if(txtAge.Text == "")
            {
                msg += "Please enter the missing information\n\n";
                txtAge.BackColor = Color.Pink;
                isValid = false;
            }

            // Validate Phone number input
            if ((!long.TryParse(txtPhoneNumber.Text, out phoneNumber)))
            {
                msg += "Please enter a valid phone number\n\n";
                txtPhoneNumber.BackColor = Color.Pink;
                isValid = false;
            }
            else if (txtPhoneNumber.Text == "")
            {
                msg += "Please enter the missing information\n\n";
                txtPhoneNumber.BackColor = Color.Pink;
                isValid = false;
            }


            // Handle Message Box
            if (!isValid)
            {
                MessageBox.Show(msg, "Input Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return isValid;

        }

        private void StoreInformation()
        {
            // Declare the required variables
            string hospital;
            string date;
            string time;
            string name;
            long phoneNumber;
            string email;
            int age;
            string gender;

            // Get input from user
            hospital = cbbHospital.Text;
            date = dtDate.Text;
            time = cbbTime.Text;
            name = txtName.Text;
            phoneNumber = Convert.ToInt64(txtPhoneNumber.Text);
            email = txtEmail.Text;
            age = Convert.ToInt32(txtAge.Text);
            gender = cbbGender.Text;

            // Save patients information to a text file for analysis
            using (FileStream fs = new FileStream("PatientAppointment.txt", FileMode.Append))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(hospital + "," +
                    date + "," +
                    time + "," +
                    name + "," +
                    phoneNumber + "," +
                    email + "," +
                    age + "," +
                    gender);
            }

            // Output Confirmation to Screen
            MessageBox.Show("Your appointment with the doctor has been confirmed!", "Confirmation",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
            
            // Close the appointment form once appointment confirmed
            this.Close();
            

        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox tb = (TextBox)sender;
                tb.BackColor = Color.White;
            }
        }

    }
}
