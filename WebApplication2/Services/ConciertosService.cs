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

        public async Task<Conciertos> addConcertService(Conciertos concierto)
        {
            Conciertos newConcert = new Conciertos
            {
                address = concierto.address,
                city = concierto.city,
                title = concierto.title,
                description = concierto.description
            };
            await dbContext.AddAsync(newConcert);
            await dbContext.SaveChangesAsync();
            return newConcert;
        }
    }
}