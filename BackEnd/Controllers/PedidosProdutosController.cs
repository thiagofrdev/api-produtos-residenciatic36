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
    public class PedidosProdutosController : ControllerBase
    {
        private readonly ApiContext _context;

        //Construtor da classe PedidoProdutoController
        public PedidosProdutosController(ApiContext context)
        {
            _context = context;
        }

        //Rota para pegar todos os PedidosProdutos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoProduto>>> GetAllPedidosProdutos()
        {
            // Usa ToListAsync para buscar todos os dados da tabela de forma assíncrona
            var pedidosProdutos = await _context.PedidosProdutos.ToListAsync();

            //Verifica se a lista está vazia
            if (pedidosProdutos is null)
            {
                return NotFound("Nada encontrado");
            }
            return Ok(pedidosProdutos);
        }

        //Rota para pegar um PedidoProduto pelo seu ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PedidoProduto>> GetPedidosProdutosById(int id)
        {
            // Usa FirstOrDefaultAsync para buscar o PedidoProduto pelo ID de forma assíncrona
            var pedidoProduto = await _context.PedidosProdutos.FirstOrDefaultAsync(pp => pp.IdPedidoProduto == id);

            //Verifica se a variável está vazia
            if (pedidoProduto is null)
            {
                return NotFound("Nada encontrado");
            }
            return new CreatedAtRouteResult("Produto capturado", new { id = pedidoProduto.IdProduto}, pedidoProduto);
        }

        //Rota para adicionar um novo PedidoProduto
        [HttpPost]
        public async Task<ActionResult<PedidoProduto>> AddNewPedidoProduto(int id, PedidoProduto pedidoProduto)
        {
            // Usa o método assíncrono PedidoProdutoExisteAsync para verificar a existência do PedidoProduto
            if (await PedidoProdutoExisteAsync(id))
            {
                return BadRequest("Essa relação pedido/produto já existe com o mesmo ID");
            }

            _context.PedidosProdutos.Add(pedidoProduto);
            // Usa SaveChangesAsync para salvar a nova entrada de forma assíncrona
            await _context.SaveChangesAsync();

            return Ok("Novo produto criado");
        }

        //Método para atualizar um PedidoProduto existente
        [HttpPut("{id:int}")]
        public async Task<ActionResult<PedidoProduto>> UpdatePedidoProduto(int id, PedidoProduto pedidoProduto)
        {
            // Usa o método assíncrono PedidoProdutoExisteAsync para verificar a existência do PedidoProduto
            if (!await PedidoProdutoExisteAsync(id))
            {
                return NotFound("Nada encontrado");
            }

            _context.PedidosProdutos.Update(pedidoProduto);
            // Usa SaveChangesAsync para salvar as alterações de forma assíncrona
            await _context.SaveChangesAsync();
            
            return Ok("Alterações gravadas");        
        }

        //Método para deletar um PedidoProduto
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePedidoProduto(int id)
        {
            // Usa FindAsync para buscar o PedidoProduto pelo ID de forma assíncrona
            var pedidoProduto = await _context.PedidosProdutos.FindAsync(id);

            if (pedidoProduto is null)
            {
                return NotFound("Nada encontrado");
            }

            _context.PedidosProdutos.Remove(pedidoProduto);
            // Usa SaveChangesAsync para deletar a entrada de forma assíncrona
            await _context.SaveChangesAsync();

            return Ok("Relação removida com sucesso");
        }

        // Método privado assíncrono para verificar se o PedidoProduto existe no banco de dados com base no ID
        private async Task<bool> PedidoProdutoExisteAsync(int id)
        {
            return await _context.PedidosProdutos.AnyAsync(pp => pp.IdPedidoProduto == id);
        }
    }
}