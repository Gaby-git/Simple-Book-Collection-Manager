using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data;
using System.Reflection;

namespace Infrastructure.WrapperClasses
{
    public abstract class DataManagerBase
    {
        private readonly string _configuration;
        #region Constructor
        public DataManagerBase(IAppSettingsWeb configuration)
        {
            _configuration = configuration.ConnectionString;
            Initialize();
        }

        #endregion

        #region Properties
        public Exception LastException = null;
        public int RowsAffected = 0;
        public string LastExceptionMessage = string.Empty;
        private string _nameOrConnectionString;

        public IDbCommand CommandObject { get; set; }
        public DataSet DataSetObject { get; set; }
        public string ConnectStringName { get; set; }
        public string ConnectionString { get; set; }
        public object IdentityGenerated { get; set; }
        public string ParameterToken { get; set; }
        public string SQL { get; set; }
        public bool IsInTransaction { get; set; }
        #endregion

        #region Initialize Method
        protected virtual void Initialize()
        {
            ParameterToken = "@";
            IdentityGenerated = null;
            SQL = string.Empty;
        }
        #endregion

        #region Reset Method
        public virtual void Reset()
        {
            Reset(CommandType.Text);
        }

        public virtual void Reset(CommandType type)
        {
            if (CommandObject != null)
            {
                CommandObject.CommandText = string.Empty;
                CommandObject.CommandType = type;
                CommandObject.Parameters.Clear();
            }
            LastExceptionMessage = string.Empty;
            LastException = null;
            RowsAffected = 0;
            IdentityGenerated = null;
            SQL = string.Empty;

        }
        #endregion

        #region Generic methods
        public virtual T GetElement<T>(int id, string sql, params IDbDataParameter[] parameters)
        {
            T element = default(T);

            SQL = sql;

            using var cnn = new SqlConnection(_configuration);
            cnn.Open();

            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    CommandObject.Parameters.Add(parameters[i]);
                }
            }

