using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using androidapp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;

namespace androidapp.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public readonly DataBaseContext _context;
        private IHostingEnvironment _hostingEnvironment;

        public ValuesController(IHostingEnvironment environment, DataBaseContext context)
        {
            this._hostingEnvironment = environment;
            this._context = context;
        }

        // GET api/values
        [HttpGet]
        public List<Employee>Get()
        {
            return _context.Employee.ToList();  
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Employee Get(int id)
        {
            return _context.Employee.Find(id);
        }

        // POST api/values
        [HttpPost]
        public Employee Post([FromBody]Employee value)
        {
            _context.Employee.Add(value);
            _context.SaveChanges();
            return value;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public String Put(int id, [FromBody]Employee value)
        {
            Employee e = _context.Employee.Find(id);
            if (e == null)
            {
                return "fail";
            }
            e.firstName = value.firstName;
            e.lastName = value.lastName;
            e.phone = value.phone;
            e.salary = value.salary;
            _context.SaveChanges();
            return "Done";
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public String Delete(int id)
        {
            Employee e = _context.Employee.Find(id);
            if (e == null)
            {
                return "fail";
            }
            _context.Employee.Remove(e);
            _context.SaveChanges();
            return "Done";
        }

        // save to disk
        //api/values/filestodisk/
        [HttpPost("filestodisk")]
        public async Task<IActionResult> savefileToDisk([FromForm]IFormFile file)
        {
            try
            {
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "F:\\uploads");

                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploads, file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            catch (Exception e)
            {

                return Ok(e);
            }


            return Ok("sucess");
        }

        //api/values/files
        [HttpPost("files")]
        public ActionResult uploadfile([FromForm]IFormFile file)
        {
            Images image = new Images();
           
             if (file.Length > 0)
             {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    String mimeType = file.ContentType;
                    image = new Images()
                    {
                        fileName = "file",
                        mimeType = mimeType,
                        fileStore = ms.ToArray()
                    };
                    _context.Images.Add(image);
                    _context.SaveChanges();
                }
             }
            return Ok(image.id);
        }

        //api/values/files/id
        [HttpGet("files/{id}")]
        public ActionResult displayImage(int id)
        {
            var file = _context.Images.Find(id);
            if (file.Equals(null))
            {
                return Ok("not exist");
                //return FileContentResult(Data, Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("image/jpeg"));
            }
            else
            {

                byte[] Data = file.fileStore;
                string contentType = file.mimeType;
                return File(Data, "image/jpg");
            }
            
           

        }
    }
}
