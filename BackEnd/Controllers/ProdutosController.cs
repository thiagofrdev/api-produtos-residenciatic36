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
    public class ProdutosController : ControllerBase
    {
        private readonly ApiContext _context;

        //Construtor da classe ProdutosController
        public ProdutosController(ApiContext context)
        {
            _context = context;
        }

        //Rota para pegar todos os produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAllProducts()
        {
            //Usando ToListAsync para buscar todos os produtos de forma assíncrona
            var produtos = await _context.Produtos.ToListAsync();

            //Verifica se a lista está vazia
            if (produtos is null)
            {
                return NotFound("Produtos não encontrados");
            }
            return Ok(produtos);
        }

        //Rota para pegar um produto pelo seu ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<RespostaRequisicao<Produto>>> GetProductById(int id)
        {
            //Usando FirstOrDefaultAsync para buscar o produto pelo ID de forma assíncrona
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.IdProduto == id);

            //Verifica se a variável está vazia
            if (produto is null)
            {
                return NotFound(new RespostaRequisicao<Produto>($"O produto com o id '{id}' não foi encontrado na base de dados", null));
            }
            return Ok(new RespostaRequisicao<Produto>($"Produto {id} encontrado com sucesso", produto));
        }

        //Rota para adicionar um novo produto
        [HttpPost]
        public async Task<ActionResult<RespostaRequisicao<Produto>>> AddNewProduct(Produto produto)
        {
            //Usando o método assíncrono ProdutoExisteAsync para verificar a existência do produto
            if (await ProdutoExisteAsync(produto))
            {
                return BadRequest(new RespostaRequisicao<Produto>("Esse produto já existe com o mesmo ID ou Nome", null));
            }

            _context.Produtos.Add(produto);
            // Usa SaveChangesAsync para salvar o novo produto de forma assíncrona
            await _context.SaveChangesAsync();

            return Ok(new RespostaRequisicao<Produto>("Novo produto adicionado com sucesso!", produto));
        }

        //Método para atualizar um produto existente
        [HttpPut("{id:int}")]
        public async Task<ActionResult<RespostaRequisicao<Produto>>> UpdateProduct(int id, Produto produto)
        {
            // Usa o método assíncrono ProdutoExisteAsync para verificar a existência do produto
            if (!await ProdutoExisteAsync(id))
            {
                return NotFound(new RespostaRequisicao<Produto>($"Produto com ID {id} não encontrado", null));
            }

            _context.Produtos.Update(produto);
            // Usa SaveChangesAsync para salvar as alterações de forma assíncrona
            await _context.SaveChangesAsync();
            
            return Ok(new RespostaRequisicao<Produto>($"Alterações no produto {id} gravadas com sucesso!", produto));        
        }

        //Método para deletar um produto
        [HttpDelete("{id}")]
        public async Task<ActionResult<RespostaRequisicao<Produto>>> DeleteProduct(int id)
        {
            // Usa FindAsync para buscar o produto pelo ID de forma assíncrona
            var produto = await _context.Produtos.FindAsync(id);

            if (produto is null)
            {
                return NotFound(new RespostaRequisicao<Produto>($"Produto com ID {id} não encontrado", null));
            }

            _context.Produtos.Remove(produto);
            // Usa SaveChangesAsync para deletar o produto de forma assíncrona
            await _context.SaveChangesAsync();

            return Ok(new RespostaRequisicao<Produto>($"Produto {id} removido com sucesso", null));
        }

        // Método privado assíncrono para verificar se o Produto existe no banco de dados com base no ID ou Nome
        private async Task<bool> ProdutoExisteAsync(Produto produto)
        {
            return await _context.Produtos.AnyAsync(t => t.IdProduto == produto.IdProduto || t.NomeProduto == produto.NomeProduto);
        }

        // Método privado assíncrono para verificar se o Produto existe no banco de dados com base apenas no ID
        private async Task<bool> ProdutoExisteAsync(int id)
        {
            return await _context.Produtos.AnyAsync(t => t.IdProduto == id);
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