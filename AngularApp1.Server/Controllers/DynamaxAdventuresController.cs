using AngularApp1.Server.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace AngularApp1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class DynamaxAdventuresController : ControllerBase
    {
        private readonly ILogger<DynamaxAdventuresController> _logger;

        public DynamaxAdventuresController(ILogger<DynamaxAdventuresController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetDynamaxAdventures")]
        public List<DynamaxAdventure> Get(string type)
        {
            using (SqlConnection connection = new SqlConnection("Server=DESKTOP-1O2PNC2;Database=DA;User Id=test2;Password=aaa;TrustServerCertificate=True;"))
            {
                String sql = $"SELECT PName, VersionEx, PrimaryType, SecondaryType FROM Pokemon " +
                    $"WHERE PrimaryType = '{type}' OR SecondaryType = '{type}'" +
                    $"ORDER BY PxID;";
                List<DynamaxAdventure> pList = new List<DynamaxAdventure>();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                pList.Add(new DynamaxAdventure
                                {
                                    PName = reader.GetString(0),
                                    VersionEx = reader.GetString(1),
                                    PrimaryType = reader.GetString(2),
                                    SecondaryType = reader.GetString(3)
                                });
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.ToString());
                                pList.Add(new DynamaxAdventure
                                {
                                    PName = reader.GetString(0),
                                    VersionEx = reader.GetString(1),
                                    PrimaryType = reader.GetString(2)
                                });
                            }
                        }
                        return pList;
                    }
                }
            }
        }
    }
}
