using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("perguntas/{tema}")]
        public IActionResult GetPerguntasByTema(string tema)
        {
            try
            {
                //busca pergunta no banco pelo tema, e retorna uma lista com as perguntas relacionadas ao tema
                var perguntas = _context.Pergunta
                                .Include(x => x.Tema)
                                .Where(x => x.Tema.DescTema == tema)
                                .Where(x => x.TemaId == x.Tema.Id)
                                .Select(x => x.DescPergunta)
                                .ToList();

                return Ok(perguntas);
                
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("perguntas/alternativas")]
        public IActionResult GetAlternativasByTema(string tema)
        {
            try
            {
                //pesquisa no banco pelo tema e retorna um objeto com as alternativas referentes

                var alternativas = _context.Pergunta
                                   .Include(x => x.Alternativa)
                                   .Where(x => x.Tema.DescTema == tema)
                                   .Where(x => x.AlternativaId == x.Alternativa.Id)
                                   .Select(x => new { Alternativas = x.Alternativa.AlternativaErrada1, x.Alternativa.AlternativaErrada2, x.Alternativa.AlternativaErrada3, x.Alternativa.AlternativaCorreta})
                                   .ToList();
                return Ok(alternativas);
                                   
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("perguntas")]
        public IActionResult PostPerguntas([FromBody]Pergunta dadoPergunta)
        {
            try
            {
                Pergunta pergunta = new Pergunta();
                Alternativa alternativa = new Alternativa();
                Tema tema = new Tema();
                
                if (dadoPergunta == null)
                {
                    throw new Exception("Não foi possível cadastrar os dados da pergunta.");
                }


                if (dadoPergunta.Alternativa != null)
                {
                    //atribui valores recebidos pelo parâmetro às propriedades da entidade Alternativa

                    alternativa = new Alternativa
                    {
                       AlternativaCorreta = dadoPergunta.Alternativa.AlternativaCorreta,
                       AlternativaErrada1 = dadoPergunta.Alternativa.AlternativaErrada1,
                       AlternativaErrada2 = dadoPergunta.Alternativa.AlternativaErrada2,
                       AlternativaErrada3 = dadoPergunta.Alternativa.AlternativaErrada3
                    };
                    _context.Alternativa.Add(alternativa);
                    _context.SaveChanges();
                }

                //busca o id da alternativa na sua respectiva tabela e armazena numa variável, onde será usada para salvar na coluna Alternativa na tab. de Pergunta
                var alternativaId = _context.Alternativa
                                    .Where(x => x.AlternativaCorreta == dadoPergunta.Alternativa.AlternativaCorreta)
                                    .Select(x => x.Id)
                                    .FirstOrDefault();

                if (dadoPergunta.Tema != null)
                {
                    tema = new Tema
                    {
                        DescTema = dadoPergunta.Tema.DescTema
                    };
                    
                    _context.Tema.Add(tema);
                    _context.SaveChanges();
                }
                //busca o id do tema na sua respectiva tabela e armazena numa variável, onde será usada para salvar na coluna TemaId na tab. de Pergunta

                var temaId = _context.Tema
                            .Where(x => x.DescTema == dadoPergunta.Tema.DescTema)
                            .Select(x => x.Id)
                            .FirstOrDefault();

                if (dadoPergunta != null)
                {
                    pergunta = new Pergunta
                    {
                        DescPergunta = dadoPergunta.DescPergunta,
                        TemaId = temaId,
                        AlternativaId = alternativaId
                    };

                    _context.Pergunta.Add(pergunta);
                    _context.SaveChanges();
                }
             
                return Ok(dadoPergunta);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("atualiza-perguntas")]
        public IActionResult PutPerguntas([FromBody] Pergunta pergunta)
        {
            try
            {
                if (pergunta.Id != null)
                {
                    //verifica se existe a pergunta no banco
                    var perguntaBanco = _context.Pergunta
                                   .Where(x => x.Id == pergunta.Id)
                                   .First();
                    //se existir atualiza com o dado enviado pelo parametro
                    if (perguntaBanco != null)
                    {
                        perguntaBanco.DescPergunta = pergunta.DescPergunta;

                        _context.Pergunta.Update(perguntaBanco);
                        _context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Não foi possível atualizar a pergunta.");

                    }
                }
               
                if (pergunta.Alternativa.Id != null)
                {
                    var alternativaBanco = _context.Alternativa
                                   .Where(x => x.Id == pergunta.Alternativa.Id)
                                   .First();

                    if (alternativaBanco != null)
                    {
                        alternativaBanco.AlternativaErrada1 = pergunta.Alternativa.AlternativaErrada1;
                        alternativaBanco.AlternativaErrada2 = pergunta.Alternativa.AlternativaErrada2;
                        alternativaBanco.AlternativaErrada3 = pergunta.Alternativa.AlternativaErrada3;
                        alternativaBanco.AlternativaCorreta = pergunta.Alternativa.AlternativaCorreta;

                        _context.Alternativa.Update(alternativaBanco);
                        _context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Não foi possível atualizar a alternativa.");

                    }
                }

                if (pergunta.Tema.Id != null)
                {
                    var temaBanco = _context.Tema
                                    .Where(x => x.Id == pergunta.Tema.Id)
                                    .First();

                    if (temaBanco != null)
                    {
                        temaBanco.DescTema = pergunta.Tema.DescTema;

                        _context.Tema.Update(temaBanco);
                        _context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Não foi possível atualizar o tema.");

                    }
                }

                return Created("quest/perguntas", pergunta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("pergunta/{id}")]
        public IActionResult deletePerguntaByTipoEId (long? id)
        {
            try
            {
                // verifica se existe a oergunta no banco
                var perguntaBanco = _context.Pergunta
                               .Where(x => x.Id == id)
                               .First();
                // se existir, apaga
                if (perguntaBanco != null)
                {
                    _context.Pergunta.Remove(perguntaBanco);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Não foi possível remover a pergunta.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("alternativa/{id}")]
        public IActionResult deleteAlternativaByTipoEId(long? id)
        {
            try
            {
                var alternativaBanco = _context.Alternativa
                               .Where(x => x.Id == id)
                               .First();
                if (alternativaBanco != null)
                {
                    _context.Alternativa.Remove(alternativaBanco);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Não foi possível remover a alternativa.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("tema/{id}")]
        public IActionResult deleteTemaByTipoEId(long? id)
        {
            try
            {
                var temaBanco = _context.Tema
                               .Where(x => x.Id == id)
                               .First();
                if (temaBanco != null)
                {
                    _context.Tema.Remove(temaBanco);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Não foi possível remover o tema.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
