using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataBaseContext.Entidades
{
    public class Person
    {
        [JsonIgnore]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public void UpdateProperties(Person nova_pessoa)
        {
            foreach (var property in typeof(Person).GetProperties())
            {
                if (property.Name.Contains("Id"))
                    continue;
                if (!object.Equals(property.GetValue(this), property.GetValue(nova_pessoa)))
                    property.SetValue(this, property.GetValue(nova_pessoa));
            }
        }
    }
}
