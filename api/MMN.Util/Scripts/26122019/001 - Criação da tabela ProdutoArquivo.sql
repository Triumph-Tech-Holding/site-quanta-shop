/*************************************************************************
Autor: Proaire / TI - Marcelo Drumand
Data : 16/02/2018
Descrição: Script Criação Tabela ProdutoArquivo
***************************************************************************/

 
PRINT '***********************************************************************';
PRINT 'Cabeçalho de execução de script - Equipe Proaire Banco de Dados';
PRINT 'Instancia   = ' + @@SERVERNAME;
PRINT 'Database    = ' + DB_NAME();
PRINT 'Data e Hora = ' + CONVERT(VARCHAR(30), GETDATE(), 121);
PRINT 'Login       = ' + SYSTEM_USER;
PRINT '***********************************************************************';
GO

--USE [atlantic-wonder]
USE MMN_Base

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ProdutoArquivo')

  CREATE TABLE ProdutoArquivo(
  	IdProdutoArquivo int Identity Primary Key,
  	IdProduto int Not Null,
  	IdTipo Int Not Null,
  	URL varchar(max),
  	Ativoo bit, 
  	CONSTRAINT FK_ProdutoId FOREIGN KEY (IdProduto)
      REFERENCES Produto(IdProduto), 
  	CONSTRAINT FK_TipoId FOREIGN KEY (IdTipo)
      REFERENCES Tipo(IdTipo)
  );

GO