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
    public class PeriodDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PeriodData/ListPeriods
        [HttpGet]
        public IEnumerable<PeriodDto> ListPeriods()
        {
            List<Period> Periods = db.Periods.ToList();
            List<PeriodDto> PeriodDtos = new List<PeriodDto>();

            Periods.ForEach(per => PeriodDtos.Add(new PeriodDto()
            {
                PeriodId = per.PeriodId,
                PeriodYear = per.PeriodYear,
                PeriodMonth = per.PeriodMonth
            }));

            return PeriodDtos;
        }

        // GET: api/PeriodData/FindPeriod/5
        [ResponseType(typeof(Period))]
        [HttpGet]
        public IHttpActionResult FindPeriod(int id)
        {
            Period Period = db.Periods.Find(id);
            PeriodDto PeriodDto = new PeriodDto()
            {
                PeriodId = Period.PeriodId,
                PeriodYear = Period.PeriodYear,
                PeriodMonth = Period.PeriodMonth
            };
            if (Period == null)
            {
                return NotFound();
            }

            return Ok(PeriodDto);
        }

        // POST: api/PeriodData/UpdatePeriod/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePeriod(int id, Period period)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != period.PeriodId)
            {
                return BadRequest();
            }

            db.Entry(period).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeriodExists(id))
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

        // POST: api/PeriodData/AddPeriod
        [ResponseType(typeof(Period))]
        [HttpPost]
        public IHttpActionResult AddPeriod(Period period)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Periods.Add(period);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = period.PeriodId }, period);
        }

        // DELETE: api/PeriodData/DeletePeriod/5
        [ResponseType(typeof(Period))]
        [HttpPost]
        public IHttpActionResult DeletePeriod(int id)
        {
            Period period = db.Periods.Find(id);
            if (period == null)
            {
                return NotFound();
            }

            db.Periods.Remove(period);
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

        private bool PeriodExists(int id)
        {
            return db.Periods.Count(e => e.PeriodId == id) > 0;
        }
    }
}