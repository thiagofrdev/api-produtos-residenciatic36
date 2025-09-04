using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BackEnd.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePedidoProdutoFKAndPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Clientes_IdCliente",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosProdutos_Pedidos_PedidoIdPedido",
                table: "PedidosProdutos");

            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_TiposProduto_IdTipoProduto",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_IdTipoProduto",
                table: "Produtos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidosProdutos",
                table: "PedidosProdutos");

            migrationBuilder.DropIndex(
                name: "IX_PedidosProdutos_PedidoIdPedido",
                table: "PedidosProdutos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_IdCliente",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "PedidoIdPedido",
                table: "PedidosProdutos");

            migrationBuilder.AlterColumn<int>(
                name: "IdPedido",
                table: "PedidosProdutos",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "IdPedidoProduto",
                table: "PedidosProdutos",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidosProdutos",
                table: "PedidosProdutos",
                column: "IdPedidoProduto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PedidosProdutos",
                table: "PedidosProdutos");

            migrationBuilder.DropColumn(
                name: "IdPedidoProduto",
                table: "PedidosProdutos");

            migrationBuilder.AlterColumn<int>(
                name: "IdPedido",
                table: "PedidosProdutos",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "PedidoIdPedido",
                table: "PedidosProdutos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PedidosProdutos",
                table: "PedidosProdutos",
                column: "IdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_IdTipoProduto",
                table: "Produtos",
                column: "IdTipoProduto");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosProdutos_PedidoIdPedido",
                table: "PedidosProdutos",
                column: "PedidoIdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdCliente",
                table: "Pedidos",
                column: "IdCliente");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Clientes_IdCliente",
                table: "Pedidos",
                column: "IdCliente",
                principalTable: "Clientes",
                principalColumn: "IdCliente",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosProdutos_Pedidos_PedidoIdPedido",
                table: "PedidosProdutos",
                column: "PedidoIdPedido",
                principalTable: "Pedidos",
                principalColumn: "IdPedido");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_TiposProduto_IdTipoProduto",
                table: "Produtos",
                column: "IdTipoProduto",
                principalTable: "TiposProduto",
                principalColumn: "IdTipo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
