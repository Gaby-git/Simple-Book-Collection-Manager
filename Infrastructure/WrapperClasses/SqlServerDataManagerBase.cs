using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.WrapperClasses
{
    public class SqlServerDataManagerBase : DataManagerBase
    {

        #region Constructor
        public SqlServerDataManagerBase(IAppSettingsWeb configuration) : base(configuration) { }
        #endregion

        #region Properties
        public int RETURN_VALUE { get; set; }
        #endregion

        #region Initialize Method
        protected override void Initialize()
        {
            base.Initialize();

            CommandObject = new SqlCommand();
            ParameterToken = "@";
        }
        #endregion

        #region Reset Methods
        public override void Reset(CommandType type)
        {
            base.Reset(type);

            if (CommandObject == null)
            {
                CommandObject = new SqlCommand
                {
                    CommandType = type
                };
            }

            RETURN_VALUE = 0;
        }
        #endregion

        #region CreateConnection Method
        public override IDbConnection CreateConnection(string connectString)
        {
            return new SqlConnection(connectString);
        }
        #endregion

        #region CreateCommand Method
        public override IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }
        #endregion

        #region CreateDataAdapter Method
        public override DbDataAdapter CreateDataAdapter(IDbCommand cmd)
        {
            return new SqlDataAdapter((SqlCommand)cmd);
        }
        #endregion

        #region CreateParameter Methods
        public override IDbDataParameter CreateParameter(string name, object value, bool isNullable)
        {
            name = name.Contains(ParameterToken) ? name : ParameterToken + name;
            return new SqlParameter { ParameterName = name, Value = value, IsNullable = isNullable };
        }

        public override IDbDataParameter CreateParameter(string name, object value, bool isNullable, DbType type, ParameterDirection direction = ParameterDirection.Input)
        {
            name = name.Contains(ParameterToken) ? name : ParameterToken + name;
            return new SqlParameter { ParameterName = name, Value = value, IsNullable = isNullable, DbType = type, Direction = direction };
        }

        public override IDbDataParameter CreateParameter(string name, object value, bool isNullable, DbType type, int size, ParameterDirection direction = ParameterDirection.Input)
        {
            name = name.Contains(ParameterToken) ? name : ParameterToken + name;
            return new SqlParameter { ParameterName = name, Value = value, IsNullable = isNullable, DbType = type, Direction = direction, Size = size };
        }
        #endregion

        #region AddStandardParameters Method
        public override void AddStandardParameters()
        {
            if (CommandObject.CommandType == CommandType.StoredProcedure)
            {
                AddParameter("RETURN_VALUE", 0, false, DbType.Int32, ParameterDirection.ReturnValue);
            }
        }
        #endregion

        #region GetOutputParameters Method
        public override void GetStandardOutputParameters()
        {
            if (CommandObject.CommandType == CommandType.StoredProcedure)
            {
                RETURN_VALUE = GetParameterValue<int>("RETURN_VALUE", default(int));
            }
        }
        #endregion

        #region GetParameterValue Method
        public override T GetParameterValue<T>(string name, object defaultValue)
        {
            T ret;
            string value;
            value = ((SqlParameter)GetParameter(name)).Value.ToString();
            if (string.IsNullOrEmpty(value))
            {
                ret = (T)defaultValue;
            }
            else
            {
                ret = (T)Convert.ChangeType(value, typeof(T));
            }

            return ret;
        }
        #endregion

    }
}
