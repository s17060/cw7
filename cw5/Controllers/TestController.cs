using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cw5.Models;
using cw5.Requests;
using cw5.Responses;
using System.Data.SqlClient;

namespace cw5.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpPost]
        public IActionResult AddTestStudent()
        {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s17060;Integrated Security=True;MultipleActiveResultSets=true"))
            using (var com = new SqlCommand())
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "INSERT INTO Studies VALUES (4, 'VERY BIG PHARMA')";
                var transaction = con.BeginTransaction();
                com.Transaction = transaction;

                try
                {
                    
                    com.ExecuteNonQuery();
                    transaction.Commit();
                   // com.Dispose();
                  //  con.Close();
                }catch(SqlException e)
                {
                    transaction.Rollback();
                }
            }
            return Ok();
        }
    }
}
