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
        public async Task<ActionResult<IEnumerable<TipoProduto>>> GetAllTipoProduto()
        {
            // Usa ToListAsync para buscar todos os tipos de produtos de forma assíncrona
            var tipos = await _context.TiposProduto.ToListAsync();

            if (tipos is null)
            {
                return NotFound("Tipos não encontrados");
            }
            return Ok(tipos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TipoProduto>> GetTipoProdutoById(int id)
        {
            // Usa FirstOrDefaultAsync para buscar o tipo de produto pelo ID de forma assíncrona
            var tipo = await _context.TiposProduto.FirstOrDefaultAsync(t => t.IdTipo == id);

            if (tipo is null)
            {
                return NotFound($"O tipo com o id \"{id}\" não encontrado na base de dados");
            }
            return Ok(tipo);
        }

        [HttpPost]
        public async Task<ActionResult> AddNewTipoProduto(TipoProduto tipo)
        {
            // Verifica se o tipo já existe usando o método assíncrono TipoExisteAsync
            if (await TipoExisteAsync(tipo))
            {
                return BadRequest("Tipo de produto já existe com o mesmo ID ou Nome");
            }

            _context.TiposProduto.Add(tipo);
            // Salva o novo tipo de produto de forma assíncrona
            await _context.SaveChangesAsync();

            return Ok("Novo tipo adicionado");
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTipoProduto(int id, TipoProduto tipo)
        {
            // Verifica se o tipo existe usando o método assíncrono TipoExisteAsync
            if (!await TipoExisteAsync(id))
            {
               return NotFound("Tipo não encontrado"); 
            }

            _context.TiposProduto.Update(tipo);
            // Salva as alterações de forma assíncrona
            await _context.SaveChangesAsync();
            
            return Ok("Alterações gravadas"); 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            // Busca o tipo de produto pelo ID usando FindAsync
            var tipo = await _context.TiposProduto.FindAsync(id);

            if (tipo is null)
            {
                return NotFound("Tipo não encontrado");
            }

            _context.TiposProduto.Remove(tipo);
            // Salva as mudanças de forma assíncrona após a remoção
            await _context.SaveChangesAsync();

            return Ok("Tipo removido com sucesso");
        }

        // Método privado assíncrono para verificar se o TipoProduto existe com base no ID ou Nome
        private async Task<bool> TipoExisteAsync(TipoProduto tipo)
        {
            return await _context.TiposProduto.AnyAsync(t => t.IdTipo == tipo.IdTipo || t.NomeTipo == tipo.NomeTipo);
        }

        // Método privado assíncrono para verificar se o TipoProduto existe com base apenas no ID
        private async Task<bool> TipoExisteAsync(int id)
        {
            return await _context.TiposProduto.AnyAsync(t => t.IdTipo == id);
        }
    }
}