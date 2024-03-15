using eTickets.Data.Base;
using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public class CinemasService : EntityBaseRepository<Cinema>, ICinemasService
    {
        public CinemasService(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> HasAssociatedMoviesAsync(int cinemaId)
        {
            // Check if the producer has associated movies
            return await _context.Movies.AnyAsync(m => m.CinemaId == cinemaId);
        }
    }
}
