using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuestAPI.Contexts;
using QuestAPI.Entities;

namespace QuestAPI.Controllers
{
    [Route("quest")]
    public class QuestController : ControllerBase
    {
        private readonly QuestContext _context;
        public QuestController(QuestContext context)
        {
            _context = context;
        }

        public class PerguntaResponse {
            public string DescPergunta { get; set; }
            public string AlternativaErrada1 { get; set; }
            public string AlternativaErrada2 { get; set; }
            public string AlternativaErrada3 { get; set; }
            public string AlternativaCorreta { get; set; }
            public string TemaPergunta { get; set; }
        }

        [HttpPost("perguntas")]
        public IActionResult PostPerguntas([FromBody]PerguntaResponse perguntaResponse)
        {
            try
            {
                var descPergunta = new SqlParameter("DescPergunta", perguntaResponse.DescPergunta);
                var alternativaErrada1 = new SqlParameter("AlternativaErrada1", perguntaResponse.AlternativaErrada1);
                var alternativaErrada2 = new SqlParameter("AlternativaErrada2", perguntaResponse.AlternativaErrada2);
                var alternativaErrada3 = new SqlParameter("AlternativaErrada3", perguntaResponse.AlternativaErrada3);
                var alternativaCorreta = new SqlParameter("AlternativaCorreta", perguntaResponse.AlternativaCorreta);

                PerguntaResponse pergunta = new PerguntaResponse();

                if (perguntaResponse == null)
                {
                    throw new Exception("Não foi possível cadastrar a pergunta");
                }

                var alternativas = _context.Alternativa
                                        .FromSqlRaw("INSERT INTO Alternativa VALUES(@AlternativaCorreta, @AlternativaErrada1, @AlternativaErrada2, @AlternativaErrada3 )", alternativaCorreta, alternativaErrada1, alternativaErrada2, alternativaErrada3);
                                        _context.SaveChanges();

                                       
                var perguntaDesc = _context.Pergunta
                                   .FromSqlRaw("INSERT INTO Pergunta (DescPergunta) values(@DescPergunta)", descPergunta);
                                   _context.SaveChanges();
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
