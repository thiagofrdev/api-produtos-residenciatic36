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
        public async Task<ActionResult<Produto>> GetProductById(int id)
        {
            //Usando FirstOrDefaultAsync para buscar o produto pelo ID de forma assíncrona
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.IdProduto == id);

            //Verifica se a variável está vazia
            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }
            return new CreatedAtRouteResult("Produto capturado", new { id = produto.IdProduto}, produto);
        }

        //Rota para adicionar um novo produto
        [HttpPost]
        public async Task<ActionResult<Produto>> AddNewProduct(Produto produto)
        {
            //Usando o método assíncrono ProdutoExisteAsync para verificar a existência do produto
            if (await ProdutoExisteAsync(produto))
            {
                return BadRequest("Esse produto já existe com o mesmo ID ou Nome");
            }

            _context.Produtos.Add(produto);
            // Usa SaveChangesAsync para salvar o novo produto de forma assíncrona
            await _context.SaveChangesAsync();

            return Ok("Novo produto criado");
        }

        //Método para atualizar um produto existente
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Produto>> UpdateProduct(int id, Produto produto)
        {
            // Usa o método assíncrono ProdutoExisteAsync para verificar a existência do produto
            if (!await ProdutoExisteAsync(id))
            {
                return NotFound("Produto não encontrado");
            }

            _context.Produtos.Update(produto);
            // Usa SaveChangesAsync para salvar as alterações de forma assíncrona
            await _context.SaveChangesAsync();
            
            return Ok("Alterações gravadas");        
        }

        //Método para deletar um produto
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            // Usa FindAsync para buscar o produto pelo ID de forma assíncrona
            var produto = await _context.Produtos.FindAsync(id);

            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }

            _context.Produtos.Remove(produto);
            // Usa SaveChangesAsync para deletar o produto de forma assíncrona
            await _context.SaveChangesAsync();

            return Ok("Produto removido com sucesso");
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
    }
}