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

        public class Response
        {
            public Tema Tema { get; set; }
            public Pergunta Pergunta { get; set; }
            public List<Alternativa> Alternativa { get; set; }
        }

        [HttpGet]
        public IActionResult GetPerguntas()
        {
            try
            {
                var response = new Response();

                var alternativas = _context.Alternativa
                                    .Select(x => new {x.AlternativaErrada1, x.AlternativaErrada2, x.AlternativaErrada3, x.AlternativaCorreta })
                                    .ToList();

                return Ok(alternativas);


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
          
        }
        [HttpPost("perguntas")]
        public IActionResult PostPerguntas([FromBody]Pergunta perguntaResponse)
        {
            try
            {
                Pergunta pergunta = new Pergunta();
                Alternativa alternativa = new Alternativa();
                Tema tema = new Tema();
                
                if (perguntaResponse == null)
                {
                    throw new Exception("Não foi possível cadastrar os dados da pergunta.");
                }


                if (perguntaResponse.Alternativa != null)
                {
                    alternativa = new Alternativa
                    {
                       AlternativaCorreta = perguntaResponse.Alternativa.AlternativaCorreta,
                       AlternativaErrada1 = perguntaResponse.Alternativa.AlternativaErrada1,
                       AlternativaErrada2 = perguntaResponse.Alternativa.AlternativaErrada2,
                       AlternativaErrada3 = perguntaResponse.Alternativa.AlternativaErrada3
                    };
                    _context.Alternativa.Add(alternativa);
                    _context.SaveChanges();
                }

                var alternativaId = _context.Alternativa
                                    .Where(x => x.AlternativaCorreta == perguntaResponse.Alternativa.AlternativaCorreta)
                                    .Select(x => x.Id)
                                    .FirstOrDefault();

                if (perguntaResponse.Tema != null)
                {
                    tema = new Tema
                    {
                        DescTema = perguntaResponse.Tema.DescTema
                    };
                    
                    _context.Tema.Add(tema);
                    _context.SaveChanges();
                }

                var temaId = _context.Tema
                            .Where(x => x.DescTema == perguntaResponse.Tema.DescTema)
                            .Select(x => x.Id)
                            .FirstOrDefault();

                if (perguntaResponse != null)
                {
                    pergunta = new Pergunta
                    {
                        DescPergunta = perguntaResponse.DescPergunta,
                        TemaId = temaId,
                        AlternativaId = alternativaId
                    };

                    _context.Pergunta.Add(pergunta);
                    _context.SaveChanges();
                }
                
                return Ok(perguntaResponse);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
