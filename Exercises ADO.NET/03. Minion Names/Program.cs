using Microsoft.Data.SqlClient;



string connectionString =
                        "Data Source=(local); Database=MinionsDB; Integrated Security=true; TrustServerCertificate=True";

string queryString =
                    @"SELECT Name FROM Villains WHERE Id = @Id

                     SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";

int paramValue = 1;

using (SqlConnection connection =
    new SqlConnection(connectionString))
{
    SqlCommand command = new SqlCommand(queryString, connection);
    command.Parameters.AddWithValue("@Id", paramValue);

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