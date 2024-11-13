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
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAllClientes()
        {
            // Usa ToListAsync para buscar todos os clientes de forma assíncrona e evitar bloqueio de thread
            var clientes = await _context.Clientes.ToListAsync();

            //Verifica se a lista está vazia
            if (clientes is null)
            {
                return NotFound("Clientes não encontrados");
            }
            return Ok(clientes);
        }

        //Rota para pegar um cliente pelo seu ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<RespostaRequisicao<Cliente>>> GetClienteById(int id)
        {
            // Usa FirstOrDefaultAsync para buscar o cliente pelo ID de forma assíncrona
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.IdCliente == id);

            //Verifica se a variável está vazia
            if (cliente is null)
            {
                return NotFound(new RespostaRequisicao<Cliente>("Cliente não encontrado", null));
            }
            return Ok(new RespostaRequisicao<Cliente>($"Cliente {id} encontrado com sucesso", cliente));
        }

        //Rota para adicionar um novo cliente
        [HttpPost]
        public async Task<ActionResult<RespostaRequisicao<Cliente>>> AddNewCliente(Cliente cliente)
        {
            // Usa o método assíncrono ClienteExisteAsync para verificar a existência do cliente sem bloquear a execução
            if (await ClienteExisteAsync(cliente))
            {
                return BadRequest(new RespostaRequisicao<Cliente>("Esse cliente já existe com o mesmo ID ou Nome", null));
            }

            _context.Clientes.Add(cliente);
            // Usa SaveChangesAsync para salvar as mudanças no banco de dados de forma assíncrona
            await _context.SaveChangesAsync();

            return Ok(new RespostaRequisicao<Cliente>("Novo cliente criado", cliente));
        }

        //Método para atualizar um cliente existente
        [HttpPut("{id:int}")]
        public async Task<ActionResult<RespostaRequisicao<Cliente>>> UpdateCliente(int id, Cliente cliente)
        {
            // Usa o método assíncrono ClienteExisteAsync para verificar se o cliente existe
            if (!await ClienteExisteAsync(id))
            {
                return NotFound (new RespostaRequisicao<Cliente>($"Cliente {id} não encontrado", null));
            }

            _context.Clientes.Update(cliente);
            // Usa SaveChangesAsync para salvar as alterações de forma assíncrona
            await _context.SaveChangesAsync();
            
            return Ok(new RespostaRequisicao<Cliente>($"Alterações no cliente {id} gravadas com sucesso!", cliente));        
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<RespostaRequisicao<Cliente>>> DeleteCliente(int id)
        {
            // Usa FirstOrDefaultAsync para buscar o cliente pelo ID de forma assíncrona
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.IdCliente == id);

            // Usa o método assíncrono ClienteExisteAsync para verificar se o cliente existe
            if (cliente is null)
            {
                return NotFound(new RespostaRequisicao<Cliente>($"Cliente com id {id} não encontrado", null));
            }

            _context.Clientes.Remove(cliente);
            // Usa SaveChangesAsync para deletar o cliente de forma assíncrona
            await _context.SaveChangesAsync();

            return Ok(new RespostaRequisicao<Cliente>($"Cliente {id} removido com sucesso", null));
        }

        // Método privado assíncrono para verificar se o cliente já existe com base em suas propriedades
        private async Task<bool> ClienteExisteAsync(Cliente cliente)
        {
            return await _context.Clientes.AnyAsync(c => c.IdCliente == cliente.IdCliente || c.NomeCliente == cliente.NomeCliente);
        }

        // Método privado assíncrono para verificar se um cliente com o ID fornecido existe
        private async Task<bool> ClienteExisteAsync(int id)
        {
            return await _context.Clientes.AnyAsync(t => t.IdCliente == id);
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