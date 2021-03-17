// Developer: Oyewumi, Boluwaji
// Course: MIS 5315 – Spring 2021
// Assignment: Project #1 - Vitals Dashboard
// Description: This program computes and displays the level of a user’s health vitals


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VitalsApp
{
    public partial class VitalsForm : Form
    {
        //FIELDS       
        //Define default theme colors
        private Color colorSchemeText = Color.DimGray;
        private Color colorSchemeNormal = Color.Gold;
        private Color colorSchemeWarning = Color.OrangeRed;
        private Color colorSchemeIssue = Color.Firebrick;

        //Define constants for MAX values
        private const int MaxSystolicBP = 180;
        private const int MaxDiastolicBP = 110;
        private const int GlucoseLevel = 300;


        public VitalsForm()
        {
            InitializeComponent();
        }

        private void VitalsForm_Load(object sender, EventArgs e)
        {
            //Call the test data method
            LoadTestData();

            //Call the display dashboard method and make panels invisible
            DisplayDashboard(false);

            ChangeApplicationResolution(); //do not remove this line of code.
        }

        private void ChangeApplicationResolution()
        {
            //Do NOT delete this method or alter this code
            int formWidth = 780;
            int formHeigth = 605;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.Size = new System.Drawing.Size(formWidth, formHeigth);
            this.CenterToScreen();
            //this.WindowState = FormWindowState.Maximized; //maximize the screen if it's chopping off
        }

        // Main Click Methods
        private void btnSubmitVitals_Click(object sender, EventArgs e)
        {
            // Validate inputs before running program
            if(ValidateFormData() == true)
            {
                // Check the Alert messages
                if(GenEmergencyAlert() == true)
                {
                    CalcHeartRate();
                    CalcBMI();
                    CalcGlucose();
                    SetOverallHypertensionLevel(CalcBloodPressureSystolic(), CalcBloodPressureDiastolic());
                    DisplayDashboard(true);
                }
                
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            // Clear out the values of the textboxes
            txtAge.Text = "";
            txtAge.BackColor = Color.White;

            txtHeightFt.Text = "";
            txtHeightFt.BackColor = Color.White;

            txtHeightIn.Text = "";
            txtHeightIn.BackColor = Color.White;

            txtWeight.Text = "";
            txtWeight.BackColor = Color.White;

            txtBloodPressureSystolic.Text = "";
            txtBloodPressureSystolic.BackColor = Color.White;

            txtBloodPressureDiastolic.Text = "";
            txtBloodPressureDiastolic.BackColor = Color.White;

            txtGlucose.Text = "";
            txtGlucose.BackColor = Color.White;

            // Call the Display Dashboard method
            DisplayDashboard(false);

            // Set focus on Age textbox
            txtAge.Focus();
            txtAge.SelectAll();
        }

        private void picLoadTestData_Click(object sender, EventArgs e)
        {
            //Call the test data method
            LoadTestData();
            //Call the display dashboard method and make panels invisible
            DisplayDashboard(false);
        }

        // Special feature Click Methods
        private void btnAppointment_Click(object sender, EventArgs e)
        {
            DisplayDashboard(false);
            AppointmentForm f = new AppointmentForm(); //create a new form instance
            f.Show(); //display the form
        }

        private void btnRightScreen_Click(object sender, EventArgs e)
        {
            int currentWidth = this.Size.Width;
            int currentHeight = this.Size.Height;

            int anyWidth = currentWidth + pnlScreenRight.Size.Width;

            this.Size = new Size(anyWidth, currentHeight);
        }

        //Processing Methods

        private void LoadTestData()
        {
            // Set the test data
            txtAge.Text = "20";
            txtHeightFt.Text = "5";
            txtHeightIn.Text = "6";
            txtWeight.Text = "170.5";
            txtBloodPressureSystolic.Text = "110";
            txtBloodPressureDiastolic.Text = "95";
            txtGlucose.Text = "90";

            // Set focus on Age textbox
            txtAge.Focus();
            txtAge.SelectAll();
        }
        /// <summary>
        /// Displays the application dashboard
        /// </summary>
        /// <param name="shouldDisplay"></param>
        private void DisplayDashboard(bool shouldDisplay)
        {
            if(shouldDisplay is true)
            {
                // Display panels
                pnlBMI.Visible = true;
                pnlGlucose.Visible = true;
                pnlHeartRate1.Visible = true;
                pnlHeartRate2.Visible = true;
                pnlBloodPressure.Visible = true;
                pnlProfile.Visible = true;
            }
            else
            {
                // Hide panels except profile panel
                pnlBMI.Visible = false;
                pnlGlucose.Visible = false;
                pnlHeartRate1.Visible = false;
                pnlHeartRate2.Visible = false;
                pnlBloodPressure.Visible = false;
            }

        }
        /// <summary>
        /// Calculates the user's heart rate
        /// </summary>
        private void CalcHeartRate()
        {
            // Declare the general Variables
            int age;
            const int MHRAge = 220;
            const double MAXPERCENT = 0.85;
            const double MINPERCENT = 0.50;
            const int SEC_IN_MIN = 60;
            const int RULE_TIME_CHANGE = 10;

            // Declare the 1-Min Rule Variables
            int maxHeartRate;
            double maxTarget;
            double minTarget;

            // Declare the 10-Sec Rule Variables
            int tenSecMaxHeartRate;
            double tenSecMaxTarget;
            double tenSecMinTarget;

            // Input
            age = Convert.ToInt32(txtAge.Text);

            // 1-Min Processing
                maxHeartRate = MHRAge - age;
                minTarget = maxHeartRate * MINPERCENT;
                maxTarget = maxHeartRate * MAXPERCENT;

            // 1-Min Rule Output
                lblMaxHeartRate.Text = maxHeartRate.ToString("F0");
                lblMaxTargetZone.Text = maxTarget.ToString("F0");
                lblMinTargetZone.Text = minTarget.ToString("F0");

            // 10-Sec Processing
                tenSecMaxHeartRate = (maxHeartRate * RULE_TIME_CHANGE) / SEC_IN_MIN;
                tenSecMinTarget = (minTarget * RULE_TIME_CHANGE) / SEC_IN_MIN;
                tenSecMaxTarget = (maxTarget * RULE_TIME_CHANGE) / SEC_IN_MIN;

            // 10-Sec Rule Output
                lblHeartBeatNotExceed.Text = tenSecMaxHeartRate.ToString("F0");
                lblHeartBeat1.Text = tenSecMinTarget.ToString("F0");
                lblHeartBeat2.Text = tenSecMaxTarget.ToString("F0");
        }
        /// <summary>
        /// Calculates the user's BMI value
        /// </summary>
        private void CalcBMI()
        {
            // Declare the variables
            double weight;
            int heightFt;
            int heightInch;
            int convertedHeight;
            double bmi;
            double roundedBMI;
            const int CONVERT_HEIGHT_INCH = 12;
            const int WEIGHT_CONVERT = 703;

            // Input
            weight = Convert.ToDouble(txtWeight.Text);
            heightFt = Convert.ToInt32(txtHeightFt.Text);
            heightInch = Convert.ToInt32(txtHeightIn.Text);

            // Processing
            convertedHeight = (CONVERT_HEIGHT_INCH * heightFt) + heightInch;
            bmi = ((weight * WEIGHT_CONVERT) / convertedHeight) / convertedHeight;
            roundedBMI = Math.Round(bmi, 1, MidpointRounding.ToEven);
            
            // Output the BMI
            lblBMI.Text = bmi.ToString("F1");

            // Check for BMI indicators
            if (roundedBMI >= 40)
            {
                int anIntX = picBmiHighRisk.Location.X;
                int anIntY = picBmiHighRisk.Location.Y;
                int shiftAmt = (picBmiHighRisk.Size.Width - picIndBmi.Size.Width) / 2;

                anIntX += shiftAmt;
                anIntY += shiftAmt;
                picIndBmi.Location = new Point(anIntX, anIntY);
                picIndBmi.BackColor = colorSchemeIssue;
            }
            else if (roundedBMI >= 30)
            {
                int anIntX = picBmiObese.Location.X;
                int anIntY = picBmiObese.Location.Y;
                int shiftAmt = (picBmiObese.Size.Width - picIndBmi.Size.Width) / 2;

                anIntX += shiftAmt;
                anIntY += shiftAmt;
                picIndBmi.Location = new Point(anIntX, anIntY);
                picIndBmi.BackColor = colorSchemeIssue;
            }
            else if (roundedBMI >= 25)
            {
                int anIntX = picBmiOver.Location.X;
                int anIntY = picBmiOver.Location.Y;
                int shiftAmt = (picBmiOver.Size.Width - picIndBmi.Size.Width) / 2;

                anIntX += shiftAmt;
                anIntY += shiftAmt;
                picIndBmi.Location = new Point(anIntX, anIntY);
                picIndBmi.BackColor = colorSchemeWarning;
            }
            else if (roundedBMI >= 18.5)
            {
                int anIntX = picBmiHealthy.Location.X;
                int anIntY = picBmiHealthy.Location.Y;
                int shiftAmt = (picBmiHealthy.Size.Width - picIndBmi.Size.Width) / 2;

                anIntX += shiftAmt;
                anIntY += shiftAmt;
                picIndBmi.Location = new Point(anIntX, anIntY);
                picIndBmi.BackColor = colorSchemeNormal;
            }
            else
            {
                int anIntX = picBmiUnder.Location.X;
                int anIntY = picBmiUnder.Location.Y;
                int shiftAmt = (picBmiUnder.Size.Width - picIndBmi.Size.Width) / 2;

                anIntX += shiftAmt;
                anIntY += shiftAmt;
                picIndBmi.Location = new Point(anIntX, anIntY);
                picIndBmi.BackColor = colorSchemeNormal;
            }

        }
        /// <summary>
        /// Calculates the user's glucose level
        /// </summary>
        private void CalcGlucose()
        {
            // Declare the variable
            double glucose;

            // Input
            glucose = Convert.ToDouble(txtGlucose.Text);

            // Output the glucose level on screen
            lblGlucose.Text = glucose.ToString("F0");

            // Check for BMI indicators
            if (glucose>=126)
            {
                int anIntX = picGlucoseDiabetes.Location.X;
                int anIntY = picGlucoseDiabetes.Location.Y;
                int shiftAmt = (picGlucoseDiabetes.Size.Width - picIndGlucose.Size.Width) / 2;

                anIntX += shiftAmt;
                anIntY += shiftAmt;
                picIndGlucose.Location = new Point(anIntX, anIntY);
                picIndGlucose.BackColor = colorSchemeIssue;
            }
            else if (glucose >= 100)
            {
                int anIntX = picGlucosePreDiabetes.Location.X;
                int anIntY = picGlucosePreDiabetes.Location.Y;
                int shiftAmt = (picGlucosePreDiabetes.Size.Width - picIndGlucose.Size.Width) / 2;

                anIntX += shiftAmt;
                anIntY += shiftAmt;
                picIndGlucose.Location = new Point(anIntX, anIntY);
                picIndGlucose.BackColor = colorSchemeWarning;
            }
            else if (glucose >= 70)
            {
                int anIntX = picGlucoseNormal.Location.X;
                int anIntY = picGlucoseNormal.Location.Y;
                int shiftAmt = (picGlucoseNormal.Size.Width - picIndGlucose.Size.Width) / 2;

                anIntX += shiftAmt;
                anIntY += shiftAmt;
                picIndGlucose.Location = new Point(anIntX, anIntY);
                picIndGlucose.BackColor = colorSchemeNormal;
            }
            else
            {
                int anIntX = picGlucoseLow.Location.X;
                int anIntY = picGlucoseLow.Location.Y;
                int shiftAmt = (picGlucoseLow.Size.Width - picIndGlucose.Size.Width) / 2;

                anIntX += shiftAmt;
                anIntY += shiftAmt;
                picIndGlucose.Location = new Point(anIntX, anIntY);
                picIndGlucose.BackColor = colorSchemeNormal;
            }

        }

        private string CalcBloodPressureSystolic()
        {
            // Declare the variable
            int systolic;
            string systolicLevel;

            // Input
            systolic = Convert.ToInt32(txtBloodPressureSystolic.Text);

            // Output to screen
            lblBloodPressureSystolic.Text = systolic.ToString("F0");

            // Set glucose progress bar
            prgBpSystolic.Maximum = MaxSystolicBP;
            prgBpSystolic.Value = systolic;

            // Determine the systolic level
            if(systolic >= 160)
            {
                systolicLevel = "Stage 2";
            }
            else if(systolic >= 140)
            {
                systolicLevel = "Stage 1";
            }
            else if(systolic >= 120)
            {
                systolicLevel = "Pre";
            }
            else
            {
                systolicLevel = "Normal";
            }

            return systolicLevel;
        }

        private string CalcBloodPressureDiastolic()
        {
            // Declare the variable
            int diastolic;
            string diastolicLevel;

            // Input
            diastolic = Convert.ToInt32(txtBloodPressureDiastolic.Text);

            // Output to screen
            lblBloodPressureDiastolic.Text = diastolic.ToString("F0");

            // Set glucose progress bar
            prgBpDiastolic.Maximum = MaxDiastolicBP;
            prgBpDiastolic.Value = diastolic;

            // Determine the diastolic level
            if (diastolic >= 100)
            {
                diastolicLevel = "Stage 2";
            }
            else if (diastolic >= 90)
            {
                diastolicLevel = "Stage 1";
            }
            else if (diastolic >= 80)
            {
                diastolicLevel = "Pre";
            }
            else
            {
                diastolicLevel = "Normal";
            }

            return diastolicLevel;
        }
        /// <summary>
        /// Sets the overall hypertension level
        /// </summary>
        /// <param name="sysLevelName">Systolic Blood Pressure Level</param>
        /// <param name="diaLevelName">Diastolic Blood Pressure Level</param>
        private void SetOverallHypertensionLevel(string sysLevelName, string diaLevelName)
        {
            lblHyperStage2.ForeColor = Color.DimGray;
            lblHyperStage1.ForeColor = Color.DimGray;
            lblHyperPre.ForeColor = Color.DimGray;
            lblHyperNormal.ForeColor = Color.DimGray;

            if (sysLevelName == "Stage 2" || diaLevelName == "Stage 2")
            {
                lblHyperStage2.ForeColor = colorSchemeIssue;
            }
            else if (sysLevelName == "Stage 1" || diaLevelName == "Stage 1")
            {
                lblHyperStage1.ForeColor = colorSchemeIssue;
            }
            else if (sysLevelName == "Pre" || diaLevelName == "Pre")
            {
                lblHyperPre.ForeColor = colorSchemeWarning;
            }
            else
            {
                lblHyperNormal.ForeColor = colorSchemeNormal;

            }
        }

        private bool ValidateFormData()
        {
            // Declare the inputs to validate
            bool isValid = true;
            uint age;
            double weight;
            uint heightFt;
            uint heightIn;
            double glucose;
            uint systolic;
            uint diastolic;

            string msg = "Please resolve the error(s) below:\n\n";
            
            // Validate Age
            if(txtAge.Text == "")
            {
                msg += "Missing Age\n";
                txtAge.BackColor = Color.Pink;
                txtAge.SelectAll();
                txtAge.Focus();
                isValid = false;
            }
            else if(!uint.TryParse(txtAge.Text, out age))
            {
                msg += "Enter a valid age\n";
                txtAge.BackColor = Color.Pink;
                txtAge.SelectAll();
                txtAge.Focus();
                isValid = false;
            }

            // Validate Height in Feet
            if (txtHeightFt.Text == "")
            {
                msg += "Missing Height in Feet\n";
                txtHeightFt.BackColor = Color.Pink;
                txtHeightFt.SelectAll();
                txtHeightFt.Focus();
                isValid = false;
            }
            else if(!uint.TryParse(txtHeightFt.Text, out heightFt))
            {
                msg += "Enter a valid height in feet\n";
                txtHeightFt.BackColor = Color.Pink;
                txtHeightFt.SelectAll();
                txtHeightFt.Focus();
                isValid = false;
            }

            // Validate Height in Inches
            if (txtHeightIn.Text == "")
            {
                msg += "Missing Height in Inches\n";
                txtHeightIn.BackColor = Color.Pink;
                txtHeightIn.SelectAll();
                txtHeightIn.Focus();
                isValid = false;
            }
            else if (!uint.TryParse(txtHeightIn.Text, out heightIn))
            {
                msg += "Enter a valid height in feet\n";
                txtHeightIn.BackColor = Color.Pink;
                txtHeightIn.SelectAll();
                txtHeightIn.Focus();
                isValid = false;
            }

            // Validate Weight
            if (txtWeight.Text == "")
            {
                msg += "Missing Weight\n";
                txtWeight.BackColor = Color.Pink;
                txtWeight.SelectAll();
                txtWeight.Focus();
                isValid = false;
            }
            else if(!double.TryParse(txtWeight.Text, out weight))
            {
                msg += "Enter a valid weight\n";
                txtWeight.BackColor = Color.Pink;
                txtWeight.SelectAll();
                txtWeight.Focus();
                isValid = false;
            }

            // Validate BP Systolic
            if (txtBloodPressureSystolic.Text == "")
            {
                msg += "Missing Systolic Blood Pressure value\n";
                txtBloodPressureSystolic.BackColor = Color.Pink;
                txtBloodPressureSystolic.SelectAll();
                txtBloodPressureSystolic.Focus();
                isValid = false;
            }
            else if(!uint.TryParse(txtBloodPressureSystolic.Text, out systolic))
            {
                msg += "Enter a valid Systolic Blood Pressure value\n";
                txtBloodPressureSystolic.BackColor = Color.Pink;
                txtBloodPressureSystolic.SelectAll();
                txtBloodPressureSystolic.Focus();
                isValid = false;
            }

            // Validate BP Diastolic
            if (txtBloodPressureDiastolic.Text == "")
            {
                msg += "Missing Diastolic Blood Pressure value\n";
                txtBloodPressureDiastolic.BackColor = Color.Pink;
                txtBloodPressureDiastolic.SelectAll();
                txtBloodPressureDiastolic.Focus();
                isValid = false;
            }
            else if(!uint.TryParse(txtBloodPressureDiastolic.Text, out diastolic))
            {
                msg += "Enter a valid Diastolic Blood Pressure value\n";
                txtBloodPressureDiastolic.BackColor = Color.Pink;
                txtBloodPressureDiastolic.SelectAll();
                txtBloodPressureDiastolic.Focus();
                isValid = false;
            }

            // Valid 
            if (txtGlucose.Text == "")
            {
                msg += "Missing Glucose value\n";
                txtGlucose.BackColor = Color.Pink;
                txtGlucose.SelectAll();
                txtGlucose.Focus();
                isValid = false;
            }
            else if(!double.TryParse(txtGlucose.Text, out glucose))
            {
                msg += "Enter a valid Glucose value\n";
                txtGlucose.BackColor = Color.Pink;
                txtGlucose.SelectAll();
                txtGlucose.Focus();
                isValid = false;
            }

            // Handle the entire input validation
            if(!isValid)
            {
                MessageBox.Show(msg, "Input Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return isValid;

        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            if(sender is TextBox)
            {
                TextBox tb = (TextBox)sender;
                tb.BackColor = Color.White;
                DisplayDashboard(false);
            }
        }

        private bool GenEmergencyAlert()
        {
            bool shouldAlert = true;

            // Get values from textboxes
            int systolic = Convert.ToInt32(txtBloodPressureSystolic.Text);
            int diastolic = Convert.ToInt32(txtBloodPressureDiastolic.Text);
            double glucose = Convert.ToInt32(txtGlucose.Text);

            string msg = "This is a Medical Emergency Alert!!! \n\nBased on the provided information, please see the advisory alert " +
                "below and visit a physician as soon as possible:\n\n";
            
            // Handle the systolic alert
            if (systolic > MaxSystolicBP)
            {
                msg += "Your current Systolic Blood Pressure  value of " + systolic + " is above the recommended value of " + MaxSystolicBP +
                    ".\n\n";
                txtBloodPressureSystolic.Focus();
                txtBloodPressureSystolic.BackColor = Color.Pink;
                shouldAlert = false;
            }

            // Handle the distolic alert
            if (diastolic > MaxDiastolicBP)
            {
                msg += "Your current Diastolic Blood Pressure value of " + diastolic + " is above the recommended value of " + MaxDiastolicBP +
                    ".\n\n";
                txtBloodPressureDiastolic.Focus();
                txtBloodPressureDiastolic.BackColor = Color.Pink;
                shouldAlert = false;
            }

            // Handle the glucose alert
            if (glucose > GlucoseLevel)
            {
                msg += "Your current Glucose Level of " + glucose + " is above the recommended value of " + GlucoseLevel +
                    ".\n\n";
                txtGlucose.Focus();
                txtGlucose.BackColor = Color.Pink;
                shouldAlert = false;
            }

            // Handle the hypoglycemia alert
            if (glucose < 70)
            {
                msg += "Your current Glucose Level of " + glucose + " indicates that you are hypoglycemic.\n\n";
                txtGlucose.Focus();
                txtGlucose.BackColor = Color.Pink;
                shouldAlert = false;
            }


            // Handle the entire alert message
            if (!shouldAlert)
            {
                MessageBox.Show(msg, "Medical Emergency Alert",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                DisplayDashboard(false);
            }

            return shouldAlert;
        }

    }
}
