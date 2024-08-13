using ConciertosNET.Utils;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Entity;

namespace ConciertosNET.Services
{
    public class UserService
    {
        AppDbContext dbContext;
        public UserService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            try
            {
            return await dbContext.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw; 
            }
        }
        public async Task<Users> GetUserByEmail(string email)
        {
            var UserFetched = await dbContext.Users.Where(u => u.email == email).FirstOrDefaultAsync();
            return UserFetched;
        }
        public async Task<Users> CreateUser(Users user)
        {
            try
            {
                var NewUser = new Users(user);
                await dbContext.Users.AddAsync(NewUser);
                await dbContext.SaveChangesAsync();
                return NewUser;
            } catch(Exception ex) 
            {
                throw new ApplicationException(ex.Message);
            }
        }
        public string HashedPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
        public Boolean verifyPassword(string userPassword, string password) => BCrypt.Net.BCrypt.Verify(password, userPassword);
    }
}
