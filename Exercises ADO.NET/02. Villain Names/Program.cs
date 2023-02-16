using Microsoft.Data.SqlClient;



string connectionString =
                        "Data Source=(local); Database=MinionsDB; Integrated Security=true; TrustServerCertificate=True";

string queryString =
                    @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                        FROM Villains AS v 
                        JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                    GROUP BY v.Id, v.Name 
                      HAVING COUNT(mv.VillainId) > @paramValue
                    ORDER BY COUNT(mv.VillainId)";

int paramValue = 3;

using (SqlConnection connection =
    new SqlConnection(connectionString))
{
    SqlCommand command = new SqlCommand(queryString, connection);
    command.Parameters.AddWithValue("@paramValue", paramValue);

    try
    {
        connection.Open();
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine($"{reader["Name"]} - {reader["MinionsCount"]}");
        }
        reader.Close();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    Console.ReadLine();


}   