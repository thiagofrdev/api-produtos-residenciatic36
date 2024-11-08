using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Context;
using BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposProdutoController : ControllerBase
    {
        private readonly ApiContext _context;   
        public TiposProdutoController(ApiContext context){
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TipoProduto>> GetAllTipoProduto()
        {
            var tipos = _context.TiposProduto.ToList();

            if (tipos is null)
            {
                return NotFound("Tipos não encontrados");
            }
            return Ok(tipos);
        }

        [HttpGet("{id:int}")]
        public ActionResult<IEnumerable<TipoProduto>> GetTipoProdutoById(int id)
        {
            var tipo = _context.TiposProduto.FirstOrDefault(t => t.IdTipo == id);

            if (tipo is null)
            {
                return NotFound($"O tipo com o id \"{id}\" não encontrado na base de dados");
            }
            return Ok(tipo);
        }

        [HttpPost]
        public ActionResult<IEnumerable<TipoProduto>> AddNewTipoProduto(TipoProduto tipo)
        {
            if (TipoExiste(tipo))
            {
                return BadRequest("Tipo de produto já existe com o mesmo ID ou Nome");
            }

            _context.TiposProduto.Add(tipo);
            _context.SaveChanges();

            return Ok("Novo tipo adicionado");
        }

        [HttpPut]
        public ActionResult<IEnumerable<TipoProduto>> UpdateTipoProduto(int id, TipoProduto tipo)
        {
            if (!TipoExiste(id))
            {
               return NotFound("Tipo não encontrado"); 
            }

            _context.TiposProduto.Update(tipo);
            _context.SaveChanges();
            
            return Ok("Alterações gravadas"); 
        }

        [HttpDelete("{id}")]
        public ActionResult<IEnumerable<TipoProduto>> DeleteProduct(int id, TipoProduto tipo)
        {
            if (!TipoExiste(id))
            {
                return NotFound("Tipo não encontrado");
            }

            _context.TiposProduto.Remove(tipo);
            _context.SaveChanges();

            return Ok("Tipo removido com sucesso");
        }

        private bool TipoExiste(TipoProduto tipo)
        {
            return _context.TiposProduto.Any(t => t.IdTipo == tipo.IdTipo || t.NomeTipo == tipo.NomeTipo);
        }

        private bool TipoExiste(int id)
        {
            return _context.TiposProduto.Any(t => t.IdTipo != id);
        }
    }
}