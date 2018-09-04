using System;
using System.Data.SqlClient;

namespace Saned.ArousQatar.Data.Persistence.Repositories
{
    public class BaseRepositroy
    {

        public static SqlParameter Getparamter(object id, string parameterName)
        {
            var accountTypeIdParameter = id == null
                ? new SqlParameter("@" + parameterName, DBNull.Value)
                : new SqlParameter("@" + parameterName, id);
            return accountTypeIdParameter;
        }
    }
}