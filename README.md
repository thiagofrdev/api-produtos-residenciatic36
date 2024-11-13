# Projeto Backend para Gestão de Produtos
Este projeto é uma API desenvolvida em C# com .NET Core e Entity Framework, que permite gerenciar produtos e tipos de produtos. A API suporta operações CRUD (Create, Read, Update, Delete) e segue uma estrutura RESTful para facilitar o consumo por outras aplicações.

## Estrutura do Projeto
O projeto está organizado da seguinte maneira:

- **Controllers**: Define os endpoints para as operações de CRUD da API.

- **Models**: Define as classes que representam as entidades do banco de dados.

- **Context**: Configura o ApiContext, que é o contexto do Entity Framework para realizar as operações de banco de dados.

- **ResponseWrapper**: Uma classe auxiliar para padronizar as respostas da API, retornando uma mensagem e os dados no mesmo JSON.

## Pré-Requisitos
Para rodar este projeto, você precisa ter:

- .NET 6.0 ou superior
- Banco de dados PostgreSQL ou SQL Server (configurável)
- Ferramentas como Visual Studio ou Visual Studio Code (com extensões para desenvolvimento em C#)

## Configuração do appsettings.json
**Importante**: Por motivos de segurança, o arquivo appsettings.json não está incluído no repositório. Esse arquivo contém as configurações sensíveis do projeto, como a string de conexão ao banco de dados. Abaixo, você encontra uma estrutura básica do arquivo para configurar o seu ambiente:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SeuBancoDeDados;User Id=SeuUsuario;Password=SuaSenha;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
#### Parâmetros
- DefaultConnection: Configure para o seu ambiente. Substitua Server, Database, User Id, e Password pelos dados do seu banco de dados.
- Coloque esse arquivo na raiz do projeto, e o .NET Core automaticamente buscará as configurações para a conexão com o banco.

## Endpoints da API
Abaixo estão os principais endpoints da API, organizados por recurso.

#### Clientes
- GET /api/Clientes: Retorna todos os clientess.
- GET /api/Clientes/{id}: Retorna um cliente específico pelo ID.
- POST /api/Clientes: Adiciona um novo cliente.
- PUT /api/Clientes/{id}: Atualiza um cliente existente.
- DELETE /api/Clientes/{id}: Remove um cliente.

#### Produtos
- GET /api/Produtos: Retorna todos os produtos.
- GET /api/Produtos/{id}: Retorna um produto específico pelo ID.
- POST /api/Produtos: Adiciona um novo produto.
- PUT /api/Produtos/{id}: Atualiza um produto existente.
- DELETE /api/Produtos/{id}: Remove um produto.

#### Pedidos
- GET /api/Pedidos: Retorna todos os pedidos.
- GET /api/Pedidos/{id}: Retorna um pedido específico pelo ID.
- POST /api/Pedidos: Adiciona um novo pedido.
- PUT /api/Pedidos/{id}: Atualiza um pedido existente.
- DELETE /api/Pedidos/{id}: Remove um pedido.

#### Produtos/Pedidos (Tabela relacional)
- GET /api/PedidosProdutos: Retorna todas as relações de pedido/produto.
- GET /api/PedidosProdutos/{id}: Retorna uma relações de pedido/produto específica pelo ID.
- POST /api/PedidosProdutos: Adiciona uma nova relação pedido/produto.
- PUT /api/PedidosProdutos/{id}: Atualiza uma relação pedido/produto existente.
- DELETE /api/PedidosProdutos/{id}: Remove uma relação pedido/produto.

#### Tipos de Produtos
- GET /api/TiposProduto: Retorna todos os tipos de produto.
- GET /api/TiposProduto/{id}: Retorna um tipo de produto específico pelo ID.
- POST /api/TiposProduto: Adiciona um novo tipo de produto.
- PUT /api/TiposProduto/{id}: Atualiza um tipo de produto existente.
- DELETE /api/TiposProduto/{id}: Remove um tipo de produto.

## Estrutura de Respostas
Todas as respostas da API seguem o padrão abaixo:
```
{
  "message": "Mensagem descritiva da operação",
  "data": {
    // Objeto com os dados do recurso
  }
}
```
Exemplo de resposta para o endpoint POST /api/Produtos:
```
{
  "message": "Produto criado com sucesso!",
  "data": {
    "idProduto": 3,
    "nomeProduto": "Produto Exemplo",
    "tipoProduto": "Tipo Exemplo",
    "quantidade": 10,
    "preco": 99.99
  }
}
```

## Executando o Projeto
Clone o repositório e navegue até o diretório do projeto:
```
git clone https://github.com/seu-usuario/nome-do-repositorio.git
```
```
cd nome-do-repositorio
```
Crie e configure o arquivo appsettings.json com os dados de conexão ao banco de dados.

Instale as dependências e crie o banco de dados (migrations e update):
```
dotnet ef database update
```
Execute o projeto:
```
dotnet run
```
Acesse a documentação da API (Swagger) no navegador em http://localhost:suaporta/swagger para explorar e testar os endpoints.

## Tecnologias Utilizadas
- .NET Core
- Entity Framework Core
- Banco de Dados (PostgreSQL ou SQL Server)
- Swagger para documentação da API

## Para contribuir com este projeto
- Faça um fork do repositório.
- Crie uma nova branch (git checkout -b feature/nova-feature).
- Faça o commit das suas alterações (git commit -m 'Adiciona nova feature').
- Faça o push para a branch (git push origin feature/nova-feature).
- Abra um Pull Request.