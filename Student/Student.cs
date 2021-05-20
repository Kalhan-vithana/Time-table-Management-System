using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//IT19170862 p.v.d.m kalhan
namespace Time_Table_managemnt
{
    class Student
    {
        public int Id { get; set; }
        public String AcdemicYear { get; set; }
        public String Programne { get; set; }
        public String GroupNumber { get; set; }
        public String SubGroup { get; set; }
        public String GroupID { get; set; }
        public String subGroupID { get; set; }

        public int id;
        //database connection
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MYDatabase1.mdf;Integrated Security=True");

       

        public int AddStd(Student s)
        {

            SqlCommand cmd = new SqlCommand("INSERT INTO Students(AcdemicYear,Programne,GroupNumber,SubGroup,GroupID,subGroupID) values (@AcdemicYear,@Programne,@GroupNumber,@SubGroup,@GroupID,@subGroupID); SELECT SCOPE_IDENTITY()", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@AcdemicYear", s.AcdemicYear);
            cmd.Parameters.AddWithValue("@Programne",s.Programne);
            cmd.Parameters.AddWithValue("@GroupNumber", s.GroupNumber);
            cmd.Parameters.AddWithValue("@SubGroup", s.SubGroup);
            cmd.Parameters.AddWithValue("@GroupID", s.GroupID);
            cmd.Parameters.AddWithValue("@subGroupID", s.subGroupID);


            con.Open();
            id = Convert.ToInt32(cmd.ExecuteScalar());
            Console.WriteLine(id);
            con.Close();

            return id;
            /*MessageBox.Show("added  sucessfully", "sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            getStudents();*/
            //Reset();


        }


        public bool update(Student s)
        {
            bool isSuccess = false;

            SqlCommand cmd = new SqlCommand("update Students set AcdemicYear=@AcdemicYear,Programne=@Programne,GroupNumber=@GroupNumber,SubGroup=@SubGroup,GroupID=@GroupID,subGroupID=@subGroupID where Id=@Id", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@AcdemicYear", s.AcdemicYear);
            cmd.Parameters.AddWithValue("@Programne", s.Programne);
            cmd.Parameters.AddWithValue("@GroupNumber", s.GroupNumber);
            cmd.Parameters.AddWithValue("@SubGroup", s.SubGroup);
            cmd.Parameters.AddWithValue("@GroupID", s.GroupID);
            cmd.Parameters.AddWithValue("@subGroupID", s.subGroupID);
            cmd.Parameters.AddWithValue("@Id", s.Id);

            con.Open();
            int row = cmd.ExecuteNonQuery();

            if (row > 0)
            {
                isSuccess = true;
            }
            else
            {
                isSuccess = false;
            }
         
            con.Close();
            return isSuccess;
        }


        public bool delete(Student s)
        {

            bool isSuccess = false;

            SqlCommand cmd = new SqlCommand("delete from Students  where Id=@Id", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Id", s.Id);
            con.Open();
            int row = cmd.ExecuteNonQuery();

            if (row > 0)
            {
                isSuccess = true;
            }
            else
            {
                isSuccess = false;
            }

            con.Close();
            return isSuccess;
        }

        public bool ADDGenarateID(Student s)
        {
            bool isSuccess = false;

            SqlCommand cmd = new SqlCommand("update Students set GroupID = @GroupID  where Id =@Id", con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@GroupID",s.GroupID);
            cmd.Parameters.AddWithValue("@Id", s.Id);
            con.Open();
            int row = cmd.ExecuteNonQuery();


            if (row > 0)
            {
                isSuccess = true;
            }
            else
            {
                isSuccess = false;
            }

            con.Close();
            return isSuccess;
        }



        public bool ADDSubGenarateID(Student s)
        {
            bool isSuccess = false;


            SqlCommand cmd = new SqlCommand("update Students set subGroupID = @subGroupID  where Id=@Id", con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@subGroupID", s.subGroupID);
            cmd.Parameters.AddWithValue("@Id", s.Id);
            con.Open();
            int row = cmd.ExecuteNonQuery();


            if (row > 0)
            {
                isSuccess = true;
            }
            else
            {
                isSuccess = false;
            }

            con.Close();
            return isSuccess;
        }
    }
}
