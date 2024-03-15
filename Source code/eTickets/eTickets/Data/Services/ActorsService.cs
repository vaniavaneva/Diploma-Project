using eTickets.Data.Base;
using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public class ActorsService : EntityBaseRepository<Actor>, IActorsService
    {
        public ActorsService(AppDbContext context) : base(context) { }

        public async Task<bool> HasAssociatedMoviesAsync(int actorId)
        {
            return await _context.Actors_Movies.AnyAsync(am => am.ActorId == actorId);
        }
    }
}
