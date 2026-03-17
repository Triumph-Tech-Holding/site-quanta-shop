-- =============================================    
-- Author:      Sergio Junior    
-- Create Date: 24/12/2019    
-- Description: Processa o cashback da rede    
-- =============================================    
ALTER PROCEDURE [dbo].[spc_ProcessaCashback]    
AS    
BEGIN    
    BEGIN TRANSACTION;    
 BEGIN TRY           
    
  DECLARE @ValorCashback decimal(18,8), @dia int, @dataProcessamento datetime    
  SET @dataProcessamento = GETDATE()    
  SELECT @dia = DATEPART(dw,GETDATE())    
  SELECT @ValorCashback = CONVERT(DECIMAL(18,8), REPLACE(Valor, ',', '.')) / 100 from Configuracao where Chave = 'PERCENTUAL_CASHBACK_DIARIO'    
    
  --Não paga de sabado pra domingo nem de domingo pra segunda    
  if(@dia >= 2)     
  BEGIN    
   CREATE TABLE #tmpCashback    
   (    
    idUsuarioProduto bigint,    
    idUsuario uniqueidentifier,    
    idProduto int,    
    valor decimal (16,8)    
   )    
    
   INSERT INTO #tmpCashback    
   SELECT IdUsuarioProduto, UP.IdUsuario, P.IdProduto, P.Valor * (@ValorCashback)    
     FROM UsuarioProduto UP    
    INNER JOIN Produto P  ON UP.IdProduto = P.IdProduto    
    INNER JOIN Pedido Ped ON Ped.IdPedido = UP.IdPedido    
    WHERE UP.Ativo = 1     
      AND P.Ativo = 1     
      AND UP.DataVinculo <= DATEADD(DAY, -2, GETDATE())    
      AND Ped.MeioPagamento NOT IN (4,5) -- Pagamentos por voucher geram cashback    
    
   --Se ja tem pagamento hoje não paga novamente    
   DELETE FROM #tmpCashback     
    WHERE Exists (SELECT 1     
        FROM LogPagamentoCashback logPC    
       WHERE #tmpCashback.idUsuarioProduto = logPC.idUsuarioProduto    
         AND CONVERT(date,GETDATE()) = CONVERT(date,logPC.DataPagamento))    
    
       
   -- Insere as transações    
   INSERT INTO Transacao    
   SELECT idUsuario, (select IdTipo from Tipo where Chave = 'CB' ), valor, @dataProcessamento, 'Divisão de lucros', 1, 2     
     FROM #tmpCashback    
    
           INSERT INTO Lancamento    
     SELECT IdUsuario, IdTransacao, IdTipo, ValorPrincipal, @dataProcessamento, 1, 'Divisão de lucros', 2 FROM Transacao    
      WHERE IdTipo = (select IdTipo from Tipo where Chave = 'CB' )    
     AND DataTransacao = @dataProcessamento    
    
           INSERT INTO LogPagamentoCashback    
     SELECT idUsuarioProduto, @dataProcessamento, valor FROM #tmpCashback    
    
     DROP TABLE #tmpCashback    
    END    
    
    COMMIT TRANSACTION;    
 END TRY                    
 BEGIN CATCH                                
  ROLLBACK TRANSACTION;                
  DECLARE @ERRORMESSAGE NVARCHAR(4000);                                                  
  DECLARE @ERRORSEVERITY INT;                                                                  
  DECLARE @ERRORSTATE INT;                                                   
     
  SELECT                                                                     
  @ERRORMESSAGE = '(UU)'+ ERROR_MESSAGE(),                                                                    
  @ERRORSEVERITY = ERROR_SEVERITY(),                                                        
  @ERRORSTATE = ERROR_STATE();                                                                    
  RAISERROR (@ERRORMESSAGE, -- MESSAGE TEXT.                                                            
  @ERRORSEVERITY, -- SEVERITY.                                          
  @ERRORSTATE -- STATE.                                       
  );                                      
                                  
 END CATCH              
END 