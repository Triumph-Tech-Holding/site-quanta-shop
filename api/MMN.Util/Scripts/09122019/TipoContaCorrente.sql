 /*************************************************************************
Autor: Proaire / Marcelo Drumand
Data : 09/11/2019
Descrição: Script tipos de lançamentos com procedimento de Saldo, ganhos, saque e investido

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

/****** Object:  Table [dbo].[Tipo]    Script Date: 09/11/2019 14:00:42 ******/
IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Conta Corrente')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES (0, 'Conta Corrente', 'CC', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Lançamento Manual')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Lançamento Manual', 'LM', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Pagamento Binário')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Pagamento Binário', 'PB', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Pagamento Automático (trader)')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Pagamento Automático (trader)', 'AT', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Pagamento Ativação Usuário')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Pagamento Ativação Usuário', 'AU', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Compra')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Compra', 'CPR', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Compra Produto')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Compra Produto', 'CP', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Compra Voucher')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Compra Voucher', 'CV', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Compra Pacote')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Compra Pacote', 'CPCT', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Compra Serviço')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Compra Serviço', 'CS', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Compra Convite')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Compra Convite', 'CCT', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Taxa de Sistema')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Taxa de Sistema', 'TSIS', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Taxa de Saque')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Taxa de Saque', 'TSQ', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Estorno')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Estorno', 'EST', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Estorno Bônus')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Estorno Bônus', 'EB', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Estorno Trader')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Estorno Trader', 'ET', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Estorno Pagamento')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Estorno Pagamento', 'EPAG', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Imposto de Renda')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Imposto de Renda', 'IR', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'INSS')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'INSS', 'INSS', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Transferência Conta Corrente')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Transferência Conta Corrente', 'TCC', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Solicitação de Saque')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Solicitação de Saque', 'SSQ', 1)
GO

IF NOT EXISTS (SELECT * FROM [dbo].[Tipo] WHERE Descricao = 'Chash Back')
 INSERT INTO [dbo].[Tipo] ([IdTipoPai], [Descricao], [Chave], [Ativo]) VALUES ((SELECT IdTipo FROM Tipo WHERE Chave = 'CC' ), 'Chash Back', 'CB', 1)
GO