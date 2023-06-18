using PersonalFinanceApplication.Models;
using PersonalFinanceApplication.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PersonalFinanceApplication.Controllers
{
    public class BalanceController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BalanceController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44394/api/");
        }

        // GET: Balance/List
        public ActionResult List()
        {
            // Objective: Communicate with balance data API to retrieve list of balances
            // curl https://localhost:44394/api/BalanceData/ListBalances

            string url = "BalanceData/ListBalances";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<BalanceDto> balances = response.Content.ReadAsAsync<IEnumerable<BalanceDto>>().Result;

            return View(balances);
        }

        // GET: Balance/Details/5
        public ActionResult Details(int id)
        {
            // Objective: Communicate with balance data API to retrieve one balance record
            // curl https://localhost:44394/api/BalanceData/FindBalance/{id}

            string url = "BalanceData/FindBalance/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BalanceDto selectedBalance = response.Content.ReadAsAsync<BalanceDto>().Result;

            return View(selectedBalance);
        }

        // Error Page
        public ActionResult Error()
        {
            return View();
        }

        // GET: Balance/New
        public ActionResult New()
        {
            // info about all periods in system
            // GET: api/PeriodData/ListPeriods

            string url = "PeriodData/ListPeriods";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<PeriodDto> periodOptions = response.Content.ReadAsAsync<IEnumerable<PeriodDto>>().Result;

            return View(periodOptions);
        }

        // POST: Balance/Create
        [HttpPost]
        public ActionResult Create(Balance balance)
        {
            Debug.WriteLine("JSON payload is:");
            // Debug.WriteLine(balance.OwnBalance);

            // Objective: Add new balance record to system using API
            // curl -H "Content-Type:application/json" -d @balance.json https://localhost:44394/api/BalanceData/AddBalance

            string url = "BalanceData/AddBalance";

            string jsonpayload = jss.Serialize(balance);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Balance/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateBalance ViewModel = new UpdateBalance();

            // existing balanc info
            string url = "BalanceData/FindBalance/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BalanceDto SelectedBalance = response.Content.ReadAsAsync<BalanceDto>().Result;
            ViewModel.SelectedBalance = SelectedBalance;

            // also include all periods to choose from when updating this balance record
            url = "PeriodData/ListPeriods/";
            response = client.GetAsync(url).Result;
            IEnumerable<PeriodDto> PeriodOptions = response.Content.ReadAsAsync<IEnumerable<PeriodDto>>().Result;
            ViewModel.PeriodOptions = PeriodOptions;

            return View(ViewModel);
        }

        // POST: Balance/Update/5
        [HttpPost]
        public ActionResult Update(int id, Balance balance)
        {
            string url = "BalanceData/UpdateBalance/" + id;
            string jsonpayload = jss.Serialize(balance);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Balance/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "BalanceData/FindBalance/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BalanceDto selectedbalance = response.Content.ReadAsAsync<BalanceDto>().Result;
            return View(selectedbalance);
        }

        // POST: Balance/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "BalanceData/DeleteBalance/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
