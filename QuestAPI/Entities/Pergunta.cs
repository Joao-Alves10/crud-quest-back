using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestAPI.Entities
{
    public class Pergunta
    {
        [Key()]
        public long? Id { get; set; }
        public string DescPergunta { get; set; }

        [ForeignKey("TemaPergunta")]
        public  long? TemaPerguntaId { get; set; }
        public virtual Tema Tema { get; set; }

        [ForeignKey("Alternativa")]
        public long? AlternativaId { get; set; }
        public virtual Alternativa Alternativa { get; set; }

    }
}
