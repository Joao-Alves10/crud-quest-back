using System.ComponentModel.DataAnnotations;

namespace QuestAPI.Entities
{
    public class Tema
    {
        [Key()]
        public long? Id { get; set; }
        public string DescTema { get; set; }
    }
}
