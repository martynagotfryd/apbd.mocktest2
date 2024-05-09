using System.Transactions;
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

        [HttpDelete]
        public async Task<IActionResult> DeleteDoctor(int idDoctor)
        {
            if (!await _repository.DoesDoctorExist(idDoctor))
            {
                return NotFound("Doctor not found");
            }

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    PrescriptionMedDeleteDto listToDelete = await _repository.DoesPrescriptionExist(idDoctor);

                    try
                    {
                        if (listToDelete is not null)
                        {

                            try
                            {
                                foreach (var index in listToDelete.PrescriptionMedDeleteDto2s)
                                {
                                    await _repository.DeletePrescriptionMedInfo(index.IdPrescription);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                return BadRequest("ERROR 3!");

                            }
                            await _repository.DeletePrescriptionInfo(idDoctor);

                        }
                    }
                    catch (Exception )
                    {
                        return BadRequest("ERROR 2!");
                    }
                    
                    await _repository.DeleteDoctorInfo(idDoctor);
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR 1!");
                    return BadRequest("ERROR 1!");
                }
                scope.Complete();
            }
            return Ok();
        }
    }
}