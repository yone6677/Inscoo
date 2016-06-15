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
using Logger.Models;

namespace Logger.Controllers
{
    public class LogsController : ApiController
    {
        private EfDbContext db = new EfDbContext();

        // GET: api/Logs
        public IQueryable<Logs> Getlog()
        {
            return db.log;
        }

        // GET: api/Logs/5
        [ResponseType(typeof(Logs))]
        public IHttpActionResult GetLogs(int id)
        {
            Logs logs = db.log.Find(id);
            if (logs == null)
            {
                return NotFound();
            }

            return Ok(logs);
        }

        // PUT: api/Logs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLogs(int id, Logs logs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != logs.Id)
            {
                return BadRequest();
            }

            db.Entry(logs).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogsExists(id))
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

        // POST: api/Logs
        [ResponseType(typeof(Logs))]
        public IHttpActionResult PostLogs(Logs logs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.log.Add(logs);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = logs.Id }, logs);
        }

        // DELETE: api/Logs/5
        [ResponseType(typeof(Logs))]
        public IHttpActionResult DeleteLogs(int id)
        {
            Logs logs = db.log.Find(id);
            if (logs == null)
            {
                return NotFound();
            }

            db.log.Remove(logs);
            db.SaveChanges();

            return Ok(logs);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LogsExists(int id)
        {
            return db.log.Count(e => e.Id == id) > 0;
        }
    }
}