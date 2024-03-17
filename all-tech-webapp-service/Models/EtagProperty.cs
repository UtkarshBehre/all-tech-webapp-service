using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace all_tech_webapp_service.Models
{
    public class EtagProperty
    {
        [Required]
        [JsonProperty("_etag")]
        public string Etag { get; set; }
    }
}
