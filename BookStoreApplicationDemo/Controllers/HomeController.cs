using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookStoreApplicationDemo.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace BookStoreApplicationDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        //string Baseurl = string.Empty;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //Hosted web API REST Service base url  
        //"http://192.168.95.1:5555/";

        public async Task<ActionResult> Index()
        {
            List<Book> EmpInfo = new List<Book>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(_configuration.GetSection("baseUri").Value);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Employee/GetAllEmployees");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    EmpInfo = JsonConvert.DeserializeObject<List<Book>>(EmpResponse);

                }
                //returning the employee list to view  
                return View(EmpInfo);
            }
        }
    }
}