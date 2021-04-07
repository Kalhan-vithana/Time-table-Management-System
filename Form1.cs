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







        }

        private void EnableGropIDs()
        {
            GroupID1.Enabled = false;
            SubGroupsID.Enabled = false;
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

        private void AddStudents_Click(object sender, EventArgs e)
        {


            con.Close();
           SqlCommand cmd = new SqlCommand("INSERT INTO Students(AcdemicYear,Programne,GroupNumber,SubGroup,GroupID,subGroupID) values (@AcdemicYear,@Programne,@GroupNumber,@SubGroup,@GroupID,@subGroupID)", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@AcdemicYear", AdecmicYear_ComboBox.Text.ToString());
            cmd.Parameters.AddWithValue("@Programne", Programme_combo.Text.ToString());
            cmd.Parameters.AddWithValue("@GroupNumber", numericUpDown1.Value);
            cmd.Parameters.AddWithValue("@SubGroup", numericUpDown2.Value);
            cmd.Parameters.AddWithValue("@GroupID", GroupID1.Text );
            cmd.Parameters.AddWithValue("@subGroupID", SubGroupsID.Text);
          

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
          
            MessageBox.Show("added", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            getStudents();
            //Reset();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            Reset();
        }


        /*============================================= Reset method  Students Group =============================================================================== */

        private void Reset()
        {
            studentID = 0;
            AdecmicYear_ComboBox.SelectedIndex =-1;
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
            SqlCommand cmd = new SqlCommand("select Id, CONCAT(AcdemicYear,'.',Programne,'.',GroupNumber) AS YEAR from Students ORDER BY Id DESC; ", con);
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
            if (studentID > 0)
            {

                SqlCommand cmd = new SqlCommand("update Students set GroupID = @GroupID  where Id =@Id", con);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@GroupID", GenarateIDGroup.Text);
                cmd.Parameters.AddWithValue("@Id", this.studentID);

                con.Open();

                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("added", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                getStudents();





            }
            else
            {

                MessageBox.Show("notupdated", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        /*=============================================  Add  Students Sub Groups =============================================================================== */
        private void SubGroupIDmaterialButton8_Click(object sender, EventArgs e)
        {

            if (studentID > 0)
            {

                SqlCommand cmd = new SqlCommand("update Students set subGroupID = @subGroupID  where Id=@Id", con);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@subGroupID", SubGroupmaterial.Text);
                cmd.Parameters.AddWithValue("@Id", this.studentID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("added", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                getStudents();


            }
            else
            {

                MessageBox.Show("notupdated", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            UpdateAcdemictxt.Text= UpdateStudentdataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            UpdateProgrammeTxt.Text = UpdateStudentdataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            UpdateStudentsnumericUpDown4.Text = UpdateStudentdataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            UpdateStudentGroupnumericUpDown3.Text = UpdateStudentdataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            UpdateSubGroup.Text = UpdateStudentdataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            UpdateGroupID.Text = UpdateStudentdataGridView1.SelectedRows[0].Cells[6].Value.ToString();
          

        }


        /*============================================= Update Students Groups  =============================================================================== */
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
                MessageBox.Show("update", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);

                getStudents();
                Reset();
            }
            else
            {

                MessageBox.Show("notupdated", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        /*=============================================    Delete Students Groups   =============================================================================== */
        private void DeleteStudentsmaterialButton10_Click(object sender, EventArgs e)
        {
            if (studentID > 0)
            {

                SqlCommand cmd = new SqlCommand("delete from Students  where Id=@Id", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", this.studentID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                getStudents();
                clearStudents();
                MessageBox.Show("deleted", "sucessfulluly", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("errpr", "sucessfulluly", MessageBoxButtons.OK, MessageBoxIcon.Error);

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


        /*=============================================   Add Tags     =============================================================================== */
        private void SaveTag_Click(object sender, EventArgs e)
        {
            con.Close();
            SqlCommand cmd = new SqlCommand("INSERT INTO Tags(SubjectName,SubjectCode,RelatedTags) values(@SubjectName,@SubjectCode,@RelatedTags)", con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@SubjectName", AddTagCombo1.Text.ToString());
            cmd.Parameters.AddWithValue("@SubjectCode", ADDTagCodeComboBox1.Text.ToString());
            cmd.Parameters.AddWithValue("@RelatedTags", VeiwRelateTagComboBox3.Text.ToString());

            con.Open();

            cmd.ExecuteNonQuery();
            con.Close();
          
            MessageBox.Show("added", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);

            GetTags(); //get tags details
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

            if(SearchTagsCombo1.Text == "ID")
            {
                SqlDataAdapter sda = new SqlDataAdapter("select * from Tags where Id like '"+SearchTags.Text +"%'", con);
                DataTable dt = new DataTable();

  
                sda.Fill(dt);
                TagdataGridView.DataSource = dt;



            }
            else if(SearchTagsCombo1.Text == "Subject name")
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
            UpdateTagTextbox.Text = TagdataGridView.SelectedRows[0].Cells[1].Value.ToString();
            UpdateCodeTxtBox.Text = TagdataGridView.SelectedRows[0].Cells[2].Value.ToString();
           // UpdateRelatedTags.Text = TagdataGridView.SelectedRows[0].Cells[3].Value.ToString();
            

            var val = TagdataGridView.SelectedRows[0].Cells[3].Value.ToString();
           
            UpdateRelatedTags.Items.Add(val);
            UpdateRelatedTags.SelectedIndex = -1;

        }



        /*=============================================   Update Students Tags   =============================================================================== */
        private void UpdateTagButton_Click(object sender, EventArgs e)
        {
            if(TagID > 0)
            {

                SqlCommand cmd = new SqlCommand("update Tags set SubjectName=@SubjectName,SubjectCode=@SubjectCode,RelatedTags=@RelatedTags where Id=@Id ", con);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@SubjectName", UpdateTagTextbox.Text);
                cmd.Parameters.AddWithValue("@SubjectCode", UpdateCodeTxtBox.Text);
                cmd.Parameters.AddWithValue("@RelatedTags", UpdateRelatedTags.Text);
                cmd.Parameters.AddWithValue("@Id", this.TagID);

                //con.Open();





                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("update", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);


                GetTags();
            }
            else
            {

                MessageBox.Show("error", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

  /*=============================================  Delete Students Tags   =============================================================================== */
        private void DeleteTagButton_Click(object sender, EventArgs e)
        {

            if (TagID > 0)
            {

                SqlCommand cmd = new SqlCommand("delete from Tags  where Id=@Id", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", this.TagID);

                cmd.ExecuteNonQuery();
                con.Close();

                GetTags();
                clearStudents();
                MessageBox.Show("deleted", "sucessfulluly", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("error", "sucessfulluly", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

       
        //Clear Tags  Method

        private void ClearTags()
        {
            TagID = 0;
            UpdateTagTextbox.Clear();
            UpdateCodeTxtBox.Clear();
            UpdateRelatedTags.SelectedIndex = -1;

            UpdateTagTextbox.Focus();
        }

        private void ClearTag_Click(object sender, EventArgs e)
        {
            ClearAddedTags();

        }

        private void ClearAddedTags()
        {
            AddTagCombo1.SelectedIndex = -1;
            ADDTagCodeComboBox1.SelectedIndex = -1;
            VeiwRelateTagComboBox3.SelectedIndex = -1;
        }

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
            UpdateAcdemictxt.Text = SubjectGridView1.SelectedRows[0].Cells[1].Value.ToString();
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


      
            LecName.Text ="";
            idLecs.Text ="";
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
            String Day ="";
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

                Day = "Sunday";
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



        /*=============================================   Add Locations    =============================================================================== */


        private void AddLocSaveBtn_Click(object sender, EventArgs e)
        {
            String Lab = "";
            String Lec = "";
            String tute = ""; 
            if(materialRadioButton1.Checked == true)
            {
                Lab = "Lecturehall ";


            }
            if(materialRadioButton2.Checked == true)
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
            DeleteStudents dlt = new DeleteStudents();
            dlt.Show();
            this.Visible = false;


        }



        private void materialButton4_Click(object sender, EventArgs e)
        {
            Add_Tag tag = new Add_Tag();
            tag.Show();
            this.Visible = false;
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
        }

        private void pictureBox11_Click_2(object sender, EventArgs e)
        {
            tagpanel2.Hide();
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
        }



    }
}
