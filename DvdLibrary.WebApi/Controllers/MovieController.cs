using DvdLibrary.Data;
using DvdLibrary.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
//using System.Data.Entity;
//using Dashboard.Data.Entities;

namespace Dashboard.WebApi.Controllers
{
    [RoutePrefix("api/Movie")]
    public class MovieController : ApiController, IDisposable
    {
        private DvdLibraryEntityModel _dbContext;

        public MovieController()
        {
            _dbContext = new DvdLibraryEntityModel();
            CreateMaps();
        }

        [HttpGet]
        [Route("AvailableMovies", Name = "AvailableMovies")]
        public async Task<IHttpActionResult> GetAvailableMovies(string q = null)
        {
            ICollection<Movie> movies = null;
            if (!string.IsNullOrEmpty(q))
            {
                movies = await _dbContext.Movies.Where(m => m.Name.Contains(q)).Include(m => m.MovieCopies).ToListAsync();
            }
            else
            {
                movies = await _dbContext.Movies.Include(m => m.MovieCopies).ToListAsync();
            }

            return Ok(AutoMapper.Mapper.Map<List<MovieModel>>(movies));
        }


        private void CreateMaps()
        {
            AutoMapper.Mapper.CreateMap<DvdLibrary.Data.Movie, MovieModel>()
                .ForMember(m => m.AvailableCount, e => e.MapFrom(src => src.MovieCopies.Count)).ReverseMap();
        }

        //[HttpGet]
        //public async Task<IHttpActionResult> GetAsync(int id)
        //{
        //    var eventItem = await _dbContext.Events.Include(e=>e.Patient).SingleOrDefaultAsync(e => e.Id == id);
        //    return Ok(AutoMapper.Mapper.Map<EventModel>(eventItem));
        //}

        //public async Task<IHttpActionResult> GetAllAsync()
        //{
        //    var eventItems = await _dbContext.Events.Include(e => e.Patient).ToListAsync();
        //    return Ok(AutoMapper.Mapper.Map<List<EventModel>>(eventItems));
        //}

        //[HttpPost]
        //public async Task<HttpResponseMessage> PostAsync(Guid eventGuid, [FromBody]EventModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var eventItem = await _dbContext.Events.SingleOrDefaultAsync(e => e.Guid == model.Guid);
        //        if (eventItem != null)
        //        {
        //            eventItem.Description = model.Description;
        //            eventItem.StartsAt = model.StartsAt;
        //            eventItem.EndsAt = model.EndsAt;
        //            eventItem.Title = model.Title;
        //        }
        //        else
        //        {
        //            _dbContext.Events.Add(AutoMapper.Mapper.Map<Event>(model));
        //        }

        //        await _dbContext.SaveChangesAsync();
        //        return Request.CreateResponse<EventModel>(System.Net.HttpStatusCode.Created, model);
        //    }
        //    else
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }
        //}

        //[HttpDelete]
        //public async Task<HttpResponseMessage> DeleteAsync(Guid eventGuid, [FromBody]EventModel model)
        //{
        //    if (model != null && eventGuid != Guid.Empty)
        //    {
        //        var eventItem = await _dbContext.Events.SingleOrDefaultAsync(e => e.Guid == eventGuid);
        //        if (eventItem != null)
        //        {
        //            _dbContext.Events.Remove(eventItem);
        //            await _dbContext.SaveChangesAsync();
        //        }
        //    }
        //    return Request.CreateResponse<EventModel>(HttpStatusCode.Accepted, model);
        //}

        //private void CreateMaps()
        //{
        //    AutoMapper.Mapper.CreateMap<Event, EventModel>().ReverseMap();
        //    AutoMapper.Mapper.CreateMap<Patient, PatientModel>().ReverseMap();
        //}

        //public void Dispose()
        //{
        //    if (_dbContext != null)
        //    {
        //        _dbContext.Dispose();
        //    }
        //}
    }
}
