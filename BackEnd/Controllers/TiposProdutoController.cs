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
        public async Task<ActionResult<RespostaRequisicao<TipoProduto>>> GetTipoProdutoById(int id)
        {
            // Usa FirstOrDefaultAsync para buscar o tipo de produto pelo ID de forma assíncrona
            var tipo = await _context.TiposProduto.FirstOrDefaultAsync(t => t.IdTipo == id);

            if (tipo is null)
            {
                return NotFound(new RespostaRequisicao<TipoProduto>($"O tipo com o id \"{id}\" não foi encontrado na base de dados", null));
            }
            return Ok(new RespostaRequisicao<TipoProduto>("Tipo encontrado com sucesso", tipo));
        }

        [HttpPost]
        public async Task<ActionResult<RespostaRequisicao<TipoProduto>>> AddNewTipoProduto(TipoProduto tipo)
        {
            // Verifica se o tipo já existe usando o método assíncrono TipoExisteAsync
            if (await TipoExisteAsync(tipo))
            {
                return BadRequest(new RespostaRequisicao<TipoProduto>("Tipo de produto já existe com o mesmo ID ou Nome", null));
            }

            _context.TiposProduto.Add(tipo);
            // Salva o novo tipo de produto de forma assíncrona
            await _context.SaveChangesAsync();

            return Ok(new RespostaRequisicao<TipoProduto>("Novo tipo adicionado com sucesso!", tipo));
        }

        [HttpPut]
        public async Task<ActionResult<RespostaRequisicao<TipoProduto>>> UpdateTipoProduto(int id, TipoProduto tipo)
        {
            // Verifica se o tipo existe usando o método assíncrono TipoExisteAsync
            if (!await TipoExisteAsync(id))
            {
               return NotFound(new RespostaRequisicao<TipoProduto>("Tipo não encontrado", null));
            }

            _context.TiposProduto.Update(tipo);
            // Salva as alterações de forma assíncrona
            await _context.SaveChangesAsync();
            
            return Ok(new RespostaRequisicao<TipoProduto>("Alterações gravadas com sucesso!", tipo));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<RespostaRequisicao<TipoProduto>>> DeleteProduct(int id)
        {
            // Busca o tipo de produto pelo ID usando FindAsync
            var tipo = await _context.TiposProduto.FindAsync(id);

            if (tipo is null)
            {
                return NotFound(new RespostaRequisicao<TipoProduto>("Tipo não encontrado", null));
            }

            _context.TiposProduto.Remove(tipo);
            // Salva as mudanças de forma assíncrona após a remoção
            await _context.SaveChangesAsync();

            return Ok(new RespostaRequisicao<TipoProduto>("Tipo removido com sucesso", null));
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

        //Classe auxiliar para definir a estrutura de resposta
        public class RespostaRequisicao<R>
        {
            public string Message { get; set; }
            public R Data { get; set; }

            public RespostaRequisicao(string message, R data)
            {
                Message = message;
                Data = data;
            }
        }
    }
}