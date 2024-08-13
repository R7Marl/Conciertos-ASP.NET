using Microsoft.EntityFrameworkCore;
using WebApplication2.Entity;

namespace WebApplication2.Services
{
    public class ConciertosService
    {
        private readonly AppDbContext dbContext;
        public ConciertosService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IList<Conciertos>> GetAllConciertosByCity(string city)
        {
            try
            {
                return await dbContext.Conciertos.Where(c => c.city == city).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ConciertosService.cs " + ex.Message);
                throw;
            }
        }

        public async Task<Conciertos> AddConcertService(Conciertos concierto)
        {
            try
            {
            var newConcert = new Conciertos(concierto);
            await dbContext.AddAsync(newConcert);
            await dbContext.SaveChangesAsync();
            return newConcert;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}