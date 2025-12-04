using Microsoft.EntityFrameworkCore;
using StudentSLC.Data;

namespace StudentSLC.Services
{
    public class CodeGenerator
    {
        private readonly AppDbContext _db;
        private readonly Random _random = new Random();

        public CodeGenerator(AppDbContext db)
        {
            _db = db;
        }

        public async Task<int> GenerateUserCode()
        {
            int code;
            bool exists;

            do
            {
                code = _random.Next(100000, 999999);
                exists = await _db.Users.AnyAsync(u => u.UserCode == code);
            }
            while (exists);

            return code;
        }
    }
}
