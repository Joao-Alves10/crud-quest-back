using System.ComponentModel.DataAnnotations;

namespace QuestAPI.Entities
{
    public class Alternativa
    {
        [Key()]
        public long? Id { get; set; }
        public string AlternativaCorreta { get; set; }
        public string AlternativaErrada1 { get; set; }
        public string AlternativaErrada2 { get; set; }
        public string AlternativaErrada3 { get; set; }
    }
}
