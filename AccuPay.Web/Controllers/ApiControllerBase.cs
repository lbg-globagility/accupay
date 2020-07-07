using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    public class ApiControllerBase : ControllerBase
    {

        public ApiControllerBase()
        {

        }

        public ActionResult GetTemplate(string path, string fileName)
        {
            var file = Path.Combine(path, fileName);

            var result = PhysicalFile(file, "application/vnd.ms-excel");

            Response.Headers["Content-Disposition"] = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Accupay Leave Template"
            }.ToString();

            return result;
        }
    }
}
