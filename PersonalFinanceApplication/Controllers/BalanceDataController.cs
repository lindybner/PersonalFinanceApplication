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
    public class BalanceDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/BalanceData/ListBalances
        [HttpGet]
        public IEnumerable<BalanceDto> ListBalances()
        {
            List<Balance> Balances = db.Balances.ToList();
            List<BalanceDto> BalanceDtos = new List<BalanceDto>();

            Balances.ForEach(bal => BalanceDtos.Add(new BalanceDto()
            {
                BalanceId = bal.BalanceId,
                OwnBalance = bal.OwnBalance,
                OweBalance = bal.OweBalance,
                PeriodId = bal.Period.PeriodId,
                PeriodYear = bal.Period.PeriodYear,
                PeriodMonth = bal.Period.PeriodMonth
            }));

            return BalanceDtos;
        }

        // GET: api/BalanceData/FindBalance/5
        [ResponseType(typeof(Balance))]
        [HttpGet]
        public IHttpActionResult FindBalance(int id)
        {
            Balance Balance = db.Balances.Find(id);
            BalanceDto BalanceDto = new BalanceDto()
            {
                BalanceId = Balance.BalanceId,
                OwnBalance = Balance.OwnBalance,
                OweBalance = Balance.OweBalance,
                PeriodId = Balance.Period.PeriodId,
                PeriodYear = Balance.Period.PeriodYear,
                PeriodMonth = Balance.Period.PeriodMonth
            };
            if (Balance == null)
            {
                return NotFound();
            }

            return Ok(BalanceDto);
        }

        // POST: api/BalanceData/UpdateBalance/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateBalance(int id, Balance balance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != balance.BalanceId)
            {
                return BadRequest();
            }

            db.Entry(balance).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BalanceExists(id))
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

        // POST: api/BalanceData/AddBalance
        [ResponseType(typeof(Balance))]
        [HttpPost]
        public IHttpActionResult AddBalance(Balance balance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Balances.Add(balance);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = balance.BalanceId }, balance);
        }

        // POST: api/BalanceData/DeleteBalance/5
        [ResponseType(typeof(Balance))]
        [HttpPost]
        public IHttpActionResult DeleteBalance(int id)
        {
            Balance balance = db.Balances.Find(id);
            if (balance == null)
            {
                return NotFound();
            }

            db.Balances.Remove(balance);
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

        private bool BalanceExists(int id)
        {
            return db.Balances.Count(e => e.BalanceId == id) > 0;
        }
    }
}