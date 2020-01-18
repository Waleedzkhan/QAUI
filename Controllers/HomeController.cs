using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QAUI.Models;

namespace QAUI.Controllers
{
    public class HomeController : Controller
    {
        
            //Hosted web API REST Service base url  
            string url = "https://localhost:44378/api/Answers/";
        public async Task<IActionResult> Index()
        {
            List<Answer> answerList = new List<Answer>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    answerList = JsonConvert.DeserializeObject<List<Answer>>(apiResponse);
                }
            }
            return View(answerList);
        }

        public ViewResult GetAnswer() => View();

        [HttpPost]
        public async Task<IActionResult> GetAnswer(long id)
        {
            Answer answer = new Answer();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    answer = JsonConvert.DeserializeObject<Answer>(apiResponse);
                }
            }
            return View(answer);
        }

        public ViewResult AddAnswer() => View();

        [HttpPost]
        public async Task<IActionResult> AddAnswer(Answer Answer)
        {
            Answer receivedAnswer = new Answer();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(Answer), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(url, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedAnswer = JsonConvert.DeserializeObject<Answer>(apiResponse);
                }
            }
            return View(receivedAnswer);
        }
        
        public async Task<IActionResult> UpdateAnswer(long Id)
        {
            Answer Answer = new Answer();
            using (var httpClient = new HttpClient())
            {
                

                using (var response = await httpClient.GetAsync(url + Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Answer = JsonConvert.DeserializeObject<Answer>(apiResponse);
                }
            }
            return View(Answer);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAnswer(Answer Answer)
        {
            Answer receivedAnswer = new Answer();

            using (var httpClient = new HttpClient())
            {
                var content = new MultipartFormDataContent();
                
                content.Add(new StringContent(Answer.Id.ToString()), "Id");
                
                content.Add(new StringContent(Answer.Body.ToString()), "Body");
                content.Add(new StringContent(Answer.Accepted.ToString()), "Accepted");
                long Id = Answer.Id;
                
                using (var response = await httpClient.PutAsync(url+Id, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                   receivedAnswer = JsonConvert.DeserializeObject<Answer>(apiResponse);
                   
                }
            }
            return View(receivedAnswer);
        }


        public async Task<IActionResult> UpdateAnswerPatch(int Id)
        {
            Answer Answer = new Answer();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url + Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Answer = JsonConvert.DeserializeObject<Answer>(apiResponse);
                }
            }
            return View(Answer);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAnswerPatch(long Id, Answer Answer)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url + Id),
                    Method = new HttpMethod("Patch"),
                    Content = new StringContent("[{ \"op\": \"replace\", \"path\": \"Body\", \"value\": \"" + Answer.Body +"\"},{ \"op\": \"replace\", \"path\":\"Accepted\", \"value\": \"" + Answer.Accepted + "\"}]", Encoding.UTF8, "application/json")};
                

                var response = await httpClient.SendAsync(request);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAnswer(long Id)
        {
            
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(url+Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }

    }
    
}
