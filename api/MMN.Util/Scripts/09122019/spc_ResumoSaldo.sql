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
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spc_ResumoSaldo')
   DROP PROCEDURE spc_ResumoSaldo     
GO

-- =============================================    
-- Author: Marcelo Drumand     
-- Create date: 09-11-2019     
-- Description: Este Procedimento irá popular os Box da Home(Saldo Disponivel, Seus Ganhos, Valor Sacado, Valor Investido)    
-- spc_ValoresHome '7282CE8B-3E9A-4876-8A97-D8CFC98CC4DD'  
-- =============================================    
CREATE PROCEDURE spc_ResumoSaldo    
(    
  @IdUsuario UNIQUEIDENTIFIER                          
)    
AS    
BEGIN     
    
 --DECLARE @IdUsuario UNIQUEIDENTIFIER = '7282CE8B-3E9A-4876-8A97-D8CFC98CC4DD'
 
 DECLARE @GanhosCashBack DECIMAL   
 DECLARE @GanhosRede DECIMAL   
 
 DECLARE @GastosCashBack DECIMAL   
 DECLARE @GastosRede DECIMAL   

 DECLARE @SaldoCashBack DECIMAL    
 DECLARE @SaldoRede DECIMAL  
     
 
 SELECT @GanhosCashBack = SUM(Valor)     
  FROM  Lancamento WHERE IdUsuario = @IdUsuario    
                     AND IdTipo IN (SELECT IdTipo FROM Tipo    
                      WHERE Chave IN ('CB') AND Ativo = 1 AND Valor > 0)    

 SELECT @GanhosRede = SUM(Valor)     
  FROM  Lancamento WHERE IdUsuario = @IdUsuario    
                     AND IdTipo IN (SELECT IdTipo FROM Tipo    
                      WHERE Chave IN ('AU','AT','PB','TCC','LM','EST','EB','ET','EAU','EPAG','IR','INSS') AND Ativo = 1) 

 SELECT @GastosCashBack = SUM(Valor)     
  FROM  Lancamento WHERE IdUsuario = @IdUsuario    
                     AND IdTipo IN (SELECT IdTipo FROM Tipo    
                      WHERE Chave IN ('CB') AND Ativo = 1 AND Valor <= 0)    

 SELECT @GastosRede = SUM(Valor)     
  FROM  Lancamento WHERE IdUsuario = @IdUsuario    
                     AND IdTipo IN (SELECT IdTipo FROM Tipo    
                      WHERE Chave IN ('CPR','CP','CV','CPCT','CS','CCT','TSIS','TSQ','INSS','TCC','IR','SSQ') AND Ativo = 1) 
 
 SELECT @SaldoCashBack = SUM(Valor)     
  FROM  Lancamento WHERE IdUsuario = @IdUsuario  
                     And idTipo IN (SELECT IdTipo FROM Tipo    
                      WHERE Chave IN ('CB') AND Ativo = 1) 

 SELECT @SaldoRede = SUM(Valor)     
  FROM  Lancamento WHERE IdUsuario = @IdUsuario  
                     And idTipo NOT IN (SELECT IdTipo FROM Tipo    
                      WHERE Chave IN ('CB') AND Ativo = 1) 
					  
					      
 SELECT 'CashBack' AS TipoSaldo, ISNULL(@GanhosCashBack,0) AS Ganhos, ISNULL(@GastosCashBack,0) AS Gastos,ISNULL(@SaldoCashBack,0) AS Saldo   
 UNION
 SELECT 'SaldoRede' AS TipoSaldo, ISNULL(@GanhosRede,0) AS Ganhos, ISNULL(@GastosRede,0) AS Gastos, ISNULL(@SaldoRede,0) AS Saldo     

END  


