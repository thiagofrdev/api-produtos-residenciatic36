using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Context;
using BackEnd.Models;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<PedidoProduto>> GetAllPedidosProdutos()
        {
            var pedidosProdutos = _context.Produtos.ToList(); //Pega todos os dados da tabela e transforma numa lista

            //Verifica se a lista está vazia
            if (pedidosProdutos is null)
            {
                return NotFound("Nada encontrado");
            }
            return Ok(pedidosProdutos);
        }

        //Rota para pegar um PedidoProduto pelo seu ID
        [HttpGet("{id:int}")]
        public ActionResult<IEnumerable<Produto>> GetPedidosProdutosById(int id)
        {
            //Pega o Produto
            var pedidoProduto = _context.PedidosProdutos.FirstOrDefault(pp => pp.IdPedidoProduto == id);

            //Verifica se a variável está vazia
            if (pedidoProduto is null)
            {
                return NotFound("Nada encontrado");
            }
            return new CreatedAtRouteResult("Produto capturado", new { id = pedidoProduto.IdProduto}, pedidoProduto);
        }

        //Rota para adicionar um novo PedidoProduto
        [HttpPost]
        public ActionResult<IEnumerable<PedidoProduto>> AddNewPedidoProduto(int id, PedidoProduto pedidoProduto)
        {
            if (PedidoProdutoExiste(id))
            {
                return BadRequest("Essa relação pedido/produto já existe com o mesmo ID");
            }

            _context.PedidosProdutos.Add(pedidoProduto);
            _context.SaveChanges();

            return Ok("Novo produto criado");
        }

        //Método para atualizar um PedidoProduto existente
        [HttpPut("{id:int}")]
        public ActionResult<IEnumerable<PedidoProduto>> UpdatePedidoProduto(int id, PedidoProduto pedidoProduto)
        {
            if (!PedidoProdutoExiste(id))
            {
                return NotFound("Nada encontrado");
            }

            _context.PedidosProdutos.Update(pedidoProduto);
            _context.SaveChanges();
            
            return Ok("Alterações gravadas");        
        }

        //Método para deletar um PedidoProduto
        [HttpDelete("{id}")]
        public ActionResult<IEnumerable<Produto>> DeletePedidoProduto(int id, PedidoProduto pedidoProduto)
        {
            if (!PedidoProdutoExiste(id))
            {
                return NotFound("Nada encontrado");
            }

            _context.PedidosProdutos.Remove(pedidoProduto);
            _context.SaveChanges();

            return Ok("Relação removida com sucesso");
        }

        private bool PedidoProdutoExiste(int id)
        {
            return _context.PedidosProdutos.Any(pp => pp.IdPedidoProduto != id);
        }
    }
}