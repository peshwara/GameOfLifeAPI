using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System;
using Newtonsoft.Json;

namespace GameOfLifeAPI.Models
{
    public class Board
    {
        public int Id { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public string State { get; set; }

        // Deserialize the state into an int[,] array
        [JsonIgnore]  // Ignore the property during serialization
        [NotMapped]   // This will ensure that EF ignores this property during database operations
        public int[,] StateArray
        {
            get
            {
                return JsonConvert.DeserializeObject<int[,]>(State);
            }
            set
            {
                // Serialize the int[,] array into a JSON string before storing it
                State = JsonConvert.SerializeObject(value);
            }
        }
    }
}
