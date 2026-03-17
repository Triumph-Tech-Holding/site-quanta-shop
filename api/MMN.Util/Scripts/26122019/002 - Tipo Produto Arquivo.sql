/*************************************************************************
Autor: Proaire / TI - Marcelo Drumand
Data : 16/02/2018
Descrição: Tipos para o Arquivo do Produto
***************************************************************************/

 
PRINT '***********************************************************************';
PRINT 'Cabeçalho de execução de script - Equipe Proaire Banco de Dados';
PRINT 'Instancia   = ' + @@SERVERNAME;
PRINT 'Database    = ' + DB_NAME();
PRINT 'Data e Hora = ' + CONVERT(VARCHAR(30), GETDATE(), 121);
PRINT 'Login       = ' + SYSTEM_USER;
PRINT '***********************************************************************';
GO

USE [atlantic-wonder]
--USE MMN_Base

/****** Object:  Table [dbo].[Tipo]    Script Date: 09/11/2019 14:00:42 ******/
IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Arquivo Produto')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES (0, 'Arquivo Produto', 'ARQP', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'PDF')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'ARQP' ), 'PDF', 'PDF', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Video')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'ARQP' ), 'Video', 'MP', 1)
GO
