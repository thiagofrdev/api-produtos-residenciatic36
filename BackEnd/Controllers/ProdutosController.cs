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
        public ActionResult<IEnumerable<Produto>> GetAllProducts()
        {
            var produtos = _context.Produtos.ToList(); //Pega todos os produtos e transforma numa lista

            //Verifica se a lista está vazia
            if (produtos is null)
            {
                return NotFound("Produtos não encontrados");
            }
            return Ok(produtos);
        }

        //Rota para pegar um produto pelo seu ID
        [HttpGet("{id:int}")]
        public ActionResult<IEnumerable<Produto>> GetProductById(int id)
        {
            //Pega o Produto
            var produto = _context.Produtos.FirstOrDefault(p => p.IdProduto == id);

            //Verifica se a variável está vazia
            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }
            return new CreatedAtRouteResult("Produto capturado", new { id = produto.IdProduto}, produto);
        }

        //Rota para adicionar um novo produto
        [HttpPost]
        public ActionResult<IEnumerable<Produto>> AddNewProduct(Produto produto)
        {
            if (ProdutoExiste(produto))
            {
                return BadRequest("Esse produto já existe com o mesmo ID ou Nome");
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return Ok("Novo produto criado");
        }

        //Método para atualizar um produto existente
        [HttpPut("{id:int}")]
        public ActionResult<IEnumerable<Produto>> UpdateProduct(int id, Produto produto)
        {
            if (!ProdutoExiste(id))
            {
                return NotFound("Produto não encontrado");
            }

            _context.Produtos.Update(produto);
            _context.SaveChanges();
            
            return Ok("Alterações gravadas");        
        }

        //Método para deletar um produto
        [HttpDelete("{id}")]
        public ActionResult<IEnumerable<Produto>> DeleteProduct(int id, Produto produto)
        {
            if (!ProdutoExiste(id))
            {
                return NotFound("Produto não encontrado");
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok("Produto removido com sucesso");
        }

        private bool ProdutoExiste(Produto produto)
        {
            return _context.Produtos.Any(t => t.IdProduto == produto.IdProduto || t.NomeProduto == produto.NomeProduto);
        }

        private bool ProdutoExiste(int id)
        {
            return _context.Produtos.Any(t => t.IdProduto != id);
        }
    }
}