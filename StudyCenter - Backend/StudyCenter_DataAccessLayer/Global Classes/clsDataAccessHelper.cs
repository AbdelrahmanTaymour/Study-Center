using System.Data;
using Microsoft.Data.SqlClient;

namespace StudyCenter_DataAccessLayer.Global_Classes;

public class clsDataAccessHelper
{
    /// <summary>
    /// Handles exceptions by logging them appropriately.
    /// </summary>
    /// <param name="ex">The exception to handle.</param>
    public static void HandleException(Exception ex)
    {
        if (ex is SqlException sqlEx)
        {
            //TODO: IMPLEMNT LOGGER
        }
        else
        {
            //TODO: IMPLEMNT LOGGER
        }
    }


    /// <summary>
    /// Retrieves all records from a stored procedure and maps them to a list of objects.
    /// </summary>
    /// <typeparam name="T">The type of the objects to return.</typeparam>
    /// <param name="storedProcedureName">The name of the stored procedure.</param>
    /// <param name="mapFunction">A function to map IDataRecord to the specified type.</param>
    /// <param name="parameters">Optional parameters for the stored procedure.</param>
    /// <returns>A list of mapped objects.</returns>
    public static List<T> All<T>(string storedProcedureName, Func<IDataRecord, T> mapFunction,
        (string name, object? value)[]? parameters = null)
    {
        var list = new List<T>();

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters to the command
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue($"@{parameter.name}", parameter.value ?? DBNull.Value);
                        }
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = mapFunction(reader);
                            list.Add(item);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return list;
    }

    public static List<T> All<T>(string storedProcedureName, string parameterName, T value, Func<IDataRecord, T> mapFunction)
    {
        var list = new List<T>();

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue($"@{parameterName}", (object?)value ?? DBNull.Value);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            var item = mapFunction(reader);
                            list.Add(item);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return list;
    }

    /// <summary>
    /// Retrieves a single record from a stored procedure using a single parameter.
    /// </summary>
    /// <typeparam name="T">The type of the object to return.</typeparam>
    /// <param name="storedProcedureName">The name of the stored procedure.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="parameterValue">The value of the parameter.</param>
    /// <param name="mapper">A function to map IDataRecord to the specified type.</param>
    /// <returns>The mapped object or default if not found.</returns>
    public static T? GetBy<T>(string storedProcedureName, string parameterName, object? parameterValue,
        Func<IDataRecord, T> mapper)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue($"@{parameterName}", parameterValue ?? DBNull.Value);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return mapper(reader);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return default;
    }


    public static T? GetBy<T>(string storedProcedureName, (string name, object? parameterValue)[] parameters,
        Func<IDataRecord, T?> mapper)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    connection.Open();
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue($"@{parameter.name}", parameter.parameterValue ?? DBNull.Value);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return mapper(reader);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return default;
    }


    /// <summary>
    /// Creates SQL parameters from an object's properties.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="dto">The object to create parameters from.</param>
    /// <returns>A list of SQL parameters.</returns>
    private static List<SqlParameter> _CreateInputParameters<T>(T dto)
    {
        var parameters = new List<SqlParameter>();
        foreach (var property in typeof(T).GetProperties())
        {
            var value = property.GetValue(dto);
            var parameter = new SqlParameter($"@{property.Name}", value ?? DBNull.Value);
            parameters.Add(parameter);
        }

        return parameters;
    }


    /// <summary>
    /// Adds input parameters to a SqlCommand.
    /// </summary>
    /// <param name="command">The SQL command.</param>
    /// <param name="parameters">The list of parameters to add.</param>
    private static void _AddInputParametersToSqlCommand(SqlCommand command, List<SqlParameter> parameters)
    {
        if (command == null || parameters == null || parameters.Count == 0) return;

        foreach (var parameter in parameters)
        {
            command.Parameters.Add(parameter);
        }
    }

    private static SqlParameter _CreateOutputParameter(string outputParameterName)
    {
        return new SqlParameter(outputParameterName, SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
    }


    /// <summary>
    /// Adds a new record using a stored procedure and returns the newly generated ID.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="storedProcedureName">The stored procedure name.</param>
    /// <param name="outputParameterName">The name of the output parameter.</param>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>The new ID or null if insertion fails.</returns>
    public static int? Add<T>(string storedProcedureName, string outputParameterName, T entity)
    {
        int? newId = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var inputParameters = _CreateInputParameters(entity);
                    _AddInputParametersToSqlCommand(command, inputParameters);

                    SqlParameter outputParameter = _CreateOutputParameter(outputParameterName);
                    command.Parameters.Add(outputParameter);

                    connection.Open();
                    command.ExecuteNonQuery();
                    newId = (int?)outputParameter.Value;
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return newId;
    }


    /// <summary>
    /// Updates an existing record using a stored procedure.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="storedProcedureName">The stored procedure name.</param>
    /// <param name="entity">The entity with updated values.</param>
    /// <returns>True if update was successful, otherwise false.</returns>
    public static bool Update<T>(string storedProcedureName, T entity)
    {
        var rowAffected = 0;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var inputParameters = _CreateInputParameters(entity);
                    _AddInputParametersToSqlCommand(command, inputParameters);

                    connection.Open();
                    rowAffected = command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return (rowAffected > 0);
    }


    /// <summary>
    /// Deletes a record using a stored procedure.
    /// </summary>
    /// <param name="storedProcedureName">The stored procedure name.</param>
    /// <param name="id">The ID of the record to delete.</param>
    /// <returns>True if deletion was successful, otherwise false.</returns>
    public static bool Delete<T>(string storedProcedureName, string parameterName, T parameterValue)
    {
        int rowAffected = 0;
        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue($"@{parameterName}", (object?)parameterValue ?? DBNull.Value);

                    connection.Open();
                    rowAffected = command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return (rowAffected > 0);
    }


    public static bool Delete(string storedProcedureName, (string name, object? value)[]? parameters)
    {
        int rowAffected = 0;
        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters to the command
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue($"@{parameter.name}", parameter.value ?? DBNull.Value);
                        }
                    }

                    connection.Open();
                    rowAffected = command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return (rowAffected > 0);
    }

    /// <summary>
    /// Checks if a record exists using a stored procedure.
    /// </summary>
    /// <param name="storedProcedureName">The stored procedure name.</param>
    /// <param name="id">The ID of the record to check.</param>
    /// <returns>True if the record exists, otherwise false.</returns>
    public static bool Exists<T>(string storedProcedureName, string parameterName, T parameterValue)
    {
        bool isFound = false;
        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue($"@{parameterName}", (object?)parameterValue ?? DBNull.Value);

                    SqlParameter outputParameter = new SqlParameter("@ReturnVal", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };

                    command.Parameters.Add(outputParameter);
                    command.ExecuteNonQuery();

                    isFound = (int)outputParameter.Value == 1;
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return isFound;
    }

    public static bool Exists<T1, T2>(string storedProcedureName, string parameterName1, T1 parameterValue1,
        string parameterName2, T2 parameterValue2)
    {
        bool isFound = false;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue($"@{parameterName1}", (object?)parameterValue1 ?? DBNull.Value);
                    command.Parameters.AddWithValue($"@{parameterName2}", (object?)parameterValue2 ?? DBNull.Value);

                    // @ReturnVal could be any name, and we don't need to add it to the SP, just use it here in the code.
                    SqlParameter returnParameter = new SqlParameter("@ReturnVal", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    command.Parameters.Add(returnParameter);

                    command.ExecuteNonQuery();

                    isFound = (int)returnParameter.Value == 1;
                }
            }
        }
        catch (Exception ex)
        {
            isFound = false;
            HandleException(ex);
        }

        return isFound;
    }

    public static bool UpdateValue<T1, T2>(string storedProcedureName, string parameterName1, T1 parameterValue1,
        string parameterName2, T2 parameterValue2)
    {
        var rowAffected = 0;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue($"@{parameterName1}", (object?)parameterValue1 ?? DBNull.Value);
                    command.Parameters.AddWithValue($"@{parameterName2}", parameterValue2);
                    rowAffected = command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return rowAffected > 0;
    }

    /*
    private static SqlDbType _GetSqlDbType(Type type)
    {
        if (type == typeof(int) || type == typeof(int?)) return SqlDbType.Int;
        if (type == typeof(string)) return SqlDbType.NVarChar;
        if (type == typeof(bool) || type == typeof(bool?)) return SqlDbType.Bit;
        if (type == typeof(DateTime) || type == typeof(DateTime?)) return SqlDbType.DateTime;
        if (type == typeof(decimal) || type == typeof(decimal?)) return SqlDbType.Decimal;
        if (type == typeof(double) || type == typeof(double?)) return SqlDbType.Float;
        if (type == typeof(byte) || type == typeof(byte?)) return SqlDbType.TinyInt;

        throw new ArgumentException($"Unsupported type: {type.FullName}");
    }

    public static T1? GetValue<T1, T2>(string storedProcedureName, string parameterName, T2 parameterValue)
    {
        T1? result = default;
        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue($"@{parameterName}", (object?)parameterValue ?? DBNull.Value);

                    // Determine SqlDbType dynamically
                    SqlDbType sqlDbType = _GetSqlDbType(typeof(T1));

                    SqlParameter outputParam = new SqlParameter("@OutputValue", sqlDbType)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputParam);

                    command.ExecuteNonQuery();

                    object? value = outputParam.Value;
                    result = (value == DBNull.Value) ? default : (T1?)Convert.ChangeType(value, typeof(T1));
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return result;
    }
    */


    public static string? GetStringValue<T>(string storedProcedureName, string parameterName, T parameterValue,
        string outputParameterName, int outputStringLength)
    {
        string? result = null;
        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue($"@{parameterName}", (object?)parameterValue ?? DBNull.Value);

                    SqlParameter outputParam =
                        new SqlParameter($"@{outputParameterName}", SqlDbType.NVarChar, outputStringLength)
                        {
                            Direction = ParameterDirection.Output
                        };
                    command.Parameters.Add(outputParam);

                    command.ExecuteNonQuery();


                    result = outputParam.Value.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return result;
    }
    
    public static int? GetIntValue<T>(string storedProcedureName, string parameterName, T parameterValue,
        string outputParameterName)
    {
        // This function will return the new person id if succeeded and null if not
        int? educationLevelID = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"{storedProcedureName}", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue($"@{parameterName}", parameterValue);

                    SqlParameter outputParam = new SqlParameter($"@{outputParameterName}", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputParam);

                    command.ExecuteNonQuery();

                    educationLevelID = (int?)outputParam.Value;
                }
            }
        }
        catch (Exception ex)
        {
            clsDataAccessHelper.HandleException(ex);
        }

        return educationLevelID;
    }
}