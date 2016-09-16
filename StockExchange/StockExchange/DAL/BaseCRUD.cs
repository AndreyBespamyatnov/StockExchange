using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Dapper;
using DapperExtensions;
using StockExchange.Extensions;
using StockExchange.Models;

namespace StockExchange.DAL
{
    public class BaseCRUD
    {
        #region private
 
        private readonly string _connectionString = ConfigurationProvider.ConnectionString;
 
        protected void Run(Action<SqlConnection> action)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                action(connection);
            }
        }
         
        #endregion
 
 
        #region dapper
 
        public ErrorCode Execute(string sql)
        {
            try
            {
                Run(connection => connection.Execute(sql));
            }
            catch (SqlException sqlException)
            {
                true.BreakOnTrue(sqlException.Message);
                return sqlException.State == 0 ? ErrorCode.UnknownError : (ErrorCode)sqlException.State;
            }
 
            return ErrorCode.NoError;
        }
 
        protected ErrorCode StoredProcedureVoid(string sql, dynamic param = null)
        {
            try
            {
                Run(connection => SqlMapper.Execute(connection, sql, param));
            }
            catch (SqlException sqlException)
            {
                true.BreakOnTrue(sqlException.Message);
                return sqlException.State == 0 ? ErrorCode.UnknownError : (ErrorCode)sqlException.State;
            }
 
            return ErrorCode.NoError;
        }
 
        protected StatusValuePair<T> StoredProcedureSingle<T>(string sql, dynamic param = null)
        {
            StatusValuePair<List<T>> executeDapperList = StoredProcedureList<T>(sql, param);
            if (executeDapperList.HasValue && executeDapperList.Value.Count > 0)
            {
                return executeDapperList.Value.Single();
            }
 
            return new StatusValuePair<T>(default(T), executeDapperList.ErrorCode);
        }
 
        protected StatusValuePair<List<T>> StoredProcedureList<T>(string sql, dynamic param = null)
        {
            List<T> result = new List<T>();
 
            try
            {
                Run(connection => result = SqlMapper.Query<T>(connection, sql, param));
            }
            catch (SqlException sqlException)
            {
                true.BreakOnTrue(sqlException.Message);
                Trace.TraceInformation(sqlException.Message);
                return new StatusValuePair<List<T>>(null, (ErrorCode) sqlException.State);
            }
 
            return result;
        }
 
        #endregion
 
 
        #region dapper ext
 
        protected StatusValuePair<T> SingleByID<T>(dynamic id) where T : class
        {
            T result = null;
            try
            {
                Run(cn => result = DapperExtensions.DapperExtensions.Get<T>(cn, id));
            }
            catch (SqlException sqlException)
            {
                true.BreakOnTrue(sqlException.Message);
                Trace.TraceInformation(sqlException.Message);
                return new StatusValuePair<T>(default(T), (ErrorCode) sqlException.State);
            }
 
            if (result == null)
            {
                return new StatusValuePair<T>(default(T), ErrorCode.EntityDoesNotExist);
            }
 
            return result;
        }
 
        protected StatusValuePair<T> Create<T>(T entity) where T : class
        {
            try
            {
                Run(cn => cn.Insert(entity));
                return entity;
            }
            catch (SqlException sqlException)
            {
                true.BreakOnTrue(sqlException.Message);
                Trace.TraceInformation(sqlException.Message);
                return new StatusValuePair<T>(default(T), (ErrorCode)sqlException.State);
            }
        }
 
        protected StatusValuePair<T> Delete<T>(IPredicate predicate) where T : class
        {
            try
            {
                Run(cn => cn.Delete<T>(predicate));
                return new StatusValuePair<T>(default(T), ErrorCode.NoError);
            }
            catch (SqlException sqlException)
            {
                true.BreakOnTrue(sqlException.Message);
                Trace.TraceInformation(sqlException.Message);
                return new StatusValuePair<T>(default(T), (ErrorCode)sqlException.State);
            }
        }
 
        protected void Insert<T>(T entity) where T : class
        {
            try
            {
                Run(cn => cn.Insert(entity));
            }
            catch (SqlException sqlException)
            {
                true.BreakOnTrue(sqlException.Message);
                Trace.TraceInformation(sqlException.Message);
            }
        }
 
        protected StatusValuePair<T> Update<T>(T entity) where T : class
        {
            try
            {
                Run(cn => cn.Update(entity));
                return entity;
            }
            catch (SqlException sqlException)
            {
                true.BreakOnTrue(sqlException.Message);
                Trace.TraceInformation(sqlException.Message);
                return new StatusValuePair<T>(default(T), (ErrorCode)sqlException.State);
            }
        }
         
        protected StatusValuePair<List<T>> GetList<T>(IPredicate predicate) where T : class
        {
            try
            {
                List<T> list = null;
                Run(cn => list = cn.GetList<T>(predicate).ToList());
                return list;
            }
            catch (SqlException sqlException)
            {
                true.BreakOnTrue(sqlException.Message);
                Trace.TraceInformation(sqlException.Message);
                return new StatusValuePair<List<T>>(null, (ErrorCode)sqlException.State);
            }
        }
 
        protected List<T> GetListSafe<T>(IPredicate predicate) where T : class
        {
            StatusValuePair<List<T>> listStatusValuePair = GetList<T>(predicate);
            return listStatusValuePair.HasValue ? listStatusValuePair.Value : new List<T>();
        }
 
        protected StatusValuePair<List<T>> GetList<T>() where T : class
        {
            try
            {
                List<T> list = null;
                Run(cn => list = cn.GetList<T>().ToList());
                return list;
            }
            catch (SqlException sqlException)
            {
                true.BreakOnTrue(sqlException.Message);
                Trace.TraceInformation(sqlException.Message);
                return new StatusValuePair<List<T>>(null, (ErrorCode)sqlException.State);
            }
        }
 
        protected StatusValuePair<T> GetSingle<T>(IPredicate predicate) where T : class
        {
            try
            {
                List<T> list = null;
                Run(cn => list = cn.GetList<T>(predicate).ToList());
                var single = list.SingleOrDefault();
                if (single == null)
                {
                    return new StatusValuePair<T>(null, ErrorCode.EntityDoesNotExist);
                }
 
                return single;
            }
            catch (SqlException sqlException)
            {
                true.BreakOnTrue(sqlException.Message);
                return new StatusValuePair<T>(null, (ErrorCode) sqlException.State);
            }
        }
         
        #endregion
 
    }
}