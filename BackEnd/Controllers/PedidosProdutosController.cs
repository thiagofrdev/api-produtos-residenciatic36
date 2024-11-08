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
        public async Task<ActionResult<RespostaRequisicao<PedidoProduto>>> GetPedidosProdutosById(int id)
        {
            // Usa FirstOrDefaultAsync para buscar o PedidoProduto pelo ID de forma assíncrona
            var pedidoProduto = await _context.PedidosProdutos.FirstOrDefaultAsync(pp => pp.IdPedidoProduto == id);

            //Verifica se a variável está vazia
            if (pedidoProduto is null)
            {
                return NotFound(new RespostaRequisicao<PedidoProduto>($"A relação Pedido/Produto com o id \"{id}\" não foi encontrada na base de dados", null));
            }
            return Ok(new RespostaRequisicao<PedidoProduto>($"A relação Pedido/Produto {id}  foi encontrada com sucesso", pedidoProduto));
        }

        //Rota para adicionar um novo PedidoProduto
        [HttpPost]
        public async Task<ActionResult<RespostaRequisicao<PedidoProduto>>> AddNewPedidoProduto(int id, PedidoProduto pedidoProduto)
        {
            // Usa o método assíncrono PedidoProdutoExisteAsync para verificar a existência do PedidoProduto
            if (await PedidoProdutoExisteAsync(id))
            {
                return BadRequest(new RespostaRequisicao<PedidoProduto>("Essa relação pedido/produto já existe com o mesmo ID", null));
            }

            _context.PedidosProdutos.Add(pedidoProduto);
            // Usa SaveChangesAsync para salvar a nova entrada de forma assíncrona
            await _context.SaveChangesAsync();

            return Ok(new RespostaRequisicao<PedidoProduto>("Nova relação Pedido/Produto adicionada com sucesso!", pedidoProduto));
        }

        //Método para atualizar um PedidoProduto existente
        [HttpPut("{id:int}")]
        public async Task<ActionResult<RespostaRequisicao<PedidoProduto>>> UpdatePedidoProduto(int id, PedidoProduto pedidoProduto)
        {
            // Usa o método assíncrono PedidoProdutoExisteAsync para verificar a existência do PedidoProduto
            if (!await PedidoProdutoExisteAsync(id))
            {
                return NotFound(new RespostaRequisicao<PedidoProduto>($"Relação Pedido/Produto com ID {id} não foi encontrada", null));
            }

            _context.PedidosProdutos.Update(pedidoProduto);
            // Usa SaveChangesAsync para salvar as alterações de forma assíncrona
            await _context.SaveChangesAsync();
            
            return Ok(new RespostaRequisicao<PedidoProduto>($"Alterações na relação Pedido/Produto {id} foram gravadas com sucesso!", pedidoProduto));         
        }

        //Método para deletar um PedidoProduto
        [HttpDelete("{id}")]
        public async Task<ActionResult<RespostaRequisicao<PedidoProduto>>> DeletePedidoProduto(int id)
        {
            // Usa FindAsync para buscar o PedidoProduto pelo ID de forma assíncrona
            var pedidoProduto = await _context.PedidosProdutos.FindAsync(id);

            if (pedidoProduto is null)
            {
                 return NotFound(new RespostaRequisicao<PedidoProduto>($"A rela~]ap Pedido/Produto com ID {id} não foi encontrada", null));
            }

            _context.PedidosProdutos.Remove(pedidoProduto);
            // Usa SaveChangesAsync para deletar a entrada de forma assíncrona
            await _context.SaveChangesAsync();

            return Ok(new RespostaRequisicao<PedidoProduto>($"Relação Pedido/Produto {id} removido com sucesso", null));
        }

        // Método privado assíncrono para verificar se o PedidoProduto existe no banco de dados com base no ID
        private async Task<bool> PedidoProdutoExisteAsync(int id)
        {
            return await _context.PedidosProdutos.AnyAsync(pp => pp.IdPedidoProduto == id);
        }

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