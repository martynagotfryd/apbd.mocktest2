using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
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
        
        
    }
}