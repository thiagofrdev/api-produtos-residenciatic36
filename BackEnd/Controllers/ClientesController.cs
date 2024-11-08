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
    public class ClientesController : ControllerBase
    {
        private readonly ApiContext _context;

        //Construtor da classe ClienteController
        public ClientesController(ApiContext context)
        {
            _context = context;
        }

        //Rota para pegar todos os Clientes
        [HttpGet]
        public ActionResult<IEnumerable<Cliente>> GetAllClientes()
        {
            var clientes = _context.Clientes.ToList(); //Pega todos os clientes e transforma numa lista

            //Verifica se a lista está vazia
            if (clientes is null)
            {
                return NotFound("Clientes não encontrados");
            }
            return Ok(clientes);
        }

        //Rota para pegar um cliente pelo seu ID
        [HttpGet("{id:int}")]
        public ActionResult<IEnumerable<Cliente>> GetClienteById(int id)
        {
            //Pega o cliente
            var cliente = _context.Clientes.FirstOrDefault(c => c.IdCliente == id);

            //Verifica se a variável está vazia
            if (cliente is null)
            {
                return NotFound("Cliente não encontrado");
            }
            return new CreatedAtRouteResult(new { id = cliente.IdCliente}, cliente);
        }

        //Rota para adicionar um novo cliente
        [HttpPost]
        public ActionResult<IEnumerable<Cliente>> AddNewCliente(Cliente cliente)
        {
            if (ClienteExiste(cliente))
            {
                return BadRequest("Esse cliente já existe com o mesmo ID ou Nome");
            }

            _context.Clientes.Add(cliente);
            _context.SaveChanges();

            return Ok("Novo cleinte criado");
        }

        //Método para atualizar um cliente existente
        [HttpPut("{id:int}")]
        public ActionResult<IEnumerable<Cliente>> UpdateCliente(int id, Cliente cliente)
        {
            if (!ClienteExiste(id))
            {
                return NotFound("Cliente não encontrado");
            }

            _context.Clientes.Update(cliente);
            _context.SaveChanges();
            
            return Ok("Alterações gravadas");        
        }

        [HttpDelete("{id}")]
        public ActionResult<IEnumerable<Cliente>> DeleteCliente(int id, Cliente cliente)
        {
            if (!ClienteExiste(id))
            {
                return NotFound("Cliente não encontrado");
            }

            _context.Clientes.Remove(cliente);
            _context.SaveChanges();

            return Ok("Cliente removido com sucesso");
        }

        private bool ClienteExiste(Cliente cliente)
        {
            return _context.Clientes.Any(c => c.IdCliente == cliente.IdCliente || c.NomeCliente == cliente.NomeCliente);
        }

        private bool ClienteExiste(int id)
        {
            return _context.Clientes.Any(t => t.IdCliente != id);
        }
    }
}