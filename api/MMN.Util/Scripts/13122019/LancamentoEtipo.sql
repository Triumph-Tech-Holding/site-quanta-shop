 /*************************************************************************
Autor: Proaire / Marcelo Drumand
Data : 09/11/2019
Descrição: Inserindo Campo Status na tabela lancamento e valores do status na tabela Tipo

***************************************************************************/
USE [atlantic-wonder]

PRINT '***********************************************************************';
PRINT 'Cabeçalho de execução de script - Equipe Proaire Banco de Dados';
PRINT 'Instancia   = ' + @@SERVERNAME;
PRINT 'Database    = ' + DB_NAME();
PRINT 'Data e Hora = ' + CONVERT(VARCHAR(30), GETDATE(), 121);
PRINT 'Login       = ' + SYSTEM_USER;
PRINT '***********************************************************************';

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

/****** Object:  Table [dbo].[UsuarioBanco]    Script Date: 13/12/2019 18:15:42 ******/
IF NOT EXISTS (Select * from SYS.COLUMNS where name = 'URLIMG' and object_id in (select object_id from sys.tables where name = 'Usuario'))	
BEGIN
   alter table Usuario
   ADD URLIMG varchar(MAX)
END
GO

/****** Object:  Table [dbo].[Lancamento]    Script Date: 13/12/2019 11:40:42 ******/
IF NOT EXISTS (Select * from SYS.COLUMNS where name = 'Status' and object_id in (select object_id from sys.tables where name = 'Lancamento'))	
BEGIN
    alter table lancamento
       ADD Status varchar(20)
END
GO

/****** Object:  Table [dbo].[Tipo]    Script Date: 13/12/2019 11:40:42 ******/
IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Status Lancamento')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES (0, 'Status Lancamento', 'STL', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Em Andamento')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'STL' ), 'Em Andamento', 'AND', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Finalizado')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'STL' ), 'Finalizado', 'FNL', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Cancelado')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'STL' ), 'Cancelado', 'CNC', 1)
GO



/****** Object:  Table [dbo].[Tipo]    Script Date: 13/12/2019 11:45:42 ******/
IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Tipo de Saldo')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES (0, 'Tipo de Saldo', 'TPSL', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Cashback')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'TPSL' ), 'Cashback', 'CHBK', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Saldo de Rede')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'TPSL' ), 'Saldo de Rede', 'SLRD', 1)
GO




IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Tipo de Conta')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES (0, 'Tipo de Conta', 'TPC', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Conta Corrente')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'TPC' ), 'Conta Corrente', 'CCR', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Conta Poupança')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'TPC' ), 'Conta Poupança', 'CPO', 1)
GO