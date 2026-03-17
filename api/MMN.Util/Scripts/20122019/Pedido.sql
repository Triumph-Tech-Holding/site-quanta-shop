 /*************************************************************************
Autor: Proaire / Marcelo Drumand
Data : 20/12/2019
Descrição: Inserindo novo campo na tabela Pedido

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

/****** Object:  Table [dbo].[Pedido]    Script Date: 20/12/2019 09:20:00 ******/
IF NOT EXISTS (Select * from SYS.COLUMNS where name = 'Cotacao' and object_id in (select object_id from sys.tables where name = 'Pedido'))	
BEGIN
   ALTER TABLE Pedido
   ADD Cotacao Decimal NOT NULL DEFAULT(0)
END
GO
