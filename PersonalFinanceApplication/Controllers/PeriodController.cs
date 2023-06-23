using PersonalFinanceApplication.Models;
using PersonalFinanceApplication.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PersonalFinanceApplication.Controllers
{
    public class PeriodController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PeriodController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44394/api/");
        }

        // GET: Period/List
        public ActionResult List()
        {
            // Objetive: Communicate with period data API to retriee list of periods
            // curl https://localhost:44394/api/PeriodData/ListPeriods

            string url = "PeriodData/ListPeriods";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<PeriodDto> periods = response.Content.ReadAsAsync<IEnumerable<PeriodDto>>().Result;

            return View(periods);
        }

        // GET: Period/Details/5
        public ActionResult Details(int id)
        {
            // Objective: Communicate with period data API to retrieve one period record
            // curl https://localhost:44394/api/PeriodData/FindPeriod/{id}

            DetailsPeriod ViewModel = new DetailsPeriod();

            string url = "PeriodData/FindPeriod/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PeriodDto SelectedPeriod = response.Content.ReadAsAsync<PeriodDto>().Result;

            ViewModel.SelectedPeriod = SelectedPeriod;
            //showcase info about balances related to this period
            //send a request to gather info about balances related to particular period ID
            url = "BalanceData/ListBalancesForPeriod/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<BalanceDto> RelatedBalances = response.Content.ReadAsAsync<IEnumerable<BalanceDto>>().Result;

            ViewModel.RelatedBalances = RelatedBalances;

            //showcase info about cashflows related to this period
            //send aa requeast to gather info about cashflows relted to particular period ID
            url = "CashflowData/ListCashflowsForPeriod/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CashflowDto> RelatedCashflows = response.Content.ReadAsAsync<IEnumerable<CashflowDto>>().Result;

            ViewModel.RelatedCashflows = RelatedCashflows;

            return View(ViewModel);
        }

        // Error Page
        public ActionResult Error()
        {
            return View();
        }

        // GET: Period/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Period/Create
        [HttpPost]
        public ActionResult Create(Period period)
        {
            Debug.WriteLine("JSON payload is:");
            // Debug.WriteLine(period.PeriodMonth)

            // Objective: Add new period record to system using API
            // curl -H "Content-Type:application/json" -d @period.json https://localhost:44394/api/PeriodData/AddPeriod

            string url = "PeriodData/AddPeriod";

            string jsonpayload = jss.Serialize(period);

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

        // GET: Period/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "PeriodData/FindPeriod/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PeriodDto selectedperiod = response.Content.ReadAsAsync<PeriodDto>().Result;

            return View(selectedperiod);
        }

        // POST: Period/Update/5
        [HttpPost]
        public ActionResult Update(int id, Period period)
        {
            string url = "PeriodData/UpdatePeriod/" + id;
            string jsonpayload = jss.Serialize(period);
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

        // GET: Period/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "PeriodData/FindPeriod/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PeriodDto selectedperiod = response.Content.ReadAsAsync<PeriodDto>().Result;
            return View(selectedperiod);
        }

        // POST: Period/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "PeriodData/DeletePeriod/" + id;
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
