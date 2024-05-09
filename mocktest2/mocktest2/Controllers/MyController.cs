using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using mocktest2.Models.DTOs;
using mocktest2.Repositories;

namespace mocktest2.Controllers
{
    [Route("api/doctors")]
    [Host]
    public class MyController : ControllerBase
    {
        private readonly IRepository _repository;

        public MyController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctor(int idDoctor)
        {
            if (!await _repository.DoesDoctorExist(idDoctor))
            {
                return NotFound("Doctor not found");
            }

            var doctor = await _repository.GetDoctorInfo(idDoctor);
            
            return Ok(doctor);
        }
    }
}