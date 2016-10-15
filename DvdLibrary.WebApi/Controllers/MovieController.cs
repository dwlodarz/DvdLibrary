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
    }
}
