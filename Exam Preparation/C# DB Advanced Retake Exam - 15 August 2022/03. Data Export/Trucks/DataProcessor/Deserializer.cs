namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using Trucks.Data.Models;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            ImportDespatcherDTO[] ImportDespatcherDTOs = XmlHelper.Deserialize<ImportDespatcherDTO[]>(xmlString, "Despatchers");

            List<Despatcher> despatchers = new List<Despatcher>();
            List<Truck> trucks = new List<Truck>();

            foreach (ImportDespatcherDTO DespatcherDTO in ImportDespatcherDTOs)
            {
                if (!IsValid(DespatcherDTO))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Despatcher despatcher = new Despatcher()
                {
                    Name = DespatcherDTO.Name,
                    Position = DespatcherDTO.Position
                };
               

                foreach (ImportTruckDTO truckDTO in DespatcherDTO.Trucks)
                {
                    if (!IsValid(truckDTO))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    Truck truck = new Truck()
                    {
                        RegistrationNumber = truckDTO.RegistrationNumber,
                        VinNumber = truckDTO.VinNumber,
                        TankCapacity = truckDTO.TankCapacity,
                        CargoCapacity = truckDTO.CargoCapacity,
                        CategoryType = truckDTO.CategoryType,
                        MakeType = truckDTO.MakeType
                    };

                    trucks.Add(truck);
                    despatcher.Trucks.Add(truck);
                }

                despatchers.Add(despatcher);

                output.AppendLine(String.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count));
            }

            context.Trucks.AddRange(trucks);
            context.Despatchers.AddRange(despatchers);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();
            ImportClientDTO[] clientDTOs = JsonConvert.DeserializeObject<ImportClientDTO[]>(jsonString)!;

            List<Client> clients = new List<Client>();
            List<ClientTruck> clientTrucks = new List<ClientTruck>();

            foreach (ImportClientDTO clientDTO in clientDTOs)
            {
                if (!IsValid(clientDTO) || clientDTO.Type == "usual")
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client()
                {
                    Name = clientDTO.Name,
                    Nationality = clientDTO.Nationality,
                    Type = clientDTO.Type
                };

                foreach (int truckId in clientDTO.Trucks.Distinct())
                {
                    Truck truck = context.Trucks.FirstOrDefault(t => t.Id == truckId)!;

                    if (truck == null)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    ClientTruck clientTruck = new ClientTruck() { Truck = truck };
                    clientTrucks.Add(clientTruck);
                    client.ClientsTrucks.Add(clientTruck);
                }

                clients.Add(client);
                output.AppendLine(String.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count));
            }

            context.ClientsTrucks.AddRange(clientTrucks);
            context.Clients.AddRange(clients);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}