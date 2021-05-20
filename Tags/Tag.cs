﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//IT19170862 p.v.d.m kalhan
namespace Time_Table_managemnt.Tags
{
   
    class Tag
    {
        public int Id { get; set; }
        public String SubjectName { get; set; }
        public String SubjectCode { get; set; }
        public String RelatedTags { get; set; }

        public int id;

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MYDatabase1.mdf;Integrated Security=True");



        public int addTag(Tag t)
        {
           
            SqlCommand cmd = new SqlCommand("INSERT INTO Tags(SubjectName,SubjectCode,RelatedTags) values(@SubjectName,@SubjectCode,@RelatedTags); SELECT SCOPE_IDENTITY()", con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@SubjectName", t.SubjectName);
            cmd.Parameters.AddWithValue("@SubjectCode", t.SubjectCode);
            cmd.Parameters.AddWithValue("@RelatedTags",t.RelatedTags );

            con.Open();

            id = Convert.ToInt32(cmd.ExecuteScalar());
            Console.WriteLine(id);
            con.Close();



            return id;

        }


        public bool updateTag(Tag t)
        {
            bool isSuccess = false;

            SqlCommand cmd = new SqlCommand("update Tags set SubjectName=@SubjectName,SubjectCode=@SubjectCode,RelatedTags=@RelatedTags where Id=@Id ", con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@SubjectName", t.SubjectName);
            cmd.Parameters.AddWithValue("@SubjectCode", t.SubjectCode);
            cmd.Parameters.AddWithValue("@RelatedTags", t.RelatedTags);
            cmd.Parameters.AddWithValue("@Id", t.Id);

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


        public bool deleteTag(Tag t)
        {

            bool isSuccess = false;

            SqlCommand cmd = new SqlCommand("delete from Tags  where Id=@Id", con);
            cmd.CommandType = CommandType.Text;
           
            cmd.Parameters.AddWithValue("@Id", t.Id);
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
