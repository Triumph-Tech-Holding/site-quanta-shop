/*************************************************************************
Autor: Proaire / Marcelo Drumand
Data : 09/11/2019
Descrição: Script tipos de lançamentos com procedimento de Saldo, ganhos, saque e investido

***************************************************************************/
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

/****** Object:  Procedure spc_ValoresHome   Script Date: 09/11/2019 14:00:42 ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spc_ValoresHome')
   DROP PROCEDURE spc_ValoresHome     
GO

-- =============================================  
-- Author: Marcelo Drumand   
-- Create date: 09-11-2019   
-- Description: Este Procedimento irá popular os Box da Home(Saldo Disponivel, Seus Ganhos, Valor Sacado, Valor Investido)  
-- spc_ValoresHome "DE526701-F9C9-425C-93AC-24316B057EED"  
-- =============================================  
CREATE PROCEDURE spc_ValoresHome  
(  
  @IdUsuario UNIQUEIDENTIFIER                        
)  
AS  
BEGIN   
   
 DECLARE @SaldoDisponivel DECIMAL  
 DECLARE @Ganhos DECIMAL  
 DECLARE @Saque DECIMAL  
 DECLARE @Investido DECIMAL  
  
 SELECT @SaldoDisponivel = SUM(Valor)   
  FROM  Lancamento WHERE IdUsuario = @IdUsuario  
  
 SELECT @Ganhos = SUM(Valor)   
  FROM  Lancamento WHERE IdUsuario = @IdUsuario  
                     AND IdTipo IN (SELECT IdTipo FROM Tipo  
                      WHERE Chave IN ('PB','AT','AU') AND Ativo = 1)   
  
 SELECT @Saque = SUM(Valor)   
  FROM  Lancamento WHERE IdUsuario = @IdUsuario  
                     AND IdTipo IN (SELECT IdTipo FROM Tipo  
                      WHERE Chave IN ('SSQ') AND Ativo = 1)   
  
 SELECT @Investido = SUM(Valor)   
  FROM  Lancamento WHERE IdUsuario = @IdUsuario  
                     AND IdTipo IN (SELECT IdTipo FROM Tipo  
                      WHERE Chave IN ('CPR','CP','CV','CPCT','CS','CCT') AND Ativo = 1)   
  
 SELECT ISNULL(@SaldoDisponivel,0) AS Saldo, ISNULL(@Ganhos,0) AS Ganhos, ISNULL(@Saque,0) AS Saque, ISNULL(@Investido,0) AS Investimento  
   
END
GO
PRINT 'PROCEDIMENTO CRIADO'
