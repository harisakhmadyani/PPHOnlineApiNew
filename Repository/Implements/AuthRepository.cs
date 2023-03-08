using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using newplgapi.model;
using newplgapi.model.Dto;
using newplgapi.Repository.Interfaces;

namespace newplgapi.Repository.Implements
{
    internal class AuthRepository : IAuthRepository
    {
        private IDapperContext _context;

        public AuthRepository(IDapperContext context)
        {
            _context = context;
        }

        public Task<bool> ChangePassword(string username, string passwordold, string passwordnew)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.db.GetAllAsync<User>();
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _context.db.QueryFirstOrDefaultAsync<User>("Select * FROM plg_tblMstUser where userId=@userId AND NotActive = 0", new { userId = username });

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.passwordHash, user.passwordSalt))
                return null;

            HistoryLogin userLogin = new HistoryLogin{
                UserID = username,
                Tanggal = DateTime.Now
            };

            _context.BeginTransaction();
            await _context.db.InsertAsync<HistoryLogin>(userLogin, _context.transaction);
            _context.Commit();
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {

                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            try
            {
                _context.BeginTransaction();
                byte[] passwordHash;
                byte[] passwordSalt;
                this.CreatePasswordHash(password, out passwordHash, out passwordSalt);
                string secret = this.RandomString() + user.userId;
                user.passwordHash = passwordHash;
                user.passwordSalt = passwordSalt;
                user.SecretKey = secret;
                int num = await _context.db.InsertAsync<User>(user, _context.transaction);
                User user1 = user;
                passwordHash = (byte[])null;
                passwordSalt = (byte[])null;
                secret = (string)null;
                _context.Commit();
                return user1;
            }
            catch (System.Exception)
            {
                _context.Rollback();
                 throw;
            }
           
        }

        private string RandomString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();
            for (int index = 0; index < 20; ++index)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26.0 * random.NextDouble() + 65.0)));
                stringBuilder.Append(ch);
            }
            return stringBuilder.ToString();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (HMACSHA512 hmacshA512 = new HMACSHA512())
            {
                passwordSalt = hmacshA512.Key;
                passwordHash = hmacshA512.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public Task<AuthReg> RegisterGA(string user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UserExists(string username)
        {
            throw new System.NotImplementedException();
        }
    }
}