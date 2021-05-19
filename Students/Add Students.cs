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
    public partial class Add_Students : MaterialForm
    {

        readonly MaterialSkin.MaterialSkinManager materialskinmanager;
        public Add_Students()
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

        private void Add_Students_Load(object sender, EventArgs e)
        {

        }

       
        private void AddStudents_Click(object sender, EventArgs e)
        {

            if (IsValidStudentsGroup())
            {

                con.Close();
                SqlCommand cmd = new SqlCommand("INSERT INTO Students(AcdemicYear,Programne,GroupNumber,SubGroup,GroupID,subGroupID) values (@AcdemicYear,@Programne,@GroupNumber,@SubGroup,@GroupID,@subGroupID)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@AcdemicYear", AdecmicYear_ComboBox.Text.ToString());
                cmd.Parameters.AddWithValue("@Programne", Programme_combo.Text.ToString());
                cmd.Parameters.AddWithValue("@GroupNumber", numericUpDown1.Value);
                cmd.Parameters.AddWithValue("@SubGroup", numericUpDown2.Value);
                cmd.Parameters.AddWithValue("@GroupID", GroupID1.Text);
                cmd.Parameters.AddWithValue("@subGroupID", SubGroupsID.Text);


                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("added  sucessfully", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                //Reset();


            }
        }

        private bool IsValidStudentsGroup()
        {
            if (AdecmicYear_ComboBox.Text == String.Empty)
            {


                MessageBox.Show("All Details Must Field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
