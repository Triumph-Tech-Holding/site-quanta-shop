-- =============================================
-- Author:      Sergio Junior
-- Create Date: 24/12/2019
-- Description: Processa o binário diário
-- =============================================               		
ALTER PROCEDURE [dbo].[spc_processabinario] 
(                                                                                		
  @dataini DATETIME = NULL,
  @dataProcessamento DATETIME = NULL
)                                                                                		
AS                                                                                		
BEGIN   
    		
BEGIN TRANSACTION;                         		
BEGIN TRY                       		
                      		
  DECLARE @vlrpagamento			INT,
          @idpagamentobinario	INT,                                  		
		  @ultimopagamento		DATETIME,
		  @valor				MONEY, 
		  @total				INT, 
		  @pago					MONEY    
                                                                     		
  SELECT TOP 1 @dataini = DataPagamento
    FROM pagamentobinario
   WHERE @dataini is null
   ORDER BY idpagamentobinario DESC
                                                                    		
  SELECT @dataProcessamento = GETDATE() WHERE @dataProcessamento IS NULL
                                   		
  INSERT INTO pagamentobinario (datapagamento,totalpontos,valorpago)                                  		
  SELECT @dataProcessamento,0,0                                   		
                                  		
                                		
	SET @idpagamentobinario = SCOPE_IDENTITY()
     
   CREATE TABLE #rendimento                                                                                                         		
   (                                		
		id				INT IDENTITY,
		pontosEsquerda	int,
		pontoDireita	int,
		pontospagos		int,
		pontospagar		int,
		idusuario		uniqueidentifier,
		pagar			int,
		porcentagem		int,
		idproduto		int,
		valorapagar		money,
		Percentual		decimal(10,3),
		Teto			money,
		feponto			int,
		fdponto			int,
   )
                                                                            		
                                   		
   --INSERE TEMPORARIA SOMENTE QUEM ESTA ATIVO E QUALIFICADO                               		
   INSERT INTO #rendimento ( pontosEsquerda, pontoDireita, pontospagos,idusuario, Percentual, idproduto, Teto)
   SELECT 0, 0, 0, m.idusuario, 0.5, P.idproduto, p.TetoBinario
     FROM matriz  m 
    INNER JOIN usuario u ON u.idusuario = m.idusuario
    INNER JOIN UsuarioProduto up ON up.idusuario = u.idusuario AND up.Ativo = 1
	INNER JOIN Produto p ON p.IdProduto = UP.IdProduto
    WHERE u.ativo = 1                               		
      AND up.Ativo = 1
	  AND DataQualificacao is not null
   
   --Encontra os pontos de cada perna                           		
   UPDATE #rendimento                                          		
      SET pontosEsquerda = dbo.fncPontosPorLado(#rendimento.idusuario,0,null,@dataProcessamento, 0),
		  pontoDireita   = dbo.fncPontosPorLado(#rendimento.idusuario,1,null,@dataProcessamento, 0),
          pontospagos    = ISNULL(( SELECT SUM(p.totalpontos)
								      FROM pagamentobinariousuario p                                   		
								     WHERE p.idusuario = #rendimento.idusuario ),0)                                  		
  
  --Remove os pontos já pagos                     		
  UPDATE #rendimento                                   		
	 SET pontosEsquerda -= pontospagos,
	 	 pontoDireita -= pontospagos                		
  
  --Seta pontos a pagar pela perna menor                                     		
  UPDATE #rendimento                                     		
     SET pontospagar = CASE WHEN pontosEsquerda > pontoDireita 
	                        THEN pontoDireita 
							ELSE pontosEsquerda	
						END             		                               		
   --Encontra valor a pagar (considerando porcentagem e teto)                                     		
   UPDATE #rendimento set valorapagar = pontospagar * percentual
   UPDATE #rendimento set valorapagar = case when valorapagar > teto then teto else valorapagar end                                		
   
   --Insere pagamento efetuado                            		
   INSERT INTO pagamentobinariousuario (idusuario,idpagamentobinario,totalpontos,valorpago,datapagamento)                    		
   SELECT idusuario,@idpagamentobinario, ISNULL(pontospagar,0),isnull(valorapagar,0),@dataProcessamento                                		
     FROM #rendimento                                  		
    WHERE valorapagar > 0                                 		
                         		
    -- Insere as transações
	INSERT INTO Transacao
	SELECT idUsuario, (SELECT IdTipo FROM Tipo WHERE Chave = 'PB' ), valorapagar, @dataProcessamento, 'Pagamento Binário do dia ' + CONVERT(VARCHAR(10), @dataProcessamento, 103) + ' - ' + CONVERT(VARCHAR(10),pontospagar) + ' pontos pagos', 1, 2 
  	  FROM #rendimento
	 WHERE valorapagar > 0

	-- Insere as Lançamentos
	INSERT INTO Lancamento
	SELECT IdUsuario, IdTransacao, IdTipo, ValorPrincipal, @dataProcessamento, 1, Transacao.descricao, 2 
	  FROM Transacao
	 WHERE IdTipo = (SELECT IdTipo FROM Tipo WHERE Chave = 'PB' )
	   AND DataTransacao = @dataProcessamento
                                             		
   SELECT @total = SUM(pontospagar), @pago =  sum(valorapagar)                                    		
     FROM #rendimento                               		
    WHERE valorapagar > 0                        		

   UPDATE pagamentobinario 
      SET totalpontos = isnull(@total,0), valorpago = isnull(@pago,0), datapagamento = @dataProcessamento 
	WHERE idpagamentobinario = @idpagamentobinario                            		
   
    COMMIT TRANSACTION;                 		
   print 'commited';
   	
   DROP TABLE #rendimento                                         		
                               		
END TRY                                		
 BEGIN CATCH                                            		
     ROLLBACK TRANSACTION;                        		
   DECLARE @ErrorMessage NVARCHAR(4000);             		
   DECLARE @ErrorSeverity INT;                                                                              		
   DECLARE @ErrorState INT;                                                               		
   SELECT                                                                                 		
   @ErrorMessage = '(UU)'+ ERROR_MESSAGE(),                                                                                		
   @ErrorSeverity = ERROR_SEVERITY(),                                          		
   @ErrorState = ERROR_STATE();                                                                                		
   RAISERROR (@ErrorMessage, -- Message text.                                                                        		
      @ErrorSeverity, -- Severity.                                                      		
      @ErrorState -- State.                                                   		
      );                                                  		
 END CATCH                                 		
END              		
EXEC spc_ProcessaCashback 		