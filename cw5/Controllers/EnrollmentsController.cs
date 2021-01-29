using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cw5.Models;
using cw5.Requests;
using cw5.Responses;
using System.Data.SqlClient;
using cw5.Services;
using cw5.Models;
using cw5.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace cw5.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDbService _service;
        public EnrollmentsController(IStudentDbService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            _service.EnrollStudent(request);
            var response = new EnrollStudentResponse();
            //response.LastName = st.LastName;
            //response.StartDate = DateTime.Now;
            //response.Semester = 1;

            return Ok(response);
        }

        [HttpPost("promotions")]
        [Authorize(Roles = "employee")]
        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {
            _service.PromoteStudent(request.Semester, request.Studies);
            return Ok();
        }
    }

}
