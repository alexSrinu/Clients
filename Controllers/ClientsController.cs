using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Clients.Models;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Clients.Controllers
{
    public class ClientsController : Controller
    {
        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://csadms.com/Devbox/DevboxAPI/")
        };

        [HttpGet]
        public async Task<ActionResult> HomePageCounters()
        {
            try
            {
                
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/HomePageCounterAPI/NewHomePageCounterList", new object());

                if (responseMessage.IsSuccessStatusCode)
                {
                   
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseData);

                  
                    var homeClientsList = apiResponse?.Data ?? new List<HomeClients>();

                   
                    return View(homeClientsList);
                }
                else
                {
                   
                    ViewBag.ErrorMessage = $"API call failed with status code {responseMessage.StatusCode}";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
              
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View("Error"); 
            }
        }
        //[HttpGet]
        //public async Task<ActionResult> EditHomePageCounters(int id)
        //{
        //    string baseAddress = "http://csadms.com/Devbox/DevboxAPI/";
        //    HomeClients homeClient = null;

        //    try
        //    {
        //        using (HttpClient client = new HttpClient { BaseAddress = new Uri(baseAddress) })
        //        {
        //            HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/HomePageCounterAPI/NewHomePageCounterList", new { Id = id });

        //            if (responseMessage.IsSuccessStatusCode)
        //            {
        //                var responseData = await responseMessage.Content.ReadAsStringAsync();
        //                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseData);

        //                if (apiResponse != null && apiResponse.Status)
        //                {
        //                    var homeClientsList = JsonConvert.DeserializeObject<List<HomeClients>>(apiResponse.Data.ToString());
        //                    homeClient = homeClientsList.FirstOrDefault();
        //                }
        //                else
        //                {
        //                    // Log or handle the API response failure
        //                    return View("Error"); // Redirect to the Error view
        //                }
        //            }
        //            else
        //            {
        //                // Log or handle the HTTP request failure
        //                return View("Error"); // Redirect to the Error view
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        return View("Error"); // Redirect to the Error view
        //    }

        //    return View(homeClient); // Pass the HomeClients object to the view
        //}


        //in this  use for loop and data json object will convert into individiual propertie and the put into one object then that object passed return view




       

[HttpGet]
    public async Task<ActionResult> EditHomePageCounters(int id)
    {
        string baseAddress = "http://csadms.com/Devbox/DevboxAPI/";

        using (HttpClient client = new HttpClient { BaseAddress = new Uri(baseAddress) })
        {
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/HomePageCounterAPI/NewHomePageCounterList", new { Id = id });

            if (!responseMessage.IsSuccessStatusCode)
            {
                return new HttpStatusCodeResult((int)responseMessage.StatusCode);
            }

            var responseData = await responseMessage.Content.ReadAsStringAsync();
                var Response = JObject.Parse(responseData);
                var isData = Response.SelectToken("Data").ToString();
                // Deserialize the JSON response into HomePageCounterDto
                var homePageCounter = JsonConvert.DeserializeObject<List<HomeClients>>(isData);

            if (homePageCounter == null)
            {
                return NotFound(); // Handle the case where the response could not be deserialized
            }
                List<HomeClients> objLists = new List<HomeClients>();
                foreach (var listItems in homePageCounter)
                {
                    HomeClients objs = new HomeClients();
                  //  objs.Id = listItems.Id;
                    objs.TextEn = listItems.TextEn;
                    
                    objs.Value = listItems.Value;
                   
                    objs.ImagePath = listItems.ImagePath;
                 
                    objLists.Add(objs);
                   
                }

                return View(objLists);
        }
    }

        private ActionResult NotFound()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult> EditHomePageCounters(HomeClients model)
        {
            string baseAddress = "http://csadms.com/Devbox/DevboxAPI/";

            using (HttpClient client = new HttpClient { BaseAddress = new Uri(baseAddress) })
            {
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/HomePageCounterAPI/NewHomePageCounterList", model);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    var response = JObject.Parse(responseData);
                    bool isStatus = Convert.ToBoolean(response.SelectToken("Status"));

                    if (isStatus)
                    {
                        // Successfully updated, redirect to another page
                        return RedirectToAction("HomePageCounters");
                    }
                    else
                    {
                        // Handle failure case, e.g., show a message to the user
                        ModelState.AddModelError("", response.SelectToken("Message").ToString());
                    }
                }
                else
                {
                    // Handle HTTP error response
                    ModelState.AddModelError("", "An error occurred while updating the data.");
                }
            }

            return View(model); // Return the view with the model to show validation errors
        }


        //[HttpGet]
        //public async Task<ActionResult> EditHomePageCounters(int id)
        //{
         
        //         HomeClients obj = new HomeClients();
        //    string baseAddress = "http://csadms.com/Devbox/DevboxAPI/"; 

        //    using (HttpClient client = new HttpClient { BaseAddress = new Uri(baseAddress) })
        //    {
        //        obj.Id = id;
                      
        //                HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/HomePageCounterAPI/NewHomePageCounterList", obj);
        //                if (responseMessage.IsSuccessStatusCode)
        //                {
        //                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
        //                    var Response = JObject.Parse(responseData);
        //                    bool isStatus = Convert.ToBoolean(Response.SelectToken("Status"));
        //                    string Message = Response.SelectToken("Message").ToString();

        //                    if (isStatus == true)
        //                    {
        //                        var isData = Response.SelectToken("Data").ToString();
        //                        var isJobList = JsonConvert.DeserializeObject<List<HomeClients>>(isData);
        //                        TempData["EditHomePageCounters"] = isJobList.FirstOrDefault();
        //                        obj.Edit = true;
        //                        obj.Create = false;

        //                    }
        //                    else
        //                    {
        //                        obj.Create = true;
        //                    }
        //                }
        //            }
        //    return View();
        //           // return RedirectToAction("HomePageCounters");
                
            
            
        //}
        [HttpPost]
        public async Task<ActionResult> DeleteHomePageCounters(int id)
        {
            HomeClients obj = new HomeClients();

            HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/HomePageClientsAPI/NewAddHomePageClients", obj);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var responseData = responseMessage.Content.ReadAsStringAsync().Result;
        var Response = JObject.Parse(responseData);
        bool isStatus = Convert.ToBoolean(Response.SelectToken("Status"));
        string Message = Response.SelectToken("Message").ToString();

                            if (isStatus == true)
                            {
                                TempData["Success"] = "Deleted Home Page Clients Successful.";
                                obj.Create = true;
                            }
                            else
                            {
                                TempData["Error"] = "Transaction Timeout!.";
                            }
                        }
                    
                    return RedirectToAction("HomePageClients");
                }
        public class ApiResponse
        {
            public List<HomeClients> Data { get; set; }
            public bool Status { get; internal set; }
        }
    }
}
