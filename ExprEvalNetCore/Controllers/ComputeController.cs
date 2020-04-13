using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExprEvalNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComputeController : ControllerBase
    {
        [HttpGet]
        public string Compute()
        {
            MathSolver _solverSvc = new MathSolver();
            var result = _solverSvc.Solve(Request.Query["expr"]);

            if (String.IsNullOrEmpty(result.Error))
                return result.Value.ToString();
            else
                return result.Error;
        }
    }
}
