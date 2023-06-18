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
    public class CashflowController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        
        static CashflowController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44394/api/");
        }

        // GET: Cashflow/List
        public ActionResult List()
        {
            // Objective: Communicate with cashflow data API to retrieve list of cashflows
            // curl https://localhost:44394/api/CashflowData/ListCashflows

            string url = "CashflowData/ListCashflows";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CashflowDto> cashflows = response.Content.ReadAsAsync<IEnumerable<CashflowDto>>().Result;

            return View(cashflows);
        }

        // GET: Cashflow/Details/5
        public ActionResult Details(int id)
        {
            // Objective: Communicate with cashflow data API to retrieve one cashflow record
            // curl https://localhost:44394/api/CashflowData/FindCashflow/{id}

            string url = "CashflowData/FindCashflow/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CashflowDto selectedCashflow = response.Content.ReadAsAsync<CashflowDto>().Result;

            return View(selectedCashflow);
        }

        // Error Page
        public ActionResult Error()
        {
            return View();
        }

        // GET: Cashflow/New
        public ActionResult New()
        {
            // Info about all periods in system
            // GET: api/PeriodData/ListPeriods
            string url = "PeriodData/ListPeriods";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<PeriodDto> periodOptions = response.Content.ReadAsAsync<IEnumerable<PeriodDto>>().Result;

            return View(periodOptions);

            return View();
        }

        // POST: Cashflow/Create
        [HttpPost]
        public ActionResult Create(Cashflow cashflow)
        {
            Debug.WriteLine("JSON payload is:");
            // Debug.WriteLine(cashflow.Inflow);

            // Objective: Add new cashflow record to system using API
            // curl -H "Content-type:application/json" -d @record.json http://localhost:44394/api/CashflowData/AddCashflow

            string url = "CashflowData/AddCashflow";

            string jsonpayload = jss.Serialize(cashflow);

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

        // GET: Cashflow/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateCashflow ViewModel = new UpdateCashflow();

            // existing cashflow info
            string url = "CashflowData/FindCashflow/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CashflowDto SelectedCashflow = response.Content.ReadAsAsync<CashflowDto>().Result;
            ViewModel.SelectedCashflow = SelectedCashflow;

            // also include all periods to choose from when updating this cashflow record
            url = "PeriodData/ListPeriods/";
            response = client.GetAsync(url).Result;
            IEnumerable<PeriodDto> PeriodOptions = response.Content.ReadAsAsync<IEnumerable<PeriodDto>>().Result;
            ViewModel.PeriodOptions = PeriodOptions;

            return View(ViewModel);
        }

        // POST: Cashflow/Update/5
        [HttpPost]
        public ActionResult Update(int id, Cashflow cashflow)
        {
            string url = "CashflowData/UpdateCashflow/" + id;
            string jsonpayload = jss.Serialize(cashflow);
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

        // GET: Cashflow/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "CashflowData/FindCashflow/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CashflowDto selectedcashflow = response.Content.ReadAsAsync<CashflowDto>().Result;
            return View(selectedcashflow);
        }

        // POST: Cashflow/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "CashflowData/DeleteCashflow/" + id;
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
