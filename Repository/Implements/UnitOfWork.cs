using newplgapi.Repository.Interfaces;

namespace newplgapi.Repository.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDapperContext _context;
        private IAuthRepository _authRepository;
        private IPlgRepository _plgRepository;

        public UnitOfWork(IDapperContext context)
        {
            _context = context;
        }
        public IAuthRepository AuthRepository {
            get { return _authRepository ?? (_authRepository = new AuthRepository(_context)); }
        }

        public IPlgRepository PlgRepository {
            get { return _plgRepository ?? (_plgRepository = new PlgRepository(_context)); }
        }
    }
}