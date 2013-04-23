using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityFrameworkCallingStoredProcedure
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var context = new TestDatabaseEntities())
			{
				// NOTE uncomment to test queries against the database
				//var users = context.Users.ToList();
				//foreach (var user in users)
				//    Console.WriteLine(user.Name);

				if (context.Connection.State != System.Data.ConnectionState.Open)
					context.Connection.Open();

				var cmd = context.Connection.CreateCommand();
				cmd.CommandText = "TestDatabaseEntities.AddNumbers";
				cmd.CommandType = System.Data.CommandType.StoredProcedure;

				var number1Param = cmd.CreateParameter();
				number1Param.ParameterName = "Number1";
				number1Param.Value = 1;
				number1Param.DbType = System.Data.DbType.Int32;
				cmd.Parameters.Add(number1Param);

				var number2Param = cmd.CreateParameter();
				number2Param.ParameterName = "Number2";
				number2Param.Value = 2;
				number2Param.DbType = System.Data.DbType.Int32;
				cmd.Parameters.Add(number2Param);

				var resultParam = cmd.CreateParameter();
				resultParam.ParameterName = "Result";
				resultParam.DbType = System.Data.DbType.Int32;
				resultParam.Direction = System.Data.ParameterDirection.Output;
				cmd.Parameters.Add(resultParam);

				cmd.ExecuteNonQuery();

				Console.WriteLine("Result: " + resultParam.Value.ToString());
			}

			Console.ReadLine();
		}
	}
}