            using (IDataReader dr = GetDataReader())
            {

            }
            return element;
        }

        #endregion

        #region CreateParameter Methods
        public abstract IDbDataParameter CreateParameter(string name, object value, bool isNullable);
        public abstract IDbDataParameter CreateParameter(string name, object value, bool isNullable, System.Data.DbType type, System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input);
        public abstract IDbDataParameter CreateParameter(string name, object value, bool isNullable, System.Data.DbType type, int size, System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input);
        #endregion

        #region AddParameter Methods
        public virtual void AddParameter(string name, object value, bool isNullable)
        {
            CommandObject.Parameters.Add(CreateParameter(name, value, isNullable));
        }

        public virtual void AddParameter(string name, object value, bool isNullable, System.Data.DbType type, System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input)
        {
            CommandObject.Parameters.Add(CreateParameter(name, value, isNullable, type, direction));
        }

        public virtual void AddParameter(string name, object value, bool isNullable, System.Data.DbType type, int size, System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input)
        {
            CommandObject.Parameters.Add(CreateParameter(name, value, isNullable, type, size, direction));
        }
        #endregion

        #region GetParameter Method
        public virtual IDataParameter GetParameter(string name)
        {
            if (!name.Contains(ParameterToken))
            {
                name = ParameterToken + name;
            }

            return (IDataParameter)CommandObject.Parameters[name];
        }
        #endregion

        #region GetParameterValue Method
        public abstract T GetParameterValue<T>(string name, object defaultValue);
        #endregion

        #region GetIdentityValue Method
        public virtual T GetIdentityValue<T>()
        {
            return GetIdentityValue<T>((T)default);
        }

        public virtual T GetIdentityValue<T>(object defaultValue)
        {
            T ret = (T)defaultValue;

            if (IdentityGenerated != null)
            {
                ret = (T)Convert.ChangeType(IdentityGenerated, typeof(T));
            }

            return ret;
        }
        #endregion

        #region GetRecords Methods
        public virtual List<T> GetRecords<T>(string sql)
        {
            return GetRecords<T>(sql, CommandType.Text, null);
        }

        public virtual List<T> GetRecords<T>(string sql, params IDbDataParameter[] parameters)
        {
            return GetRecords<T>(sql, CommandType.Text, parameters);
        }

        public virtual List<T> GetRecords<T>(string sql, CommandType type)
        {
            return GetRecords<T>(sql, type, null);
        }

        public virtual List<T> GetRecords<T>(string sql, CommandType type, params IDbDataParameter[] parameters)
        {
            List<T> ret = new List<T>();

            Reset(type);

            SQL = sql;

            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    CommandObject.Parameters.Add(parameters[i]);
                }
            }

            using (IDataReader dr = GetDataReader())
            {
                ret = ToList<T>(dr);
                RowsAffected = ret.Count;
            }

            return ret;
        }
        #endregion

        #region GetRecordsUsingDataSet Method
        public virtual List<T> GetRecordsUsingDataSet<T>(string sql)
        {
            return GetRecordsUsingDataSet<T>(sql, CommandType.Text, null);
        }

        public virtual List<T> GetRecordsUsingDataSet<T>(string sql, params IDbDataParameter[] parameters)
        {
            return GetRecordsUsingDataSet<T>(sql, CommandType.Text, parameters);
        }

        public virtual List<T> GetRecordsUsingDataSet<T>(string sql, CommandType type)
        {
            return GetRecordsUsingDataSet<T>(sql, type, null);
        }

        public virtual List<T> GetRecordsUsingDataSet<T>(string sql, CommandType type, params IDbDataParameter[] parameters)
        {
            List<T> ret;

            Reset(type);

            SQL = sql;

            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    CommandObject.Parameters.Add(parameters[i]);
                }
            }

            DataSetObject = GetDataSet();

            ret = ToList<T>(DataSetObject.Tables[0]);
            RowsAffected = ret.Count;

            return ret;
        }
        #endregion

        #region GetRecord Methods
        public virtual T GetRecord<T>(string sql) where T : new()
        {
            return GetRecord<T>(sql, CommandType.Text, null);
        }

        public virtual T GetRecord<T>(string sql, params IDbDataParameter[] parameters) where T : new()
        {
            return GetRecord<T>(sql, CommandType.Text, parameters);
        }

        public virtual T GetRecord<T>(string sql, CommandType type) where T : new()
        {
            return GetRecord<T>(sql, type, null);
        }

        public virtual T GetRecord<T>(string sql, CommandType type, params IDbDataParameter[] parameters) where T : new()
        {
            T ret = new T();

            Reset(type);

            SQL = sql;

            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    CommandObject.Parameters.Add(parameters[i]);
                }
            }

            using (IDataReader dr = GetDataReader())
            {
                List<T> list = ToList<T>(dr);
                if (list.Count > 0)
                {
                    ret = list[0];
                    RowsAffected = list.Count;
                }
            }

            return ret;
        }
        #endregion

        #region GetRecordUsingDataSet Method
        public virtual T GetRecordUsingDataSet<T>(string sql) where T : new()
        {
            return GetRecordUsingDataSet<T>(sql, CommandType.Text, null);
        }

        public virtual T GetRecordUsingDataSet<T>(string sql, params IDbDataParameter[] parameters) where T : new()
        {
            return GetRecordUsingDataSet<T>(sql, CommandType.Text, parameters);
        }

        public virtual T GetRecordUsingDataSet<T>(string sql, CommandType type) where T : new()
        {
            return GetRecordUsingDataSet<T>(sql, type, null);
        }

        public virtual T GetRecordUsingDataSet<T>(string sql, CommandType type, params IDbDataParameter[] parameters) where T : new()
        {
            T ret = new T();

            Reset(type);

            SQL = sql;

            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    CommandObject.Parameters.Add(parameters[i]);
                }
            }

            DataSetObject = GetDataSet();
            if (DataSetObject.Tables.Count > 0)
            {
                List<T> list = ToList<T>(DataSetObject.Tables[0]);
                if (list.Count > 0)
                {
                    ret = list[0];
                    RowsAffected = list.Count;
                }
            }

            return ret;
        }
        #endregion

        #region CountRecords Methods
        public virtual int CountRecords(string sql)
        {
            return CountRecords(sql, CommandType.Text);
        }

        public virtual int CountRecords(string sql, params IDbDataParameter[] parameters)
        {
            return CountRecords(sql, CommandType.Text, parameters);
        }

        public virtual int CountRecords(string sql, CommandType type)
        {
            return CountRecords(sql, type, null);
        }

        public virtual int CountRecords(string sql, CommandType type, params IDbDataParameter[] parameters)
        {
            Reset(type);

            SQL = sql;

            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    CommandObject.Parameters.Add(parameters[i]);
                }
            }

            RowsAffected = ExecuteScalar<int>();

            return RowsAffected;
        }
        #endregion

        #region CreateConnection Methods
        public virtual IDbConnection CreateConnection()
        {
            return CreateConnection(_configuration);
        }

        public abstract IDbConnection CreateConnection(string connectString);
        #endregion

        #region CreateCommand Methods    
        public abstract IDbCommand CreateCommand();
        #endregion

        #region CreateDataAdapter Methods   
        public abstract DbDataAdapter CreateDataAdapter(IDbCommand cmd);
        #endregion

        #region BeginTransaction Method
        public IDbTransaction BeginTransaction()
        {
            IsInTransaction = true;
            CheckCommand(CommandObject);

            if (CommandObject.Connection.State != ConnectionState.Open)
            {
                CommandObject.Connection.Open();
            }

            CommandObject.Transaction = CommandObject.Connection.BeginTransaction();

            return CommandObject.Transaction;
        }
        #endregion

        #region Commit Method
        public void Commit()
        {
            IsInTransaction = false;
            CommandObject.Transaction.Commit();
        }
        #endregion

        #region Rollback Method
        public void Rollback()
        {
            IsInTransaction = false;
            CommandObject.Transaction.Rollback();
        }
        #endregion

        #region CheckCommand Method
        protected virtual void CheckCommand(IDbCommand cmd)
        {
            if (cmd == null)
            {
                cmd = CreateCommand();
            }

            if (cmd.Connection == null)
            {
                cmd.Connection = CreateConnection();
            }
            if (string.IsNullOrEmpty(cmd.CommandText))
            {
                cmd.CommandText = SQL;
            }
        }
        #endregion

        #region ExecuteScalar Methods
        public virtual T ExecuteScalar<T>(string exceptionMsg = "", T defaultValue = default)
        {
            return ExecuteScalar(CommandObject, exceptionMsg, defaultValue);
        }

        public virtual T ExecuteScalar<T>(IDbCommand cmd, string exceptionMsg = "", T defaultValue = default)
        {
            bool isConnectionOpen;
            T ret = defaultValue;

            RowsAffected = 0;
            IdentityGenerated = null;
            try
            {
                CheckCommand(cmd);

                isConnectionOpen = (cmd.Connection.State == ConnectionState.Open);

                if (!isConnectionOpen)
                {
                    cmd.Connection.Open();
                }

                ret = (T)cmd.ExecuteScalar();

                if (!isConnectionOpen)
                {
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {

            }

            return ret;
        }
        #endregion

        #region ExecuteNonQuery Methods
        public virtual int ExecuteNonQuery()
        {
            return ExecuteNonQuery(CommandObject, false, string.Empty, string.Empty);
        }

        public virtual int ExecuteNonQuery(string exceptionMsg)
        {
            return ExecuteNonQuery(CommandObject, false, string.Empty, exceptionMsg);
        }

        public virtual int ExecuteNonQuery(bool retrieveIdentity)
        {
            return ExecuteNonQuery(CommandObject, retrieveIdentity, string.Empty, string.Empty);
        }

        public virtual int ExecuteNonQuery(bool retrieveIdentity, string exceptionMsg = "")
        {
            return ExecuteNonQuery(CommandObject, retrieveIdentity, string.Empty, exceptionMsg);
        }

        public virtual int ExecuteNonQuery(bool retrieveIdentity, string identityParamName = "", string exceptionMsg = "")
        {
            return ExecuteNonQuery(CommandObject, retrieveIdentity, identityParamName, exceptionMsg);
        }

        public virtual int ExecuteNonQuery(IDbCommand cmd, bool retrieveIdentity = false, string identityParamName = "", string exceptionMsg = "")
        {
            bool isConnectionOpen;

            RowsAffected = 0;
            IdentityGenerated = null;
            try
            {
                CheckCommand(cmd);

                isConnectionOpen = (cmd.Connection.State == ConnectionState.Open);

                if (!isConnectionOpen)
                {
                    cmd.Connection.Open();
                }

                if (retrieveIdentity)
                {
                    if (string.IsNullOrEmpty(identityParamName))
                    {
                        RowsAffected = ExecuteNonQueryUsingDataSet(cmd);
                    }
                    else
                    {
                        RowsAffected = cmd.ExecuteNonQuery();

                        IdentityGenerated = ((IDbDataParameter)cmd.Parameters[identityParamName]).Value;
                    }
                }
                else
                {
                    RowsAffected = cmd.ExecuteNonQuery();
                }

                if (!isConnectionOpen)
                {
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {

            }

            return RowsAffected;
        }
        #endregion

        #region ExecuteNonQueryUsingDataSet Method
        public int ExecuteNonQueryUsingDataSet(IDbCommand cmd)
        {
            return ExecuteNonQueryUsingDataSet(cmd, string.Empty);
        }

        public int ExecuteNonQueryUsingDataSet(IDbCommand cmd, string exceptionMsg)
        {
            DataSetObject = new DataSet();

            RowsAffected = 0;
            IdentityGenerated = null;

            cmd.CommandText += ";SELECT @@ROWCOUNT As RowsAffected, SCOPE_IDENTITY() AS IdentityGenerated";
            try
            {
                using (DbDataAdapter da = CreateDataAdapter(cmd))
                {
                    da.Fill(DataSetObject);
                    if (DataSetObject.Tables.Count > 0)
                    {
                        RowsAffected = (int)DataSetObject.Tables[0].Rows[0]["RowsAffected"];
                        IdentityGenerated = DataSetObject.Tables[0].Rows[0]["IdentityGenerated"];
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return RowsAffected;
        }
        #endregion

        #region GetDataSet Methods
        public virtual DataSet GetDataSet()
        {
            return GetDataSet(CommandObject, string.Empty);
        }

        public virtual DataSet GetDataSet(string exceptionMsg)
        {
            return GetDataSet(CommandObject, exceptionMsg);
        }

        public virtual DataSet GetDataSet(IDbCommand cmd, string exceptionMsg = "")
        {
            DataSetObject = new DataSet();

            try
            {
                CheckCommand(cmd);
                using (DbDataAdapter da = CreateDataAdapter(cmd))
                {
                    da.Fill(DataSetObject);
                }
            }
            catch (Exception ex)
            {

            }

            return DataSetObject;
        }
        #endregion

        #region GetDataReader Methods
        public virtual IDataReader GetDataReader()
        {
            return GetDataReader(CommandObject, string.Empty);
        }

        public virtual IDataReader GetDataReader(string exceptionMsg)
        {
            return GetDataReader(CommandObject, exceptionMsg);
        }

        public virtual IDataReader GetDataReader(IDbCommand cmd, string exceptionMsg = "")
        {
            IDataReader dr = null;

            try
            {
                CheckCommand(cmd);

                cmd.Connection.Open();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {

            }

            return dr;
        }
        #endregion

        #region AddStandardParameters
        public abstract void AddStandardParameters();
        #endregion

        #region GetStandardOutputParameters Method
        public abstract void GetStandardOutputParameters();
        #endregion

        #region ToList Methods
        public virtual List<T> ToList<T>(IDataReader rdr)
        {
            List<T> ret = new List<T>();
            T entity;
            Type typ = typeof(T);
            List<ColumnMapper> columns = new List<ColumnMapper>();
            string name;
            PropertyInfo col;

            PropertyInfo[] props = typ.GetProperties();

            var attrs = props.Where(p => p.GetCustomAttributes(typeof(ColumnAttribute), false).Any()).ToArray();

            for (int index = 0; index < rdr.FieldCount; index++)
            {
                name = rdr.GetName(index);
                col = props.FirstOrDefault(c => c.Name == name);

                if (col == null)
                {
                    for (int i = 0; i < attrs.Length; i++)
                    {
                        var tmp = attrs[i].GetCustomAttribute(typeof(ColumnAttribute));
                        if (tmp != null && ((ColumnAttribute)tmp).Name == name)
                        {
                            col = props.FirstOrDefault(c => c.Name == attrs[i].Name);
                            break;
                        }
                    }
                }

                if (col != null)
                {
                    columns.Add(new ColumnMapper
                    {
                        ColumnName = name,
                        ColumnProperty = col
                    });
                }
            }

            while (rdr.Read())
            {
                entity = Activator.CreateInstance<T>();

                for (int i = 0; i < columns.Count; i++)
                {
                    if (rdr[columns[i].ColumnName].Equals(DBNull.Value))
                    {
                        columns[i].ColumnProperty.SetValue(entity, null, null);
                    }
                    else
                    {
                        columns[i].ColumnProperty.SetValue(entity, rdr[columns[i].ColumnName], null);
                    }
                }

                ret.Add(entity);
            }

            return ret;
        }

        public virtual List<T> ToList<T>(DataTable dt)
        {
            List<T> ret = new List<T>();
            T entity;
            Type typ = typeof(T);
            List<ColumnMapper> columns = new List<ColumnMapper>();
            string name;
            PropertyInfo col;

            PropertyInfo[] props = typ.GetProperties();

            var attrs = props.Where(p => p.GetCustomAttributes(typeof(ColumnAttribute), false).Any()).ToArray();

            for (int index = 0; index < dt.Columns.Count; index++)
            {
                name = dt.Columns[index].ColumnName;
                col = props.FirstOrDefault(c => c.Name == name);

                if (col == null)
                {
                    for (int i = 0; i < attrs.Length; i++)
                    {
                        var tmp = attrs[i].GetCustomAttribute(typeof(ColumnAttribute));
                        if (tmp != null && ((ColumnAttribute)tmp).Name == name)
                        {
                            col = props.FirstOrDefault(c => c.Name == attrs[i].Name);
                            break;
                        }
                    }
                }

                if (col != null)
                {
                    columns.Add(new ColumnMapper
                    {
                        ColumnName = name,
                        ColumnProperty = col
                    });
                }
            }

            for (int rows = 0; rows < dt.Rows.Count; rows++)
            {
                entity = Activator.CreateInstance<T>();

                for (int i = 0; i < columns.Count; i++)
                {
                    if (dt.Rows[rows][columns[i].ColumnName].Equals(DBNull.Value))
                    {
                        columns[i].ColumnProperty.SetValue(entity, null, null);
                    }
                    else
                    {
                        columns[i].ColumnProperty.SetValue(entity, dt.Rows[rows][columns[i].ColumnName], null);
                    }
                }

                ret.Add(entity);
            }

            return ret;
        }
        #endregion



        #region Dispose Method
        public virtual void Dispose()
        {
            if (CommandObject != null)
            {
                if (CommandObject.Connection != null)
                {
                    if (CommandObject.Transaction != null)
                    {
                        CommandObject.Transaction.Dispose();
                    }
                    CommandObject.Connection.Close();
                    CommandObject.Connection.Dispose();
                }
                CommandObject.Dispose();
            }
        }
        #endregion
    }

    public class ColumnMapper
    {
        public string ColumnName { get; set; }
        public PropertyInfo ColumnProperty { get; set; }
    }

}
