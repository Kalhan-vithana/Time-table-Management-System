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
using Time_Table_managemnt.Tags;

namespace Time_Table_managemnt
{
    public partial class Form1 : MaterialForm
    {
        readonly MaterialSkin.MaterialSkinManager materialskinmanager;
        public Form1()
        {
            InitializeComponent();
            materialskinmanager = MaterialSkin.MaterialSkinManager.Instance;
            materialskinmanager.EnforceBackcolorOnAllComponents = true;
            materialskinmanager.AddFormToManage(this);
            materialskinmanager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            materialskinmanager.ColorScheme = new MaterialSkin.ColorScheme(MaterialSkin.Primary.BlueGrey800, MaterialSkin.Primary.Grey800, MaterialSkin.Primary.Indigo100, MaterialSkin.Accent.Pink200, MaterialSkin.TextShade.WHITE);


            ManagePanelSize();

            pictureBox18.Show();

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
        private void Form1_Load(object sender, EventArgs e)
        {



            fillChart();
            //method of hide all panels
            HidePanel();


            //change Tags and Add panale size 
            ChangeSizeTagPanel();
            //code


            getStudents();
            GetTags();

            //enable false Genrare ID and Sub Group ID
            EnableGropIDs();

            ComboIndexMatch();

            getConsectivedata();



            checoboxrowConsectiveSession();
            checkboxParrelSesssonCheckbox();
            NonoverlappingCheckbox();
            AddLectureLocation();
            GetLocationCpnsective();

            Getnootavalibletime();
            loadComboLectureNames();
            loadComboSessionID();
            loadComboGroups();
        }

        private void EnableGropIDs()
        {
            GroupID1.Enabled = false;
            SubGroupsID.Enabled = false;
            UpdateSubGroup.Enabled = false;
            UpdateGroupID.Enabled = false;
        }


        void checoboxrowConsectiveSession()
        {

            DataGridViewCheckBoxColumn chkbox = new DataGridViewCheckBoxColumn();
            chkbox.HeaderText = "";
            chkbox.Width = 30;
            chkbox.Name = "checkboxColum";
            ConsecutiveDataGridView.Columns.Insert(0, chkbox);
        }

        void checkboxParrelSesssonCheckbox()
        {


            DataGridViewCheckBoxColumn chkbox = new DataGridViewCheckBoxColumn();
            chkbox.HeaderText = "";
            chkbox.Width = 30;
            chkbox.Name = "parallelCheckboxcolumn";
            ParellDatagridview.Columns.Insert(0, chkbox);
        }

        void NonoverlappingCheckbox()
        {


            DataGridViewCheckBoxColumn chkbox = new DataGridViewCheckBoxColumn();
            chkbox.HeaderText = "";
            chkbox.Width = 30;
            chkbox.Name = "Nonchecheckbox";
            datagridviewcoloum.Columns.Insert(0, chkbox);
        }


        void AddLectureLocation()
        {
            DataGridViewCheckBoxColumn chkbox = new DataGridViewCheckBoxColumn();
            chkbox.HeaderText = "";
            chkbox.Width = 30;
            chkbox.Name = "Locationcheckbox";
            SessionridView1.Columns.Insert(0, chkbox);

        }


        void GetLocationCpnsective()
        {
            DataGridViewCheckBoxColumn chkbox = new DataGridViewCheckBoxColumn();
            chkbox.HeaderText = "";
            chkbox.Width = 30;
            chkbox.Name = "Locationcheckbox1";
            conataGridView1.Columns.Insert(0, chkbox);

        }
        /*=============================================Get Students to Table =============================================================================== */


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


        /*=============================================  Add Students Group=============================================================================== */

        //call method
        Student s = new Student();

        private void AddStudents_Click(object sender, EventArgs e)
        {

            if (IsValidStudentsGroup())
            {

            
                s.AcdemicYear = AdecmicYear_ComboBox.Text;
                s.Programne = Programme_combo.Text;
                s.GroupNumber = numericUpDown1.Text;
                s.SubGroup = numericUpDown2.Text;
                s.GroupID = GroupID1.Text;
                s.subGroupID = GroupID1.Text;

                int SID = s.AddStd(s);

                if(SID > 0)
                {
                    MessageBox.Show("added sucesss", "sucessfull", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("please genarate your ID", "sucessfull", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("not added", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);


                }
            }

            loadComboGroups();
        }

        private bool IsValidStudentsGroup()
        {
            if (AdecmicYear_ComboBox.Text == String.Empty || Programme_combo.Text == String.Empty || numericUpDown1.Value == 0|| numericUpDown2.Value == 0)
            {


                MessageBox.Show("All Details Must Field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void ComboIndexMatch()
        {
            AdecmicYear_ComboBox.SelectedIndex = -1;
            Programme_combo.SelectedIndex = -1;
            AddTagCombo1.SelectedIndex = -1;
            ADDCodeComboBox1.SelectedIndex = -1;
            VeiwRelateTagComboBox3.SelectedIndex = -1;
            UpdateComboBox9.SelectedIndex = -1;
            UpdateTagCode.SelectedIndex = -1;
            UpdateRelatedTags.SelectedIndex = -1;
            UpdateAcdemictxt.SelectedIndex = -1;
            UpdateProgrammeTxt.SelectedIndex = -1;
            UpdateStudentsnumericUpDown4.ResetText();
            UpdateStudentGroupnumericUpDown3.ResetText();

        }

        private void Clear_Click(object sender, EventArgs e)
        {
            Reset();
        }


        /*============================================= Reset method  Students Group =============================================================================== */

        private void Reset()
        {
            studentID = 0;
            AdecmicYear_ComboBox.SelectedIndex = -1;
            Programme_combo.SelectedIndex = -1;
            numericUpDown1.ResetText();
            numericUpDown2.ResetText();
            GroupID1.Clear();
            SubGroupsID.Clear();



        }



        /*=============================================  Search Students Groups  =============================================================================== */

        private void SearchStudntGroup_TextChanged(object sender, EventArgs e)
        {


            if (SearchStudentsCombobox1.Text == "Id")
            {
                SqlDataAdapter sda = new SqlDataAdapter("select * from Students where Id like '" + SearchStudntGroup.Text + "%'", con);
                DataTable dt = new DataTable();


                sda.Fill(dt);
                UpdateStudentdataGridView1.DataSource = dt;



            }
            else if (SearchStudentsCombobox1.Text == "Programme")
            {

                SqlDataAdapter sda = new SqlDataAdapter("select * from Students where Programne like '" + SearchStudntGroup.Text + "%'", con);
                DataTable dt = new DataTable();

                sda.Fill(dt);

                UpdateStudentdataGridView1.DataSource = dt;
            }
        }


        private void GenGroupmaterialButton7_Click(object sender, EventArgs e)
        {
            ClearGroupID();
            SqlCommand cmd = new SqlCommand("select Id, CONCAT(AcdemicYear,'.',Programne,'.',GroupNumber) AS GroupID from Students ORDER BY Id DESC; ", con);
            DataTable dt = new DataTable();

            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            GropIDdataGridView1.DataSource = dt;

        }


        private void materialButton7_Click(object sender, EventArgs e)
        {

            ClearSubGroupID();
            SqlCommand cmd = new SqlCommand("select Id,CONCAT(AcdemicYear,'.',Programne,'.',GroupNumber,'.',SubGroup) AS SubGroup  from Students ORDER BY Id DESC ", con);
            DataTable dt = new DataTable();
            con.Open();

            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();

            SubdataGridView1.DataSource = dt;


        }
        //genarate Group ID gridveiw cell
        private void GropIDdataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            studentID = Convert.ToInt32(GropIDdataGridView1.SelectedRows[0].Cells[0].Value);
            GenarateIDGroup.Text = GropIDdataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            GroupID1.Text = GropIDdataGridView1.SelectedRows[0].Cells[1].Value.ToString();

        }


        /*=============================================  Add Students GroupsID   =============================================================================== */
        private void AddGroupID_Click(object sender, EventArgs e)
        {
            
                s.GroupID = GenarateIDGroup.Text;
                s.Id = this.studentID;
                bool success = s.ADDGenarateID(s);

                if (success == true)
                {
                    MessageBox.Show("update sucessfully", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    getStudents();
                }
                else
                {

                    MessageBox.Show("not sucessfully", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }  
           
        }


        /*=============================================  Add  Students Sub Groups =============================================================================== */
        private void SubGroupIDmaterialButton8_Click(object sender, EventArgs e)
        {

            if (studentID > 0)
            {

               
                s.subGroupID = SubGroupmaterial.Text;
                s.Id = this.studentID;
                bool success = s.ADDSubGenarateID(s);

                if (success == true)
                {
                    MessageBox.Show("added sucessfully", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    getStudents();
                }
                else
                {

                    MessageBox.Show("not added error", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }


            }
            else
            {

                MessageBox.Show("not added", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /*=============================================  Select SubGroupID data  ============================================================================== */
        private void SubdataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            studentID = Convert.ToInt32(SubdataGridView1.SelectedRows[0].Cells[0].Value);
            SubGroupmaterial.Text = SubdataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            SubGroupsID.Text = SubdataGridView1.SelectedRows[0].Cells[1].Value.ToString();
        }


        /*============================================= Select  GroupID data =============================================================================== */
        private void UpdateStudentdataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            studentID = Convert.ToInt32(UpdateStudentdataGridView1.SelectedRows[0].Cells[0].Value);
            UpdateAcdemictxt.Text = UpdateStudentdataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            UpdateProgrammeTxt.Text = UpdateStudentdataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            UpdateStudentsnumericUpDown4.Text = UpdateStudentdataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            UpdateStudentGroupnumericUpDown3.Text = UpdateStudentdataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            UpdateSubGroup.Text = UpdateStudentdataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            UpdateGroupID.Text = UpdateStudentdataGridView1.SelectedRows[0].Cells[6].Value.ToString();


        }


        /*============================================= Update Students Groups  =============================================================================== */
        private void UpdateStudentsGroupmaterialButton11_Click(object sender, EventArgs e)
        {
 
                s.AcdemicYear = UpdateAcdemictxt.Text;
                s.Programne = UpdateProgrammeTxt.Text;
                s.GroupNumber = UpdateStudentsnumericUpDown4.Text;
                s.SubGroup = UpdateStudentGroupnumericUpDown3.Text;
                s.GroupID = UpdateSubGroup.Text;
                s.subGroupID = UpdateGroupID.Text;
                s.Id = this.studentID;
                bool success = s.update(s);

                if(success == true)
                {
                    MessageBox.Show("addded sucessfully  ", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    getStudents();
                     Reset();
            }
                else
                {

                    MessageBox.Show("not adedd", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

        }

        /*=============================================    Delete Students Groups   =============================================================================== */
        private void DeleteStudentsmaterialButton10_Click(object sender, EventArgs e)
        {
            if (studentID > 0)
            {
                bool success = false;
                /* con.Close();
                 SqlCommand cmd = new SqlCommand("delete from Students  where Id=@Id", con);
                 cmd.CommandType = CommandType.Text;
                 cmd.Parameters.AddWithValue("@Id", this.studentID);
                 con.Open();
                 cmd.ExecuteNonQuery();
                 con.Close();

                 getStudents();
                 clearStudents();*/

                s.Id = this.studentID;
                success = s.delete(s);
                if (success == true)
                {
                    MessageBox.Show("deleted sucessfully", "sucessfulluly", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    getStudents();
                }

            }


            else
            {
                MessageBox.Show("Not deleted", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }


        /*=============================================   Clear Students Groups   =============================================================================== */
        private void ClearData_Click(object sender, EventArgs e)
        {
            clearStudents();
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


        /*=============================================   Match Updated ID ================================================================================ */


        private void MatchUpdatedID_Click(object sender, EventArgs e)
        {

            UpdateSubGroup.Text = UpdateAcdemictxt.Text + '.' + UpdateProgrammeTxt.Text + '.' + UpdateStudentsnumericUpDown4.Value.ToString();
            UpdateGroupID.Text = UpdateAcdemictxt.Text + '.' + UpdateProgrammeTxt.Text + '.' + UpdateStudentsnumericUpDown4.Value.ToString() + '.' + UpdateStudentGroupnumericUpDown3.Value.ToString();
        }


        /*=============================================   Add Tags     =============================================================================== */


        Tag t = new Tag();
        private void SaveTag_Click(object sender, EventArgs e)
        {
            if (IsTagValid())
            {

                t.SubjectName = AddTagCombo1.Text.ToString();
                t.SubjectCode = ADDCodeComboBox1.Text.ToString();
                t.RelatedTags = VeiwRelateTagComboBox3.Text.ToString();
               

                int TID = t.addTag(t);

                if (TID > 0)
                {
                    MessageBox.Show("added sucess", "sucessfull", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                    GetTags();
                    
                }
                else
                {
                     MessageBox.Show("not added", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
        }
        //validation
        private bool IsTagValid()
        {

            if (AddTagCombo1.Text == String.Empty || ADDCodeComboBox1.Text == String.Empty || VeiwRelateTagComboBox3.Text == String.Empty)
            {


                MessageBox.Show("All Details Must Field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }


        /*============================================  Get Tags data in Table  =============================================================================== */
        private void GetTags()
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("select * from Tags", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);



            TagdataGridView.DataSource = dt;
        }


        /*=============================================  Search Tags   =============================================================================== */
        private void SearchTags_TextChanged(object sender, EventArgs e)
        {

            if (SearchTagsCombo1.Text == "ID")
            {
                SqlDataAdapter sda = new SqlDataAdapter("select * from Tags where Id like '" + SearchTags.Text + "%'", con);
                DataTable dt = new DataTable();


                sda.Fill(dt);
                TagdataGridView.DataSource = dt;



            }
            else if (SearchTagsCombo1.Text == "Subject name")
            {

                SqlDataAdapter sda = new SqlDataAdapter("select * from Tags where SubjectName like '" + SearchTags.Text + "%'", con);
                DataTable dt = new DataTable();

                sda.Fill(dt);

                TagdataGridView.DataSource = dt;
            }
        }




        /*=============================================   Select TagDatagridView   =============================================================================== */
        private void TagdataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            TagID = Convert.ToInt32(TagdataGridView.SelectedRows[0].Cells[0].Value);
            UpdateComboBox9.Text = TagdataGridView.SelectedRows[0].Cells[1].Value.ToString();
            UpdateTagCode.Text = TagdataGridView.SelectedRows[0].Cells[2].Value.ToString();
            UpdateRelatedTags.Text = TagdataGridView.SelectedRows[0].Cells[3].Value.ToString();


            //var val = TagdataGridView.SelectedRows[0].Cells[3].Value.ToString();

            //UpdateRelatedTags.Items.Add(val);
            //UpdateRelatedTags.SelectedIndex = -1;

        }



        /*=============================================   Update Students Tags   =============================================================================== */
        private void UpdateTagButton_Click(object sender, EventArgs e)
        {




            t.SubjectName = UpdateComboBox9.Text;
            t.SubjectCode = UpdateTagCode.Text;
            t.RelatedTags = UpdateRelatedTags.Text;

                t.Id = this.TagID;
                bool success = t.updateTag(t);

                if (success == true)
                {
                    MessageBox.Show("update  sucessfull", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
             
                    Reset();
                GetTags();
            }
                else
                {

                    MessageBox.Show("not updated", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
          
        }

        /*=============================================  Delete Students Tags   =============================================================================== */
        private void DeleteTagButton_Click(object sender, EventArgs e)
        {

            bool success = false;

            if (TagID > 0)
            {
                t.Id = this.TagID;
                success = t.deleteTag(t);
                if (success == true)
                {
                    MessageBox.Show("deleted sucessfully", "sucessfulluly", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GetTags();
                }

            }

            else
            {
                MessageBox.Show("Not deleted", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

         
        }


        //Clear Tags  Method

        private void ClearTags()
        {
            TagID = 0;
            UpdateComboBox9.SelectedIndex = -1;
            UpdateTagCode.SelectedIndex = -1;
            UpdateRelatedTags.SelectedIndex = -1;

            UpdateComboBox9.Focus();
        }

        private void ClearTag_Click(object sender, EventArgs e)
        {
            ClearAddedTags();

        }

        private void ClearAddedTags()
        {
            AddTagCombo1.SelectedIndex = -1;
            ADDCodeComboBox1.SelectedIndex = -1;
            VeiwRelateTagComboBox3.SelectedIndex = -1;
        }

        //====================================================Mebmer 1 Sprint2 =================================================



        //====================================================-Add Consective Sprint 2====================================================
        public void getConsectivedata()
        {

            con.Close();
            SqlCommand cmd = new SqlCommand("select * from session", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();

            dt.Load(sdr);
            con.Close();

            ConsecutiveDataGridView.DataSource = dt;



        }


        //====================================================-Add Consective session====================================================
        private void materialButton21_Click(object sender, EventArgs e)
        {


            if (selectcheckbox())
            {
                foreach (DataGridViewRow dr in ConsecutiveDataGridView.Rows)
                {


                    bool checkboxselected = Convert.ToBoolean(dr.Cells["checkboxColum"].Value);
                    if (checkboxselected)
                    {

                        con.Close();
                        SqlCommand cmd = new SqlCommand("INSERT INTO consective(Lecture,Tags,SubjectCode,Groups,Subject,Duration,NumofStudents) values(@Lecture,@Tags,@SubjectCode,@Groups,@Subject,@Duration,@NumofStudents)", con);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Lecture", dr.Cells[2].Value);
                        cmd.Parameters.AddWithValue("@Tags", dr.Cells[3].Value);
                        cmd.Parameters.AddWithValue("@SubjectCode", dr.Cells[4].Value);
                        cmd.Parameters.AddWithValue("@Groups", dr.Cells[5].Value);
                        cmd.Parameters.AddWithValue("@Subject", dr.Cells[6].Value);
                        cmd.Parameters.AddWithValue("@Duration", dr.Cells[7].Value);
                        cmd.Parameters.AddWithValue("@NumofStudents", dr.Cells[8].Value);



                        con.Open();

                        cmd.ExecuteNonQuery();
                        con.Close();
                        loadconsectiverow();

                    }


                }


            }

        }


        //check the checkbox------------------------------------------------------------------------------------------

        private bool selectcheckbox()
        {


            foreach (DataGridViewRow row in ConsecutiveDataGridView.Rows)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)row.Cells["checkboxColum"];

                if (Convert.ToBoolean(cell.Value))
                {
                    MessageBox.Show("added consective session", "sucessfull", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return true;
                }
            }
            MessageBox.Show("check the checkbox", "sucessfull", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;

        }

        //load session------------------------------------------------------------------

        public void loadconsectiverow()
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("select * from consective", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();

            dt.Load(sdr);
            con.Close();

            CondataGridView1.DataSource = dt;

        }

        //search session----------------------------------

        private void materialTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (Searchconsectivecombo.Text == "ID")
            {
                SqlDataAdapter sda = new SqlDataAdapter("select * from session where Id like '" + materialTextBox2.Text + "%'", con);
                DataTable dt = new DataTable();


                sda.Fill(dt);
                ConsecutiveDataGridView.DataSource = dt;



            }
            else if (Searchconsectivecombo.Text == "Subject name")
            {

                SqlDataAdapter sda = new SqlDataAdapter("select * from session where Subject like '" + materialTextBox2.Text + "%'", con);
                DataTable dt = new DataTable();

                sda.Fill(dt);

                ConsecutiveDataGridView.DataSource = dt;
            }
        }


        //====================================================-Add Parelle Sprint 2====================================================


       
        public void insertParrellSession()
        {
            if (checkselectcheckboxParelll())
            {
                foreach (DataGridViewRow dr in ParellDatagridview.Rows)
                {


                    bool checkboxselected = Convert.ToBoolean(dr.Cells["parallelCheckboxcolumn"].Value);
                    if (checkboxselected)
                    {

                        con.Close();
                        SqlCommand cmd = new SqlCommand("INSERT INTO parallelSession(Lecture,Tags,SubjectCode,Groups,Subject,Duration,NumofStudents) values(@Lecture,@Tags,@SubjectCode,@Groups,@Subject,@Duration,@NumofStudents)", con);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Lecture", dr.Cells[2].Value);
                        cmd.Parameters.AddWithValue("@Tags", dr.Cells[3].Value);
                        cmd.Parameters.AddWithValue("@SubjectCode", dr.Cells[4].Value);
                        cmd.Parameters.AddWithValue("@Groups", dr.Cells[5].Value);
                        cmd.Parameters.AddWithValue("@Subject", dr.Cells[6].Value);
                        cmd.Parameters.AddWithValue("@Duration", dr.Cells[7].Value);
                        cmd.Parameters.AddWithValue("@NumofStudents", dr.Cells[8].Value);



                        con.Open();

                        cmd.ExecuteNonQuery();
                        con.Close();
                        loadParallelROw();

                    }

                }
            }

        }


        //check parealll session-------------------------------------------------
        private bool checkselectcheckboxParelll()
        {


            foreach (DataGridViewRow row in ParellDatagridview.Rows)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)row.Cells["parallelCheckboxcolumn"];

                if (Convert.ToBoolean(cell.Value))
                {
                    MessageBox.Show("added Parallel session", "sucessfull", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return true;
                }
            }
            MessageBox.Show("check the checkbox", "sucessfull", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;

        }

        //load parell session

        public void loadParallelROw()
        {


            con.Close();
            SqlCommand cmd = new SqlCommand("select * from parallelSession", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();

            dt.Load(sdr);
            con.Close();

            PaldataGridView1.DataSource = dt;
        }

        //loadd sessions------------------------------------------
        public void GetParrallSessionData()
        {

            con.Close();
            SqlCommand cmd = new SqlCommand("select * from session", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();

            dt.Load(sdr);
            con.Close();

            ParellDatagridview.DataSource = dt;

        }

        //-------------------------------search parell session---------------------------------------
        private void materialTextBox3_TextChanged(object sender, EventArgs e)
        {

            if (materialComboBox2.Text == "ID")
            {
                SqlDataAdapter sda = new SqlDataAdapter("select * from session where Id like '" + materialTextBox3.Text + "%'", con);
                DataTable dt = new DataTable();


                sda.Fill(dt);
                ParellDatagridview.DataSource = dt;



            }
            else if (materialComboBox2.Text == "Subject name")
            {

                SqlDataAdapter sda = new SqlDataAdapter("select * from session where Subject like '" + materialTextBox3.Text + "%'", con);
                DataTable dt = new DataTable();

                sda.Fill(dt);

                ParellDatagridview.DataSource = dt;
            }
        }




        //====================================================-Add -Non Overlapping Sprint 2====================================================

       

        public void insertNonoverlappingSession()
        {
            if (checkselectcheckboxNonooverlap())
            {
                foreach (DataGridViewRow dr in datagridviewcoloum.Rows)
                {


                    bool checkboxselected = Convert.ToBoolean(dr.Cells["Nonchecheckbox"].Value);
                    if (checkboxselected)
                    {

                        con.Close();
                        SqlCommand cmd = new SqlCommand("INSERT INTO nonoverlap(Lecture,Tags,SubjectCode,Groups,Subject,Duration,NumofStudents) values(@Lecture,@Tags,@SubjectCode,@Groups,@Subject,@Duration,@NumofStudents)", con);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Lecture", dr.Cells[2].Value);
                        cmd.Parameters.AddWithValue("@Tags", dr.Cells[3].Value);
                        cmd.Parameters.AddWithValue("@SubjectCode", dr.Cells[4].Value);
                        cmd.Parameters.AddWithValue("@Groups", dr.Cells[5].Value);
                        cmd.Parameters.AddWithValue("@Subject", dr.Cells[6].Value);
                        cmd.Parameters.AddWithValue("@Duration", dr.Cells[7].Value);
                        cmd.Parameters.AddWithValue("@NumofStudents", dr.Cells[8].Value);



                        con.Open();

                        cmd.ExecuteNonQuery();
                        con.Close();
                        GetnonoverlappingData();

                    }

                }


            }

        }
        //-----------check box check Non Overlapping  -------------------------------------------------------------------------------------------------


        private bool checkselectcheckboxNonooverlap()
        {


            foreach (DataGridViewRow row in datagridviewcoloum.Rows)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)row.Cells["Nonchecheckbox"];

                if (Convert.ToBoolean(cell.Value))
                {
                    MessageBox.Show("added overlapping session", "sucessfull", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return true;
                }
            }
            MessageBox.Show("check the checkbox", "sucessfull", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;

        }

        //----------load session Sprint 2-------------------------------------------------------------------------------------------------


        public void GetnonoverlappingSession()
        {

            con.Close();
            SqlCommand cmd = new SqlCommand("select * from session", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();

            dt.Load(sdr);
            con.Close();

            datagridviewcoloum.DataSource = dt;

        }


        //-----------retrive Non Overlapping ------------------------------------------------------------------------------------------------


        public void GetnonoverlappingData()
        {

            con.Close();
            SqlCommand cmd = new SqlCommand("select * from nonoverlap", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();

            dt.Load(sdr);
            con.Close();

            NondataGridView1.DataSource = dt;
        }
        //--------------



        /*=============================================Addd Subjects =============================================================================== */

        private void SaveBtnSub_Click(object sender, EventArgs e)
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("INSERT INTO Subject(offerdYear,offerdSemester,subjectname,subjectCode,lecHours,labHours,TuteHours,evhours) values (@offerdYear,@offerdSemester,@subjectname,@subjectCode,@lecHours,@labHours,@TuteHours,@evhours)", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@offerdYear", materialComboBox24.Text.ToString());
            cmd.Parameters.AddWithValue("@offerdSemester", materialComboBox25.Text.ToString());
            cmd.Parameters.AddWithValue("@subjectname", materialTextBox27.Text);
            cmd.Parameters.AddWithValue("@subjectCode", materialTextBox30.Text);
            cmd.Parameters.AddWithValue("@lecHours", numericUpDown14.Value);
            cmd.Parameters.AddWithValue("@labHours", numericUpDown15.Value);
            cmd.Parameters.AddWithValue("@TuteHours", numericUpDown16.Value);
            cmd.Parameters.AddWithValue("@evhours", numericUpDown17.Value);

            con.Open();

            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("added");

            getSubjects();
            selectSubject();



        }

        private void getSubjects()
        {

            SqlCommand cmd = new SqlCommand("select * from Subject", con);
            DataTable dt = new DataTable();

            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();


            SubjectGridView1.DataSource = dt;

        }

        /*=============================================  Clear Subjects ============================================================================== */



        private void ClearBtnSub_Click(object sender, EventArgs e)
        {
            studentID = 0;
            materialComboBox24.SelectedIndex = -1;
            materialComboBox25.SelectedIndex = -1;
            materialTextBox27.Clear();
            materialTextBox30.Clear();
            numericUpDown14.ResetText();
            numericUpDown15.ResetText();
            numericUpDown16.ResetText();
            numericUpDown17.ResetText();



        }


        /*============================================  select Subjects =============================================================================== */



        private void SubjectGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            SubjectID = Convert.ToInt32(SubjectGridView1.SelectedRows[0].Cells[0].Value);
            OfferdYear.Text = SubjectGridView1.SelectedRows[0].Cells[1].Value.ToString();
            offerdSem.Text = SubjectGridView1.SelectedRows[0].Cells[2].Value.ToString();
            Subjectname.Text = SubjectGridView1.SelectedRows[0].Cells[3].Value.ToString();
            SubjectCode.Text = SubjectGridView1.SelectedRows[0].Cells[4].Value.ToString();
            numericUpDown10.Text = SubjectGridView1.SelectedRows[0].Cells[5].Value.ToString();
            numericUpDown11.Text = SubjectGridView1.SelectedRows[0].Cells[6].Value.ToString();
            numericUpDown12.Text = SubjectGridView1.SelectedRows[0].Cells[7].Value.ToString();
            numericUpDown13.Text = SubjectGridView1.SelectedRows[0].Cells[7].Value.ToString();




        }


        /*============================================= Update  Subjects  =============================================================================== */

        private void materialButtonSaveManage_Click(object sender, EventArgs e)
        {


            SqlCommand cmd = new SqlCommand("update Subject set offerdYear=@offerdYear,offerdSemester=@offerdSemester,subjectname=@subjectname,subjectCode=@subjectCode,lecHours=@lecHours,labHours=@labHours,TuteHours=@TuteHours,evhours=@evhours where Id=@Id", con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@offerdYear", OfferdYear.Text.ToString());
            cmd.Parameters.AddWithValue("@offerdSemester", offerdSem.Text.ToString());
            cmd.Parameters.AddWithValue("@subjectname", Subjectname.Text);
            cmd.Parameters.AddWithValue("@subjectCode", SubjectCode.Text);
            cmd.Parameters.AddWithValue("@lecHours", numericUpDown10.Value);
            cmd.Parameters.AddWithValue("@labHours", numericUpDown11.Value);
            cmd.Parameters.AddWithValue("@TuteHours", numericUpDown12.Value);
            cmd.Parameters.AddWithValue("@evhours", numericUpDown13.Value);

            cmd.Parameters.AddWithValue("@Id", SubjectID);

            con.Open();


            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("updated");
            getSubjects();
        }


        /*=============================================    Delete Subjects    =============================================================================== */

        private void materialButtonManagesub_Click(object sender, EventArgs e)
        {

            SqlCommand cmd = new SqlCommand("delete from Subject  where Id=@Id", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Id", SubjectID);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            getSubjects();

            MessageBox.Show("deleted");
        }

        /*=============================================Clear Subjects =============================================================================== */

        private void materialButtonManage_Click(object sender, EventArgs e)
        {
            OfferdYear.Clear();
            offerdSem.Clear();
            Subjectname.Clear();
            numericUpDown10.ResetText();
            numericUpDown11.ResetText();
            numericUpDown12.ResetText();
            numericUpDown13.ResetText();
        }

        /*=============================================Add Lecture =============================================================================== */

        private void LecSave_Click(object sender, EventArgs e)
        {



            con.Close();
            SqlCommand cmd = new SqlCommand("INSERT INTO Lecture(Lecname,LecID,Faculty,Department,Center,Bulding,Leval,Rank) values (@Lecname,@LecID,@Faculty,@Department,@Center,@Bulding,@Leval,@Rank)", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Lecname", LecName.Text);
            cmd.Parameters.AddWithValue("@LecID", idLecs.Text);
            cmd.Parameters.AddWithValue("@Faculty", materialComboBox17.Text);
            cmd.Parameters.AddWithValue("@Department", materialComboBox18.Text);
            cmd.Parameters.AddWithValue("@Center", materialComboBox19.Text);
            cmd.Parameters.AddWithValue("@Bulding", materialComboBox20.Text);
            cmd.Parameters.AddWithValue("@Leval", materialComboBox21.Text);
            cmd.Parameters.AddWithValue("@Rank", Rank.Text);


            con.Open();

            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Added ");
            getLecData();
            SelectedLectrues();
            loadComboLectureNames();

        }

        private void getLecData()
        {


            SqlCommand cmd = new SqlCommand("select * from  Lecture", con);
            DataTable dt = new DataTable();

            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();


            LecdataGridView.DataSource = dt;
        }


        //clear Lecture
        private void LecClear_Click(object sender, EventArgs e)
        {



            LecName.Text = "";
            idLecs.Text = "";
            materialComboBox17.SelectedIndex = -1;
            materialComboBox18.SelectedIndex = -1;
            materialComboBox19.SelectedIndex = -1;
            materialComboBox20.SelectedIndex = -1;
            materialComboBox21.SelectedIndex = -1;
            Rank.Text = "";


        }

        //Geta Data
        private void LecdataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LectureID = Convert.ToInt32(LecdataGridView.SelectedRows[0].Cells[0].Value);
            Lec1UP.Text = LecdataGridView.SelectedRows[0].Cells[1].Value.ToString();
            Lec1UP2.Text = LecdataGridView.SelectedRows[0].Cells[2].Value.ToString();
            Lec1UP3.Text = LecdataGridView.SelectedRows[0].Cells[3].Value.ToString();
            Lec1UP4.Text = LecdataGridView.SelectedRows[0].Cells[4].Value.ToString();
            Lec1UP5.Text = LecdataGridView.SelectedRows[0].Cells[5].Value.ToString();
            Lec1UP6.Text = LecdataGridView.SelectedRows[0].Cells[6].Value.ToString();
            Lec1UP7.Text = LecdataGridView.SelectedRows[0].Cells[7].Value.ToString();
            Lec1UP8.Text = LecdataGridView.SelectedRows[0].Cells[8].Value.ToString();
        }


        /*============================================= Update Lectures =============================================================================== */



        private void Updatelec_Click(object sender, EventArgs e)
        {

            con.Close();
            SqlCommand cmd = new SqlCommand("update Lecture set Lecname=@Lecname,LecID=@LecID,Faculty=@Faculty,Department=@Department,Center=@Center,Bulding=@Bulding,Leval=@Leval,Rank=@Rank  where Id=@Id", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Lecname", Lec1UP.Text);
            cmd.Parameters.AddWithValue("@LecID", Lec1UP2.Text);
            cmd.Parameters.AddWithValue("@Faculty", Lec1UP3.Text);
            cmd.Parameters.AddWithValue("@Department", Lec1UP4.Text);
            cmd.Parameters.AddWithValue("@Center", Lec1UP5.Text);
            cmd.Parameters.AddWithValue("@Bulding", Lec1UP6.Text);
            cmd.Parameters.AddWithValue("@Leval", Lec1UP7.Text);
            cmd.Parameters.AddWithValue("@Rank", Lec1UP8.Text);
            cmd.Parameters.AddWithValue("@Id", LectureID);

            con.Open();

            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Updated ");
            getLecData();
        }

        /*============================================= Delete Lectures =============================================================================== */



        private void materiaDeletelec_Click(object sender, EventArgs e)
        {

            SqlCommand cmd = new SqlCommand("delete from Lecture  where Id=@Id", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Id", this.LectureID);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            getLecData();

            MessageBox.Show("Deleted ");
        }

        /*============================================= Clear Lectures =============================================================================== */



        private void Clearlec_Click(object sender, EventArgs e)
        {
            Lec1UP.Text = "";
            Lec1UP2.Text = "";
            Lec1UP3.Text = "";
            Lec1UP4.Text = "";
            Lec1UP5.Text = "";
            Lec1UP6.Text = "";
            Lec1UP7.Text = "";
            Lec1UP8.Text = "";
        }



        /*============================================= Generate Rank =============================================================================== */


        private void LecRank_Click(object sender, EventArgs e)
        {
            Rank.Text = "";
            Rank.Text = idLecs.Text + '.' + materialComboBox21.Text;

        }




        /*============================================= Add Working Days Hours =============================================================================== */


        private void addworksavebut_Click(object sender, EventArgs e)
        {
            String Day = "";
            String Mon = "";
            String Tue = "";
            String Wen = "";
            String Thu = "";
            String Fri = "";
            String Sat = "";
            String Sun = "";





            if (AWmaterialCheckbox1.Checked)
            {

                Mon = "Monday";
            }

            if (AWmaterialCheckbox2.Checked)
            {

                Tue = "Tuesday";
            }
            if (AWmaterialCheckbox6.Checked)
            {

                Wen = "wednesday";
            }
            if (AWmaterialCheckbox4.Checked)
            {

                Thu = "ThursDay";
            }
            if (AWmaterialCheckbox7.Checked)
            {

                Fri = "Friday";
            }
            if (AWmaterialCheckbox3.Checked)
            {

                Sun = "Saturday";
            }
            if (AWmaterialCheckbox5.Checked)
            {

                Sun = "Sunday";
            }
            else
            {
                Day = "";
            }

            con.Close();

            SqlCommand cmd = new SqlCommand("INSERT INTO Work(NoWorkingDays,WorkingHours,Mon,Tue,Wen,Thu,Fri,Sat,Sun) values(@NoWorkingDays,@WorkingHours,@Mon,@Tue,@Wen,@Thu,@Fri,@Sat,@Sun)", con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@NoWorkingDays", AWmaterialComboBox10.Text.ToString());
            cmd.Parameters.AddWithValue("@WorkingHours", HoursMinitsCombo.Text.ToString());
            cmd.Parameters.AddWithValue("@Mon", Mon);
            cmd.Parameters.AddWithValue("@Tue", Tue);
            cmd.Parameters.AddWithValue("@Wen", Wen);
            cmd.Parameters.AddWithValue("@Thu", Thu);
            cmd.Parameters.AddWithValue("@Fri", Fri);
            cmd.Parameters.AddWithValue("@Sat", Sat);
            cmd.Parameters.AddWithValue("@Sun", Sun);

            con.Open();

            cmd.ExecuteNonQuery();
            con.Close();


            MessageBox.Show("added");

            getWorkingDays();

        }

        private void getWorkingDays()
        {


            SqlCommand cmd = new SqlCommand("select * from Work", con);
            DataTable dt = new DataTable();

            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();


            WorkingDatdataGridView.DataSource = dt;
        }


        /*=============================================   Clear Working Days    =============================================================================== */


        private void addworkclearbut_Click(object sender, EventArgs e)
        {
            AWmaterialComboBox10.SelectedIndex = -1;
            HoursMinitsCombo.SelectedIndex = -1;
            AWmaterialCheckbox1.Checked = false;
            AWmaterialCheckbox2.Checked = false;
            AWmaterialCheckbox3.Checked = false;
            AWmaterialCheckbox4.Checked = false;
            AWmaterialCheckbox5.Checked = false;
            AWmaterialCheckbox6.Checked = false;
            AWmaterialCheckbox7.Checked = false;

        }






        /*=============================================   Select work Days    =============================================================================== */

        private void WorkingDatdataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            WorkDayID = Convert.ToInt32(WorkingDatdataGridView.SelectedRows[0].Cells[0].Value);
            MWmaterialComboBox10.Text = WorkingDatdataGridView.SelectedRows[0].Cells[1].Value.ToString();
            materialComboBox8.Text = WorkingDatdataGridView.SelectedRows[0].Cells[2].Value.ToString();
            if (WorkingDatdataGridView.SelectedRows[0].Cells[3].Value.ToString() == "Monday")
            {
                MWmaterialCheckbox1.Checked = true;


            }
            if (WorkingDatdataGridView.SelectedRows[0].Cells[4].Value.ToString() == "Tuesday")
            {
                MWmaterialCheckbox7.Checked = true;


            }
            if (WorkingDatdataGridView.SelectedRows[0].Cells[5].Value.ToString() == "wednesday")
            {
                MWmaterialCheckbox6.Checked = true;


            }
            if (WorkingDatdataGridView.SelectedRows[0].Cells[6].Value.ToString() == "ThursDay")
            {
                MWmaterialCheckbox5.Checked = true;


            }
            if (WorkingDatdataGridView.SelectedRows[0].Cells[7].Value.ToString() == "Friday")
            {
                MWmaterialCheckbox4.Checked = true;


            }
            if (WorkingDatdataGridView.SelectedRows[0].Cells[8].Value.ToString() == "Saturday")
            {
                MWmaterialCheckbox2.Checked = true;


            }
            if (WorkingDatdataGridView.SelectedRows[0].Cells[9].Value.ToString() == "Sunday")
            {
                MWmaterialCheckbox3.Checked = true;


            }


            //MWmaterialCheckbox1.Text = WorkingDatdataGridView.SelectedRows[0].Cells[3].Value.ToString();
            /* MWmaterialCheckbox7.Text = WorkingDatdataGridView.SelectedRows[0].Cells[4].Value.ToString();
             MWmaterialCheckbox6.Text = WorkingDatdataGridView.SelectedRows[0].Cells[5].Value.ToString();
             MWmaterialCheckbox5.Text = WorkingDatdataGridView.SelectedRows[0].Cells[6].Value.ToString();
             MWmaterialCheckbox4.Text = WorkingDatdataGridView.SelectedRows[0].Cells[7].Value.ToString();
             MWmaterialCheckbox2.Text = WorkingDatdataGridView.SelectedRows[0].Cells[8].Value.ToString();
             MWmaterialCheckbox3.Text = WorkingDatdataGridView.SelectedRows[0].Cells[9].Value.ToString();*/

        }



        /*=============================================   Update Work Days    =============================================================================== */


        private void manageworkupdatebut_Click(object sender, EventArgs e)
        {

            String Day = "";
            String Mon = "";
            String Tue = "";
            String Wen = "";
            String Thu = "";
            String Fri = "";
            String Sat = "";
            String Sun = "";





            if (MWmaterialCheckbox1.Checked)
            {

                Mon = "Monday";
            }

            if (MWmaterialCheckbox7.Checked)
            {

                Tue = "Tuesday";
            }
            if (MWmaterialCheckbox6.Checked)
            {

                Wen = "wednesday";
            }
            if (MWmaterialCheckbox5.Checked)
            {

                Thu = "ThursDay";
            }
            if (MWmaterialCheckbox4.Checked)
            {

                Fri = "Friday";
            }
            if (MWmaterialCheckbox2.Checked)
            {

                Sun = "Saturday";
            }
            if (MWmaterialCheckbox3.Checked)
            {

                Sun = "Sunday";
            }
            else
            {
                Day = "";
            }


            SqlCommand cmd = new SqlCommand("update  Work set NoWorkingDays=@NoWorkingDays,WorkingHours=@WorkingHours,Mon=@Mon,Tue=@Tue,Wen=@Wen,Thu=@Thu,Fri=@Fri,Sat=@Sat,Sun=@Sun where Id =@Id ", con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@NoWorkingDays", MWmaterialComboBox10.Text.ToString());
            cmd.Parameters.AddWithValue("@WorkingHours", materialComboBox8.Text.ToString());
            cmd.Parameters.AddWithValue("@Mon", Mon);
            cmd.Parameters.AddWithValue("@Tue", Tue);
            cmd.Parameters.AddWithValue("@Wen", Wen);
            cmd.Parameters.AddWithValue("@Thu", Thu);
            cmd.Parameters.AddWithValue("@Fri", Fri);
            cmd.Parameters.AddWithValue("@Sat", Sat);
            cmd.Parameters.AddWithValue("@Sun", Sun);
            cmd.Parameters.AddWithValue("@Id", WorkDayID);
            con.Open();

            cmd.ExecuteNonQuery();
            con.Close();


            MessageBox.Show("Updated");

            getWorkingDays();

        }

        /*=============================================   Delete Working Days    =============================================================================== */


        private void manageworkdeletebut_Click(object sender, EventArgs e)
        {



            SqlCommand cmd = new SqlCommand("delete from Work  where Id=@Id", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Id", WorkDayID);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();


            MessageBox.Show("Deleted ");
            getWorkingDays();


        }


        /*=============================================   Add Locations    =============================================================================== */


        private void AddLocSaveBtn_Click(object sender, EventArgs e)
        {
            String Lab = "";
            String Lec = "";
            String tute = "";
            if (materialRadioButton1.Checked == true)
            {
                Lab = "Lecturehall ";


            }
            if (materialRadioButton2.Checked == true)
            {
                Lab = "Laboratory";
            }
            else
            {
                tute = "";
            }
            con.Close();

            SqlCommand cmd = new SqlCommand("INSERT INTO Location(BuildingName,RoomName,Capacity,RoomType) values(@BuildingName,@RoomName,@Capacity,@RoomType)", con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@BuildingName", materialTextBox14.Text);
            cmd.Parameters.AddWithValue("@RoomName", materialTextBox1.Text);
            cmd.Parameters.AddWithValue("@Capacity", materialTextBox6.Text);

            cmd.Parameters.AddWithValue("@RoomType", Lab);



            con.Open();

            cmd.ExecuteNonQuery();
            con.Close();

            getLocationdata();


        }

        private void getLocationdata()
        {
            SqlCommand cmd = new SqlCommand("select * from Location", con);
            DataTable dt = new DataTable();

            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();


            ManagedataGridView.DataSource = dt;
        }



        //ClearLocation


        private void AddLocClearBtn_Click(object sender, EventArgs e)
        {
            materialTextBox14.Text = "";
            materialTextBox1.Text = "";
            materialTextBox1.Text = "";
            materialRadioButton1.Checked = false;
            materialRadioButton2.Checked = false;
        }

        //select Location view

        private void ManagedataGridView_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            LocationID = Convert.ToInt32(ManagedataGridView.SelectedRows[0].Cells[0].Value);
            materialTextBox16.Text = ManagedataGridView.SelectedRows[0].Cells[1].Value.ToString();
            materialTextBox17.Text = ManagedataGridView.SelectedRows[0].Cells[2].Value.ToString();
            materialTextBox15.Text = ManagedataGridView.SelectedRows[0].Cells[3].Value.ToString();
            materialComboBox4.Text = ManagedataGridView.SelectedRows[0].Cells[4].Value.ToString();

        }

        /*=============================================   Update Location     =============================================================================== */



        private void DisplayLocUpdateBtn_Click(object sender, EventArgs e)
        {

            SqlCommand cmd = new SqlCommand("update  Location set  BuildingName=@BuildingName,RoomName=@RoomName,Capacity=@Capacity,RoomType=@RoomType where Id=@Id", con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@BuildingName", materialTextBox16.Text);
            cmd.Parameters.AddWithValue("@RoomName", materialTextBox17.Text);
            cmd.Parameters.AddWithValue("@Capacity", materialTextBox15.Text);

            cmd.Parameters.AddWithValue("@RoomType", materialComboBox4.Text);
            cmd.Parameters.AddWithValue("@Id", LocationID);


            con.Open();

            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("added");
            getLocationdata();

        }



        /*=============================================   Delete Location   =============================================================================== */



        private void DisplayLocDeleteBtn_Click(object sender, EventArgs e)
        {


            SqlCommand cmd = new SqlCommand("delete from Location  where Id=@Id", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Id", SubjectID);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();


            MessageBox.Show("deleted");
            getLocationdata();


        }



        //clear Location
        private void DisplayLocClearBtn_Click(object sender, EventArgs e)
        {

            materialTextBox16.Text = "";
            materialTextBox17.Text = "";
            materialTextBox15.Text = "";
            materialComboBox4.Text = "";


        }


        //----------- Section 2 add sessions -------------------------------------------------------------------------------------------------


        void SelectedLectrues()
        {


            SqlCommand cmd = new SqlCommand("select * from  Lecture", con);
            DataTable dt = new DataTable();

            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                materialComboBox28.Items.Add(sdr.GetValue(1).ToString());

                materialComboBox28.Text = "";
                materialTextBox21.Text = materialComboBox28.Text;
            }


            con.Close();






        }

        void selectedtags()
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("select * from  Tags", con);
            DataTable dt = new DataTable();

            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                materialComboBox31.Items.Add(sdr.GetValue(1).ToString());
            }


            con.Close();

        }


        void selectSubject()
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("select * from  Subject", con);
            DataTable dt = new DataTable();

            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                materialComboBox29.Items.Add(sdr.GetValue(5).ToString());
            }


            con.Close();

        }

        //add section2 ------------------------------------------------------------------

        private void AddSessionSubmit_Click(object sender, EventArgs e)
        {


            con.Close();
            SqlCommand cmd = new SqlCommand("INSERT INTO session(Lecture,Tags,SubjectCode,Groups,Subject,Duration,NumofStudents) values(@Lecture,@Tags,@SubjectCode,@Groups,@Subject,@Duration,@NumofStudents)", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Lecture", materialComboBox28.Text);
            cmd.Parameters.AddWithValue("@Tags", materialComboBox31.Text);
            cmd.Parameters.AddWithValue("@SubjectCode", materialTextBox10.Text);
            cmd.Parameters.AddWithValue("@Groups", materialComboBox30.Text);
            cmd.Parameters.AddWithValue("@Subject", materialComboBox29.Text);
            cmd.Parameters.AddWithValue("@Duration", materialTextBox19.Text);
            cmd.Parameters.AddWithValue("@NumofStudents", materialTextBox20.Text);



            con.Open();

            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Added ");
            getLecData();
            SelectedLectrues();
            sessiontable();
            getConsectivedata();
            GetParrallSessionData();
            GetnonoverlappingSession();
            loadComboSessionID();
        }

        void sessiontable()
        {

            con.Close();
            SqlCommand cmd = new SqlCommand("select * from session", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();

            dt.Load(sdr);
            con.Close();

            dataGridView9.DataSource = dt;


        }

        private void dataGridView9_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            SessionID = Convert.ToInt32(dataGridView9.SelectedRows[0].Cells[0].Value);
            materialTextBox18.Text = dataGridView9.SelectedRows[0].Cells[1].Value.ToString();
            materialTextBox22.Text = dataGridView9.SelectedRows[0].Cells[2].Value.ToString();
            materialTextBox23.Text = dataGridView9.SelectedRows[0].Cells[3].Value.ToString();
            materialTextBox25.Text = dataGridView9.SelectedRows[0].Cells[4].Value.ToString();
            materialTextBox26.Text = dataGridView9.SelectedRows[0].Cells[5].Value.ToString();
            materialTextBox28.Text = dataGridView9.SelectedRows[0].Cells[6].Value.ToString();
            materialTextBox29.Text = dataGridView9.SelectedRows[0].Cells[7].Value.ToString();

        }


        private void ManageSessionUpdate_Click(object sender, EventArgs e)
        {


            con.Close();
            SqlCommand cmd = new SqlCommand("update session set Lecture=@Lecture,Tags=@Tags,SubjectCode=@SubjectCode,Groups=@Groups,Subject=@Subject,Duration=@Duration,NumofStudents=@NumofStudents where Id =@Id", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Lecture", materialTextBox18.Text);
            cmd.Parameters.AddWithValue("@Tags", materialTextBox22.Text);
            cmd.Parameters.AddWithValue("@SubjectCode", materialTextBox23.Text);
            cmd.Parameters.AddWithValue("@Groups", materialTextBox25.Text);
            cmd.Parameters.AddWithValue("@Subject", materialTextBox26.Text);
            cmd.Parameters.AddWithValue("@Duration", materialTextBox28.Text);
            cmd.Parameters.AddWithValue("@NumofStudents", materialTextBox29.Text);
            cmd.Parameters.AddWithValue("@Id", SessionID);



            con.Open();

            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("updated ");
            sessiontable();
        }


        private void materialButtonDeleteSession_Click(object sender, EventArgs e)
        {

            SqlCommand cmd = new SqlCommand("delete from session  where Id=@Id", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Id", SessionID);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();


            MessageBox.Show("deleted");
            sessiontable();

        }


        private void materialButtonRefresh_Click(object sender, EventArgs e)
        {
            materialTextBox18.Text = "";
            materialTextBox22.Text = "";
            materialTextBox23.Text = "";
            materialTextBox25.Text = "";
            materialTextBox26.Text = "";
            materialTextBox28.Text = "";
            materialTextBox29.Text = "";



        }

        //------------------------------------


        private void materialTextBox24_TextChanged(object sender, EventArgs e)
        {
            if (materialComboBox27.Text == "Lecture")
            {
                SqlDataAdapter sda = new SqlDataAdapter("select * from session where Lecture like '" + materialTextBox24.Text + "%'", con);
                DataTable dt = new DataTable();


                sda.Fill(dt);
                dataGridView9.DataSource = dt;



            }
        }



//----------------Location ADD Sprint2-------------------------------------------------------------
        private void RefreshRoomBtn2_Click(object sender, EventArgs e)
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("select * from session", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();

            dt.Load(sdr);
            con.Close();

            SessionridView1.DataSource = dt;
        }


        public void LectuesRLocation()
        {

            foreach (DataGridViewRow dr in SessionridView1.Rows)
            {


                bool checkboxselected = Convert.ToBoolean(dr.Cells["Locationcheckbox"].Value);
                if (checkboxselected)
                {

                    con.Close();
                    SqlCommand cmd = new SqlCommand("INSERT INTO SessionLocation(Lecture,Tags,SubjectCode,Groups,Subject,Duration,NumofStudents) values(@Lecture,@Tags,@SubjectCode,@Groups,@Subject,@Duration,@NumofStudents)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Lecture", dr.Cells[2].Value);
                    cmd.Parameters.AddWithValue("@Tags", dr.Cells[3].Value);
                    cmd.Parameters.AddWithValue("@SubjectCode", dr.Cells[4].Value);
                    cmd.Parameters.AddWithValue("@Groups", dr.Cells[5].Value);
                    cmd.Parameters.AddWithValue("@Subject", dr.Cells[6].Value);
                    cmd.Parameters.AddWithValue("@Duration", dr.Cells[7].Value);
                    cmd.Parameters.AddWithValue("@NumofStudents", dr.Cells[8].Value);
                    



                    con.Open();

                    cmd.ExecuteNonQuery();
                    con.Close();
                    selectsession();

                    MessageBox.Show("sucss");
                }

            }
        }

        public void ConsectiveLectuesRLocation()
        {

            foreach (DataGridViewRow dr in conataGridView1.Rows)
            {


                bool checkboxselected = Convert.ToBoolean(dr.Cells["Locationcheckbox1"].Value);
                if (checkboxselected)
                {

                    con.Close();
                    SqlCommand cmd = new SqlCommand("INSERT INTO consectvelocation(Lecture,Tags,SubjectCode,Groups,Subject,Duration,NumofStudents) values(@Lecture,@Tags,@SubjectCode,@Groups,@Subject,@Duration,@NumofStudents)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Lecture", dr.Cells[2].Value);
                    cmd.Parameters.AddWithValue("@Tags", dr.Cells[3].Value);
                    cmd.Parameters.AddWithValue("@SubjectCode", dr.Cells[4].Value);
                    cmd.Parameters.AddWithValue("@Groups", dr.Cells[5].Value);
                    cmd.Parameters.AddWithValue("@Subject", dr.Cells[6].Value);
                    cmd.Parameters.AddWithValue("@Duration", dr.Cells[7].Value);
                    cmd.Parameters.AddWithValue("@NumofStudents", dr.Cells[8].Value);




                    con.Open();

                    cmd.ExecuteNonQuery();
                    con.Close();
                   

                    MessageBox.Show("sucss");
                }

            }
        }

        private void consective_Click(object sender, EventArgs e)
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("select * from consectvelocation", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();

            dt.Load(sdr);
            con.Close();

            SessionGridView1.DataSource = dt;

            
        }

        private void selectsession()
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("select * from SessionLocation", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();

            dt.Load(sdr);
            con.Close();

            SessionGridView1.DataSource = dt;
        }

        //session
        private void materialButton6_Click(object sender, EventArgs e)
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("update SessionLocation set room = @room  where Id =@Id", con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@room", materialTextBox32.Text);
            cmd.Parameters.AddWithValue("@Id", this.SessionID);
            con.Open();

            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("updated ");
        }

        //consective
        private void materialButton10_Click(object sender, EventArgs e)
        {

            con.Close();
            SqlCommand cmd = new SqlCommand("update consectvelocation set room = @room  where Id =@Id", con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@room", materialTextBox32.Text);
            cmd.Parameters.AddWithValue("@Id", this.SessionID);
            con.Open();

            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("added ");

        }
        private void SessionGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SessionID = Convert.ToInt32(SessionGridView1.SelectedRows[0].Cells[0].Value);
            materialTextBox31.Text = SessionGridView1.SelectedRows[0].Cells[1].Value.ToString();
            materialTextBox32.Text = SessionGridView1.SelectedRows[0].Cells[2].Value.ToString();

        }

        private void materialButton7_Click_1(object sender, EventArgs e)
        {
            selectsession();
            
        }


        private void AddRoomBtn2_Click(object sender, EventArgs e)
        {
            LectuesRLocation();
        }


        private void materialButton8_Click(object sender, EventArgs e)
        {


            con.Close();
            SqlCommand cmd = new SqlCommand("select * from consective", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();

            dt.Load(sdr);
            con.Close();

            conataGridView1.DataSource = dt;
        }

        private void materialButton9_Click(object sender, EventArgs e)
        {
            ConsectiveLectuesRLocation();
        }
        // ------------------------------------------------------------------------------------------------------
        private void materialTextBox4_TextChanged(object sender, EventArgs e)
        {

            if (materialComboBox3.Text == "ID")
            {
                SqlDataAdapter sda = new SqlDataAdapter("select * from session where Id like '" + materialTextBox4.Text + "%'", con);
                DataTable dt = new DataTable();


                sda.Fill(dt);
                datagridviewcoloum.DataSource = dt;



            }
            else if (materialComboBox3.Text == "Subject name")
            {

                SqlDataAdapter sda = new SqlDataAdapter("select * from session where Subject like '" + materialTextBox4.Text + "%'", con);
                DataTable dt = new DataTable();

                sda.Fill(dt);

                datagridviewcoloum.DataSource = dt;
            }
        }



        private void materialButton23_Click(object sender, EventArgs e)
        {
            loadParallelROw();
            Palpanel2.Show();
            Palpanel2.BringToFront();

        }
        private void materialButton22_Click(object sender, EventArgs e)
        {
            consectivepanel.Show();
            consectivepanel.BringToFront();
        }




        //-----------panels and buttons navigations-------------------------------------------------------------------------------------------------



        //hide Student and Tag panel Mthods
        private void ChangeSizeTagPanel()
        {

            panel1.Location = new Point(
     this.ClientSize.Width / 2 - panel1.Size.Width / 2, this.ClientSize.Height / 2 - panel1.Size.Height / 2);
            this.panel1.Anchor = AnchorStyles.None;

            tagpanel2.Location = new Point(
        this.ClientSize.Width / 2 - tagpanel2.Size.Width / 2, this.ClientSize.Height / 2 - tagpanel2.Size.Height / 2);
            this.tagpanel2.Anchor = AnchorStyles.None;
        }


        //Hide  other All panels
        private void ManagePanelSize()
        {

            Updatepanel2.Location = new Point(
              this.ClientSize.Width / 2 - Updatepanel2.Size.Width / 2, this.ClientSize.Height / 2 - Updatepanel2.Size.Height / 2);
            this.Updatepanel2.Anchor = AnchorStyles.None;

            Tag1panel.Location = new Point(
            this.ClientSize.Width / 2 - Tag1panel.Size.Width / 2, this.ClientSize.Height / 2 - Tag1panel.Size.Height / 2);
            this.Tag1panel.Anchor = AnchorStyles.None;


            paneManageLec.Location = new Point(
          this.ClientSize.Width / 2 - paneManageLec.Size.Width / 2, this.ClientSize.Height / 2 - panel1.Size.Height / 2);
            this.paneManageLec.Anchor = AnchorStyles.None;

            SubjectPanalAddSubject.Location = new Point(
          this.ClientSize.Width / 2 - panel1.Size.Width / 2, this.ClientSize.Height / 2 - panel1.Size.Height / 2);
            this.SubjectPanalAddSubject.Anchor = AnchorStyles.None;

            SubjectManageSubpanal.Location = new Point(
          this.ClientSize.Width / 2 - panel1.Size.Width / 2, this.ClientSize.Height / 2 - panel1.Size.Height / 2);
            this.SubjectManageSubpanal.Anchor = AnchorStyles.None;

            manageworkpanel.Location = new Point(
          this.ClientSize.Width / 2 - panel1.Size.Width / 2, this.ClientSize.Height / 2 - panel1.Size.Height / 2);
            this.manageworkpanel.Anchor = AnchorStyles.None;

            addworkpanel.Location = new Point(
          this.ClientSize.Width / 2 - panel1.Size.Width / 2, this.ClientSize.Height / 2 - panel1.Size.Height / 2);
            this.addworkpanel.Anchor = AnchorStyles.None;

            SessionRoomPanel3.Location = new Point(
          this.ClientSize.Width / 2 - panel1.Size.Width / 2, this.ClientSize.Height / 2 - panel1.Size.Height / 2);
            this.SessionRoomPanel3.Anchor = AnchorStyles.None;

            panelManageSessionsRooms.Location = new Point(
          this.ClientSize.Width / 2 - panel1.Size.Width / 2, this.ClientSize.Height / 2 - panel1.Size.Height / 2);
            this.panelManageSessionsRooms.Anchor = AnchorStyles.None;

            AddLocationPanel1.Location = new Point(
          this.ClientSize.Width / 2 - panel1.Size.Width / 2, this.ClientSize.Height / 2 - panel1.Size.Height / 2);
            this.AddLocationPanel1.Anchor = AnchorStyles.None;

            DisplayLocationPanel.Location = new Point(
          this.ClientSize.Width / 2 - panel1.Size.Width / 2, this.ClientSize.Height / 2 - panel1.Size.Height / 2);
            this.DisplayLocationPanel.Anchor = AnchorStyles.None;

            addSssionpanel.Location = new Point(
    this.ClientSize.Width / 2 - panel1.Size.Width / 2, this.ClientSize.Height / 2 - panel1.Size.Height / 2);
            this.addSssionpanel.Anchor = AnchorStyles.None;

            panelManageSession.Location = new Point(
   this.ClientSize.Width / 2 - panel1.Size.Width / 2, this.ClientSize.Height / 2 - panel1.Size.Height / 2);
            this.panelManageSession.Anchor = AnchorStyles.None;

            Lecpanel2.Location = new Point(
 this.ClientSize.Width / 2 - panel1.Size.Width / 2, this.ClientSize.Height / 2 - panel1.Size.Height / 2);
            this.Lecpanel2.Anchor = AnchorStyles.None;

        }

        private void fillChart()
        {
            chart1.Series["StudentsGroups"].Points.AddXY("Lecture", "500");
            chart1.Series["StudentsGroups"].Points.AddXY("StudentsGroups", "2000");
            chart1.Series["StudentsGroups"].Points.AddXY("Tags", "800");
            //chart1.Titles.Add("StudentsGroups");
        }


        //Refresh Genarate ID Data
        private void RefreshGenarateID()
        {
            GropIDdataGridView1.Columns.Clear();
            SubdataGridView1.Columns.Clear();
            GenarateIDGroup.Clear();
            SubGroupmaterial.Clear();



        }

        private void ClearSubGroupID()
        {
            SubGroupmaterial.Clear();
        }

        private void ClearGroupID()
        {
            GenarateIDGroup.Clear();

        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            //AddStudent std = new AddStudent();
            //std.Show();
            //this.Visible = false;

            panel1.Show();
            panel1.BringToFront();







        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            tagpanel2.Show();
            tagpanel2.BringToFront();
        }









        private void materialButton2_Click(object sender, EventArgs e)
        {
            //updateStudents update = new updateStudents();
            //update.Show();
            //this.Visible = false;

            Updatepanel2.Show();
            Updatepanel2.BringToFront();
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
           
          


        }



        private void materialButton4_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox10_Click_1(object sender, EventArgs e)
        {
            panel1.Hide();


        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Updatepanel2.Hide();
        }


        private void materialButton4_Click_1(object sender, EventArgs e)
        {
            Tag1panel.Show();
            Tag1panel.BringToFront();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Tag1panel.Hide();
        }

        private void pictureBox11_Click_1(object sender, EventArgs e)
        {
            tagpanel2.Hide();
        }






        private void pictureBox5_Click_1(object sender, EventArgs e)
        {
            Updatepanel2.Hide();
        }

        private void pictureBox10_Click_2(object sender, EventArgs e)
        {
            panel1.Hide();
            //refresh Data Student Groups and Genarate ID 
            Reset();
            RefreshGenarateID();

        }


        private void pictureBox9_Click_1(object sender, EventArgs e)
        {
            Tag1panel.Hide();
            ClearAddedTags();
        }

        private void pictureBox11_Click_2(object sender, EventArgs e)
        {
            tagpanel2.Hide();
            ClearTags();


        }

        //Clear Tags
        private void ClearTagButton_Click(object sender, EventArgs e)
        {
            ClearTags();
        }



       
        private void ManageLec_Click(object sender, EventArgs e)
        {
            paneManageLec.Show();
        }

        private void AddLecs_Click(object sender, EventArgs e)
        {
            Lecpanel2.Show();
        }

        private void LecpictureBox_Click(object sender, EventArgs e)
        {
            Lecpanel2.Hide();
        }

       
        private void managelecpic_Click(object sender, EventArgs e)
        {
            paneManageLec.Hide();
        }

        private void AddSubbtn_Click(object sender, EventArgs e)
        {
            SubjectPanalAddSubject.Show();
        }

        private void Managesubbtn_Click(object sender, EventArgs e)
        {

            SubjectManageSubpanal.Show();
        }

        private void pictureBoxManagesub_Click(object sender, EventArgs e)
        {
            SubjectManageSubpanal.Hide();
        }

        private void pictureBoxSujectadd_Click(object sender, EventArgs e)
        {
            SubjectPanalAddSubject.Hide();
        }



        private void homeaddworkbut_Click(object sender, EventArgs e)
        {
            addworkpanel.Show();
        }

        private void manageaddworkbut_Click_1(object sender, EventArgs e)
        {
            manageworkpanel.Show();
        }

        private void manageworkpicBox_Click(object sender, EventArgs e)
        {
            manageworkpanel.Hide();

        }

        private void addworkpicBox_Click(object sender, EventArgs e)
        {
            addworkpanel.Hide();
        }

        private void addroombtn_Click(object sender, EventArgs e)
        {
            SessionRoomPanel3.Show();
        }



        private void manageSessionBtn_Click(object sender, EventArgs e)
        {
            panelManageSessionsRooms.Show();
        }

        private void pictureBoxRooms_Click(object sender, EventArgs e)
        {
            SessionRoomPanel3.Hide();

        }

        private void pictureBoxMSRooms_Click(object sender, EventArgs e)
        {
            panelManageSessionsRooms.Hide();
        }

        private void pictureBoxSujectadd_Click_1(object sender, EventArgs e)
        {
            SubjectPanalAddSubject.Hide();
        }





        private void pictureBoxDisplayLoc_Click_1(object sender, EventArgs e)
        {
            DisplayLocationPanel.Hide();
        }

        private void LocationIcon2Btn_Click(object sender, EventArgs e)
        {
            DisplayLocationPanel.Show();
        }

        private void LocationIcon1Btn_Click(object sender, EventArgs e)
        {
            AddLocationPanel1.Show();
        }

        private void pictureBoxAddLoc_Click(object sender, EventArgs e)
        {
            AddLocationPanel1.Hide();
        }


        private void AddSessionbtn_Click(object sender, EventArgs e)
        {
            addSssionpanel.Show();
        }

        private void managesebtn_Click(object sender, EventArgs e)
        {
            panelManageSession.Show();
        }

        private void pictureBoxAddSession_Click(object sender, EventArgs e)
        {
            addSssionpanel.Hide();
        }

        private void pictureBoxManageSession_Click(object sender, EventArgs e)
        {
            panelManageSession.Hide();
        }


        private void materialButton27_Click(object sender, EventArgs e)
        {
            GenarateIDPanel.Show();
            GenarateIDPanel.BringToFront();
        }

        private void pictureBox31_Click(object sender, EventArgs e)
        {
            GenarateIDPanel.Hide();


        }

        private void pictureBox32_Click(object sender, EventArgs e)
        {
            GenarateIDPanel.Hide();
        }

        //hide Panel method
        private void HidePanel()
        {
            panel1.Hide();
            Updatepanel2.Hide();
            Tag1panel.Hide();
            tagpanel2.Hide();
            Lecpanel2.Hide();
            paneManageLec.Hide();
            SubjectPanalAddSubject.Hide();
            SubjectManageSubpanal.Hide();
            manageworkpanel.Hide();
            addworkpanel.Hide();
            SessionRoomPanel3.Hide();
            panelManageSessionsRooms.Hide();
            AddLocationPanel1.Hide();
            DisplayLocationPanel.Hide();
            addSssionpanel.Hide();
            panelManageSession.Hide();
            GenarateIDPanel.Hide();
            consectivepanel.Hide();
            Palpanel2.Hide();
            Nonpanel3.Hide();
            updatenotsessionpanel.Hide();
        }

        private void materialTextBox21_TextChanged(object sender, EventArgs e)
        {

            materialComboBox28.Text = "";
            materialTextBox21.Text = materialComboBox28.Text;
        }

        private void ConpictureBox14_Click(object sender, EventArgs e)
        {
            consectivepanel.Hide();
        }

        private void PalpictureBox14_Click(object sender, EventArgs e)
        {
            Palpanel2.Hide();

        }

        private void NonpictureBox14_Click(object sender, EventArgs e)
        {
            Nonpanel3.Hide();
        }

        private void materialButton25_Click(object sender, EventArgs e)
        {
            Nonpanel3.Show();
            Nonpanel3.BringToFront();
        }

        private void materialButton24_Click(object sender, EventArgs e)
        {
            insertParrellSession();
        }

        private void materialButton26_Click(object sender, EventArgs e)
        {
            insertNonoverlappingSession();
        }

        private void ANSviewbut_Click(object sender, EventArgs e)
        {
            updatenotsessionpanel.Show();
            updatenotsessionpanel.BringToFront();
        }

        private void updatenotsessionpicBox_Click(object sender, EventArgs e)
        {
            updatenotsessionpanel.Hide();
        }

        //non overlap


        private void loadComboLectureNames()
        {
            try
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT Lecname FROM Lecture", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                //Console.WriteLine(dt.Rows.Count);
                ANSmaterialComboBox10.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    //Console.WriteLine(row[0].ToString());
                    ANSmaterialComboBox10.Items.Add(row[0].ToString());
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void loadComboGroups()
        {
            try
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT GroupNumber,SubGroup FROM Students", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                //Console.WriteLine(dt.Rows.Count);
                ANSmaterialComboBox11.Items.Clear();
                ANSmaterialComboBox12.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    //Console.WriteLine(row[0].ToString());
                    ANSmaterialComboBox11.Items.Add(row[0].ToString());
                    ANSmaterialComboBox12.Items.Add(row[0].ToString());

                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void loadComboSessionID()
        {
            try
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT Id FROM session", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                //Console.WriteLine(dt.Rows.Count);
                ANSmaterialComboBox13.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    //Console.WriteLine(row[0].ToString());
                    ANSmaterialComboBox13.Items.Add(row[0].ToString());

                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ANSsubmitbut_Click(object sender, EventArgs e)
		{
            con.Close();
            SqlCommand cmd = new SqlCommand("INSERT INTO SessionNotAvailable VALUES(@Lecturer, @Groups, @SubGroup, @SessionID, @Day, @Time)", con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@Lecturer", ANSmaterialComboBox10.Text.ToString());
            cmd.Parameters.AddWithValue("@Groups", ANSmaterialComboBox11.Text.ToString());
            cmd.Parameters.AddWithValue("@SubGroup", ANSmaterialComboBox12.Text.ToString());
            cmd.Parameters.AddWithValue("@SessionID", ANSmaterialComboBox13.Text.ToString());
            cmd.Parameters.AddWithValue("@Day", ANSmaterialComboBox14.Text.ToString());
            cmd.Parameters.AddWithValue("@Time", ANSmaterialComboBox26.Text.ToString());
            con.Open();

            cmd.ExecuteNonQuery();
            con.Close();


            MessageBox.Show("added");
            Getnootavalibletime();

        }

		private void Getnootavalibletime()
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("select * from SessionNotAvailable", con);
            DataTable dt = new DataTable();

            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();


            UNSdataGridView9.DataSource = dt;
        }

		private void ANSclearbut_Click(object sender, EventArgs e)
		{
            ANSmaterialComboBox10.SelectedIndex = -1;
            ANSmaterialComboBox11.SelectedIndex = -1;
            ANSmaterialComboBox12.SelectedIndex = -1;
            ANSmaterialComboBox13.SelectedIndex = -1;
            ANSmaterialComboBox14.SelectedIndex = -1;
            ANSmaterialComboBox26.SelectedIndex = -1;
        }

		private void UNSupdatebut_Click(object sender, EventArgs e)
		{
            SqlCommand cmd = new SqlCommand("update   SessionNotAvailable set Lecturer=@Lecturer,Groups=@Groups,SubGroup=@SubGroup,SessionID=@SessionID,Day=@Day,Time=@Time where Id =@Id ", con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@Lecturer", UNSmaterialTextBox33.Text.ToString());
            cmd.Parameters.AddWithValue("@Groups", UNSmaterialTextBox36.Text.ToString());
            cmd.Parameters.AddWithValue("@SubGroup", UNSmaterialTextBox35.Text.ToString());
            cmd.Parameters.AddWithValue("@SessionID", UNSmaterialTextBox34.Text.ToString());
            cmd.Parameters.AddWithValue("@Day", UNSmaterialTextBox37.Text.ToString());
            cmd.Parameters.AddWithValue("@Time", UNSmaterialTextBox38.Text.ToString());
            cmd.Parameters.AddWithValue("@Id", this.SessionID);
            con.Open();

            cmd.ExecuteNonQuery();
            con.Close();


            MessageBox.Show("Update Successfully");

            Getnootavalibletime();
        }

		private void UNSdataGridView9_CellClick(object sender, DataGridViewCellEventArgs e)
		{

            SessionID = Convert.ToInt32(UNSdataGridView9.SelectedRows[0].Cells[0].Value);
            UNSmaterialTextBox33.Text = UNSdataGridView9.SelectedRows[0].Cells[1].Value.ToString();
            UNSmaterialTextBox36.Text = UNSdataGridView9.SelectedRows[0].Cells[2].Value.ToString();
            UNSmaterialTextBox35.Text = UNSdataGridView9.SelectedRows[0].Cells[3].Value.ToString();
            UNSmaterialTextBox34.Text = UNSdataGridView9.SelectedRows[0].Cells[4].Value.ToString();
            UNSmaterialTextBox37.Text = UNSdataGridView9.SelectedRows[0].Cells[5].Value.ToString();
            UNSmaterialTextBox38.Text = UNSdataGridView9.SelectedRows[0].Cells[6].Value.ToString();
        }

		private void UNSrefreshbut_Click(object sender, EventArgs e)
		{
            con.Close();
            SqlCommand cmd = new SqlCommand("select * from SessionNotAvailable", con);
            DataTable dt = new DataTable();

            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();


            UNSdataGridView9.DataSource = dt;
        }

		private void UNSdeletebut_Click(object sender, EventArgs e)
		{
            SqlCommand cmd = new SqlCommand("delete from SessionNotAvailable  where Id=@Id", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Id", SessionID);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();


            MessageBox.Show("deleted");
        }
	}
}
