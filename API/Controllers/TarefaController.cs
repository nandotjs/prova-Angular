using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Controllers
{
    [Route("api/tarefa")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly AppDataContext _context;

        public TarefaController(AppDataContext context) =>
            _context = context;

        // GET: api/tarefa/listar
        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            try
            {
                List<Tarefa> tarefas = _context.Tarefas.Include(x => x.Categoria).ToList();
                return Ok(tarefas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/tarefa/cadastrar
        [HttpPost]
        [Route("cadastrar")]
        public IActionResult Cadastrar([FromBody] Tarefa tarefa)
        {
            try
            {
                Categoria? categoria = _context.Categorias.Find(tarefa.CategoriaId);
                if (categoria == null)
                {
                    return NotFound("Categoria não encontrada.");
                }

                tarefa.Categoria = categoria;
                _context.Tarefas.Add(tarefa);
                _context.SaveChanges();

                return Created("", tarefa);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        // GET: api/tarefa/feitas
        [HttpGet]
        [Route("feitas")]
        public IActionResult TarefasFeitas()
        {
            try
            {
                List<Tarefa> tarefasFeitas = _context.Tarefas.Where(x => x.Concluida).ToList();
                return Ok(tarefasFeitas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        // GET: api/tarefa/naofeitas
        [HttpGet]
        [Route("naofeitas")]
        public IActionResult TarefasNaoFeitas()
        {
            try
            {
                List<Tarefa> tarefasNaoFeitas = _context.Tarefas.Where(x => !x.Concluida).ToList();
                return Ok(tarefasNaoFeitas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        // PATCH: api/tarefa/alterar/id
        [HttpPatch]
        [Route("alterar/{id}")]
        public IActionResult Alterar(int id, [FromBody] Tarefa tarefa)
        {
            try
            {
                Tarefa tarefaExistente = _context.Tarefas.Find(id);
                if (tarefaExistente == null)
                {
                    return NotFound("Tarefa não encontrada.");
                }

                tarefaExistente.Titulo = tarefa.Titulo;
                tarefaExistente.Descricao = tarefa.Descricao;
                tarefaExistente.CategoriaId = tarefa.CategoriaId;
                tarefaExistente.Concluida = tarefa.Concluida;  

                _context.SaveChanges();

                return Ok(tarefaExistente);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
