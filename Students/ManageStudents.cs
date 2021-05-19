using MaterialSkin.Controls;
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

namespace Time_Table_managemnt.Students
{
    public partial class ManageStudents : MaterialForm
    {

        readonly MaterialSkin.MaterialSkinManager materialskinmanager;
        public ManageStudents()
        {
            InitializeComponent();
            materialskinmanager = MaterialSkin.MaterialSkinManager.Instance;
            materialskinmanager.EnforceBackcolorOnAllComponents = true;
            materialskinmanager.AddFormToManage(this);
            materialskinmanager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            materialskinmanager.ColorScheme = new MaterialSkin.ColorScheme(MaterialSkin.Primary.BlueGrey800, MaterialSkin.Primary.Grey800, MaterialSkin.Primary.Indigo100, MaterialSkin.Accent.Pink200, MaterialSkin.TextShade.WHITE);

        }

        //database connection
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MYDatabase1.mdf;Integrated Security=True");

        public int studentID;
        public int TagID;
        public int SubjectID;
        public int LectureID;
        public int LocationID;
        public int WorkDayID;
        public int SessionID;
        public int i;

        private void materialTabSelector3_Click(object sender, EventArgs e)
        {

        }

        private void ManageStudents_Load(object sender, EventArgs e)
        {

        }

        private void UpdateStudentsGroupmaterialButton11_Click(object sender, EventArgs e)
        {
            if (studentID > 0)
            {

                SqlCommand cmd = new SqlCommand("update Students set AcdemicYear=@AcdemicYear,Programne=@Programne,GroupNumber=@GroupNumber,SubGroup=@SubGroup,GroupID=@GroupID,subGroupID=@subGroupID where Id=@Id", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@AcdemicYear", UpdateAcdemictxt.Text.ToString());
                cmd.Parameters.AddWithValue("@Programne", UpdateProgrammeTxt.Text.ToString());
                cmd.Parameters.AddWithValue("@GroupNumber", UpdateStudentsnumericUpDown4.Value);
                cmd.Parameters.AddWithValue("@SubGroup", UpdateStudentGroupnumericUpDown3.Value);
                cmd.Parameters.AddWithValue("@GroupID", UpdateSubGroup.Text);
                cmd.Parameters.AddWithValue("@subGroupID", UpdateGroupID.Text);
                cmd.Parameters.AddWithValue("@Id", this.studentID);


                con.Open();

                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("update sucessfully", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);

                getStudents();
               ;
            }
            else
            {

                MessageBox.Show("not updated", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void getStudents()
        {
            SqlCommand cmd = new SqlCommand("select * from Students", con);
            DataTable dt = new DataTable();

            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);

            con.Close();

            UpdateStudentdataGridView1.DataSource = dt;
        }

        private void DeleteStudentsmaterialButton10_Click(object sender, EventArgs e)
        {
            if (studentID > 0)
            {
                con.Close();
                SqlCommand cmd = new SqlCommand("delete from Students  where Id=@Id", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", this.studentID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                getStudents();
                clearStudents();
                MessageBox.Show("deleted sucessfully", "sucessfulluly", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Not deleted", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void clearStudents()
        {
            studentID = 0;
            UpdateAcdemictxt.SelectedIndex = -1;
            UpdateProgrammeTxt.SelectedIndex = -1;
            UpdateStudentsnumericUpDown4.ResetText();
            UpdateStudentGroupnumericUpDown3.ResetText();
            UpdateSubGroup.Clear();

            UpdateGroupID.Clear();

            UpdateAcdemictxt.Focus();
        }

        private void ClearData_Click(object sender, EventArgs e)
        {
            clearStudents();
        }

        private void MatchUpdatedID_Click(object sender, EventArgs e)
        {

            UpdateSubGroup.Text = UpdateAcdemictxt.Text + '.' + UpdateProgrammeTxt.Text + '.' + UpdateStudentsnumericUpDown4.Value.ToString();
            UpdateGroupID.Text = UpdateAcdemictxt.Text + '.' + UpdateProgrammeTxt.Text + '.' + UpdateStudentsnumericUpDown4.Value.ToString() + '.' + UpdateStudentGroupnumericUpDown3.Value.ToString();
        }
    }


    }

