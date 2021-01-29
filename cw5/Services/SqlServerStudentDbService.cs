using cw5.Models;
using cw5.Requests;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cw5.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    {
        public SqlServerStudentDbService()
        {

        }

        public void EnrollStudent(EnrollStudentRequest request)
        {
            var st = new Student();
            st.FirstName = request.FirstName;
            st.LastName = request.LastName;
            st.IndexNumber = request.IndexNumber;
            st.BirthDate = request.BirthDate;
            st.Studies = request.Studies;

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s17060;Integrated Security=True;MultipleActiveResultSets=true"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                var transaction = con.BeginTransaction();
                com.Transaction = transaction;

                try
                {

                    com.CommandText = "SELECT IdStudy FROM Studies WHERE Name=@nameParam";
                    com.Parameters.AddWithValue("nameParam", request.Studies);
                    var dr = com.ExecuteReader();

                    if (!dr.Read())
                    {
                        transaction.Rollback();
                        //return BadRequest("Studies not found");
                    }
                    int IdStudyTemp = (int)dr["IdStudy"];


                    dr.Close();


                    com.CommandText = "SELECT IdEnrollment FROM Enrollment WHERE IdStudy=@idStudyParam AND Semester=1";
                    com.Parameters.AddWithValue("idStudyParam", IdStudyTemp);
                    dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        com.CommandText = "INSERT INTO Enrollment(IdEnrollment, Semester, IdStudy, StartDate) VALUES(@idenrollment, 1, @idstudy, @startdate)";
                        com.Parameters.AddWithValue("idenrollment", 1);
                        com.Parameters.AddWithValue("idstudy", IdStudyTemp);
                        com.Parameters.AddWithValue("startdate", DateTime.Now);
                        com.ExecuteNonQuery();
                    }
                    dr.Close();


                    com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, IdEnrollment, BirthDate) VALUES(@index, @firstname, @lastname, @idenrollment, @birthdate)";
                    com.Parameters.AddWithValue("index", request.IndexNumber);
                    com.Parameters.AddWithValue("firstname", request.FirstName);
                    com.Parameters.AddWithValue("lastname", request.LastName);
                    com.Parameters.AddWithValue("idenrollment", 20);
                    com.Parameters.AddWithValue("birthdate", request.BirthDate);


                    com.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException e)
                {
                    transaction.Rollback();
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public Student GetStudent(string index)
        {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s17060;Integrated Security=True;MultipleActiveResultSets=true"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT * FROM Student WHERE IndexNumber=@index";

                com.Parameters.AddWithValue("index", index);

                con.Open();
                var dr = com.ExecuteReader();
                if (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    return st;
                }
            }

            return null;
        }

        public void PromoteStudent(int semester, string studies)
        {
            throw new NotImplementedException();
        }
    }
}
