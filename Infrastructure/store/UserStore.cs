using AutoMapper;
using Domaine.Model;
using Domaine.Stores;
using Infrastructure.SQL.Users;
using Infrastructure.WrapperClasses;

namespace Infrastructure.store
{
    public class UserStore : SqlServerDataManagerBase, IUserStore
    {
        private readonly IMapper _mapper;

        public UserStore (IAppSettingsWeb configuration, IMapper mapper) : base (configuration)
        {
            _mapper = mapper;
        }

        public IEnumerable<User> GetUsers()
        {
            SQL = SpUser.Names.GetUsers;

            return _mapper.Map<IEnumerable<User>>(
                GetRecords<Entities.User>(SQL, System.Data.CommandType.StoredProcedure));
        }
    }
}
