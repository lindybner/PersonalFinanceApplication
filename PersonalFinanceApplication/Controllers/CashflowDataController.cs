using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Controllers
{
    public class CashflowDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CashflowData/ListCashflows
        [HttpGet]
        public IEnumerable<CashflowDto> ListCashflows()
        {
            List<Cashflow> Cashflows = db.Cashflows.ToList();
            List<CashflowDto> CashflowDtos = new List<CashflowDto>();

            Cashflows.ForEach(flow => CashflowDtos.Add(new CashflowDto()
            {
                CashflowId = flow.CashflowId,
                Inflow = flow.Inflow,
                Outflow = flow.Outflow,
                PeriodId = flow.Period.PeriodId,
                PeriodYear = flow.Period.PeriodYear,
                PeriodMonth = flow.Period.PeriodMonth
            }));

            return CashflowDtos;
        }

        // GET: api/CashflowData/FindCashflow/5
        [ResponseType(typeof(Cashflow))]
        [HttpGet]
        public IHttpActionResult FindCashflow(int id)
        {
            Cashflow Cashflow = db.Cashflows.Find(id);
            CashflowDto CashflowDto = new CashflowDto()
            {
                CashflowId = Cashflow.CashflowId,
                Inflow = Cashflow.Inflow,
                Outflow = Cashflow.Outflow,
                PeriodId = Cashflow.Period.PeriodId,
                PeriodYear = Cashflow.Period.PeriodYear,
                PeriodMonth = Cashflow.Period.PeriodMonth
            };
            if (Cashflow == null)
            {
                return NotFound();
            }

            return Ok(CashflowDto);
        }

        // POST: api/CashflowData/UpdateCashflow/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCashflow(int id, Cashflow cashflow)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cashflow.CashflowId)
            {
                return BadRequest();
            }

            db.Entry(cashflow).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CashflowExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CashflowData/AddCashflow
        [ResponseType(typeof(Cashflow))]
        [HttpPost]
        public IHttpActionResult AddCashflow(Cashflow cashflow)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cashflows.Add(cashflow);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cashflow.CashflowId }, cashflow);
        }

        // POST: api/CashflowData/5
        [ResponseType(typeof(Cashflow))]
        [HttpPost]
        public IHttpActionResult DeleteCashflow(int id)
        {
            Cashflow cashflow = db.Cashflows.Find(id);
            if (cashflow == null)
            {
                return NotFound();
            }

            db.Cashflows.Remove(cashflow);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CashflowExists(int id)
        {
            return db.Cashflows.Count(e => e.CashflowId == id) > 0;
        }
    }
}