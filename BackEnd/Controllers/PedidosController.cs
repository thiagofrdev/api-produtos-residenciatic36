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
    public class PedidosController : ControllerBase
    {
        private readonly ApiContext _context;

        //Construtor da classe PedidoController
        public PedidosController(ApiContext context)
        {
            _context = context;
        } 

        //Rota para pegar todos os pedidos
        [HttpGet]
        public ActionResult<IEnumerable<Pedido>> GetAllPedidos()
        {
            var pedidos = _context.Pedidos.ToList(); //Pega todos os pedidos e transforma numa lista

            //Verifica se a lista está vazia
            if (pedidos is null)
            {
                return NotFound("Nenhum pedido foi encontrado");
            }
            return Ok(pedidos);
        }

        //Rota para pegar um pedido pelo seu ID
        [HttpGet("{id:int}")]
        public ActionResult<IEnumerable<Pedido>> GetPedidoById(int id, Pedido pedido)
        {
            //Verifica se a variável está vazia
            if (!PedidoExiste(id))
            {
                return NotFound($"O pedido {id} não existe");
            }
            return new CreatedAtRouteResult(new { id = pedido.IdPedido }, pedido);
        }

        //Rota para adicionar um novo pedido
        [HttpPost("{id:int}")]
        public ActionResult<IEnumerable<Pedido>> AddNewPedido(int id, Pedido pedido)
        {
            if (PedidoExiste(id))
            {
                return BadRequest($"Já existe um pedido com o id {id}");
            }

            _context.Pedidos.Add(pedido);
            _context.SaveChanges();

            return Ok($"Pedido {id} criado com sucesso");
        }

        //Método para atualizar um pedido existente
        [HttpPut("{id:int}")]
        public ActionResult<IEnumerable<Pedido>> UpdatePedido(int id, Pedido pedido)
        {
            if (!PedidoExiste(id))
            {
                return NotFound($"O pedido {id} não foi encontrado");
            }

            _context.Pedidos.Update(pedido);
            _context.SaveChanges();
            
            return Ok($"Alterações gravadas no pedido {id}");        
        }

        //Método para deletar um pedido
        [HttpDelete("{id}")]
        public ActionResult<IEnumerable<Pedido>> DeletePedido(int id, Pedido pedido)
        {
            if (!PedidoExiste(id))
            {
                return NotFound($"O pedido {id} não foi encontrado");
            }

            _context.Pedidos.Remove(pedido);
            _context.SaveChanges();

            return Ok($"Pedido {id} removido com sucesso");
        }

        private bool PedidoExiste(int id)
        {
            return _context.Pedidos.Any(p => p.IdPedido != id);
        }
    }
}