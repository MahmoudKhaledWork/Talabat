using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Repository.Data;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Controllers
{    
    [ApiController]
    public class BuggyController : ApiBaseController
    {
        private readonly StoreContext _dbcontext;

        public BuggyController(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet("NotFound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = _dbcontext.Products.Find(100);
            if (product is null) return NotFound(new ApiResponse(404));
            return Ok(product);
        }

        [HttpGet("ServerError")]
        public ActionResult GetServerError()
        {
            var product = _dbcontext.Products.Find(100);
            // Cant Convert null to String 
            var productToReturn = product.ToString();
            // Will Return null refernce exception 
            return Ok(productToReturn);
        }
        

        [HttpGet("BadRequest")]
        public ActionResult GetbadRequest()
        {
            return BadRequest();
        }

        [HttpGet("BadRequest/{id}")]
        public ActionResult GetValidationError(int id )
        {
            return Ok();
        }





    }
}
