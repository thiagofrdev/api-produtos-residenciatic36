using BackEnd.Infrastructure.Data;
using BackEnd.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<Pedido>>> GetAllPedidos()
        {
             //Usaando ToListAsync para buscar todos os pedidos de forma assíncrona
            var pedidos = await _context.Pedidos.ToListAsync();

            //Verifica se a lista está vazia
            if (pedidos is null)
            {
                return NotFound("Nenhum pedido foi encontrado");
            }
            return Ok(pedidos);
        }

        //Rota para pegar um pedido pelo seu ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<RespostaRequisicao<Pedido>>> GetPedidoById(int id)
        {
            //Usando FindAsync para buscar o pedido pelo ID de forma assíncrona
            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.IdPedido == id);
            
            //Verifica se a variável está vazia
            if (pedido is null)
            {
                return NotFound(new RespostaRequisicao<Pedido>($"O pedido com o id '{id}' não foi encontrado na base de dados", null));
            }
            return Ok(new RespostaRequisicao<Pedido>($"Pedido {id} encontrado com sucesso", pedido));
        }

        //Rota para adicionar um novo pedido
        [HttpPost("{id:int}")]
        public async Task<ActionResult<RespostaRequisicao<Pedido>>> AddNewPedido(int id, Pedido pedido)
        {
            //Usando o método assíncrono PedidoExisteAsync para verificar a existência do pedido
            if (await PedidoExisteAsync(id))
            {
                return BadRequest(new RespostaRequisicao<Pedido>($"Já existe um pedido com o mesmo ID '{id}'", null));
            }

            _context.Pedidos.Add(pedido);
            //Usando SaveChangesAsync para salvar as mudanças de forma assíncrona
            await _context.SaveChangesAsync();

            return Ok(new RespostaRequisicao<Pedido>("Novo pedido adicionado com sucesso!", pedido));
        }

        //Método para atualizar um pedido existente
        [HttpPut("{id:int}")]
        public async Task<ActionResult<RespostaRequisicao<Pedido>>> UpdatePedido(int id, Pedido pedido)
        {
            if (!await PedidoExisteAsync(id))
            {
                return NotFound(new RespostaRequisicao<Pedido>($"Pedido com ID {id} não encontrado", null));
            }

            _context.Pedidos.Update(pedido);
             //Usando SaveChangesAsync para salvar as alterações de forma assíncrona
            await _context.SaveChangesAsync();
            
            return Ok(new RespostaRequisicao<Pedido>($"Alterações no pedido {id} gravadas com sucesso!", pedido));       
        }

        //Método para deletar um pedido
        [HttpDelete("{id}")]
        public async Task<ActionResult<RespostaRequisicao<Pedido>>> DeletePedido(int id)
        {
            //Usando FindAsync para buscar o pedido pelo ID de forma assíncrona
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido is null)
            {
                return NotFound(new RespostaRequisicao<Pedido>($"Pedido com ID {id} não encontrado", null));
            }

            _context.Pedidos.Remove(pedido);
            //Usando SaveChangesAsync para deletar o pedido de forma assíncrona
            await _context.SaveChangesAsync();

            return Ok(new RespostaRequisicao<Pedido>($"Pedido {id} removido com sucesso", null));
        }

        //Método privado assíncrono para verificar se o pedido existe no banco de dados com base no ID
        private async Task<bool> PedidoExisteAsync(int id)
        {
            return await _context.Pedidos.AnyAsync(p => p.IdPedido != id);
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