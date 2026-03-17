
INSERT INTO Tipo
VALUES(3, 'Distribuição de Cashback', 'DTCSH', 1)



USE [Bigcash_dev]
GO
/****** Object:  StoredProcedure [dbo].[spc_ProcessaCashback]    Script Date: 24/03/2020 11:06:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================    
-- Author:      LEONARDO DOS SANTOS
-- Create Date: 24-03-20    
-- Description: Processa o cashback da rede
-- [dbo].[spc_DistribuicaoCashback] '806A92A9-CA5C-4DDE-9B29-54579BE0B279', '806A92A9-CA5C-4DDE-9B29-54579BE0B315', 10, 'Carrefuor BR', '12396'
-- =============================================    
ALTER PROCEDURE [dbo].[spc_DistribuicaoCashback]
(
	@IdUsuario UNIQUEIDENTIFIER,
	@IdVenda UNIQUEIDENTIFIER,
	@ValorCashback DECIMAL(18,8),
	@nomeProgramaZanox VARCHAR(150),
	@idProgramZanox VARCHAR(80)
)
AS    
BEGIN    
  BEGIN TRY         
	BEGIN TRANSACTION;
		DECLARE @percentualUsuario VARCHAR(30);
		DECLARE @valorFinalUsuario DECIMAL(18,2);
		DECLARE @idTransacao INT;
		DECLARE @idTipo INT;
		DECLARE @data DATETIME;
		DECLARE @transacaoDesc VARCHAR(200);
		DECLARE @porcentagemCashback DECIMAL(18,2);
		DECLARE @nivel INT;
		DECLARE @idProdutoAtivo INT;
		DECLARE @tipoCashbackLancamento INT;
		DECLARE @valorTotalDistribuido DECIMAL(18,8);
		DECLARE @valorFinalUsuarioUpline DECIMAL(18,8);
		DECLARE @loginUsuario VARCHAR(150);

		SELECT @percentualUsuario = Valor FROM Configuracao WHERE Chave = 'PERCENTUAL_CASHBACK';
		SELECT @valorFinalUsuario = @ValorCashback * CAST(@percentualUsuario AS DECIMAL(18,8));
		SELECT @idTipo = IdTipo FROM Tipo WHERE Chave = 'CHBK';
		SELECT @data = GETDATE();

		IF(EXISTS(SELECT * FROM Usuario WHERE IdUsuario = @IdUsuario))
		BEGIN
			SELECT @loginUsuario = Login FROM Usuario WHERE IdUsuario = @IdUsuario;
			SELECT @idProdutoAtivo = IdProduto FROM UsuarioProduto WHERE IdUsuario = @IdUsuario AND Ativo = 1 ORDER BY IdUsuarioProduto DESC;
			
			INSERT INTO Transacao
			VALUES (
				@IdUsuario, 
				@idTipo, 
				@valorFinalUsuario, 
				@data, 
				'CashBack referente à compra em: ' + @nomeProgramaZanox, 
				2, 
				1, 
				@idProgramZanox, 
				@IdVenda,
				null
			);

			SELECT @idTransacao = @@IDENTITY;

			INSERT INTO Lancamento
			VALUES (
				@IdUsuario, 
				@idTransacao, 
				@idTipo, 
				@valorFinalUsuario, 
				@data, 
				1, 
				'CashBack referente à compra em: ' + @nomeProgramaZanox, 
				2
			);

			SELECT @valorTotalDistribuido = @valorFinalUsuario;

			DECLARE upline_cursor CURSOR FOR   
			SELECT Nivel, PorcentagemCashback  
			FROM ProdutoNivel
			WHERE IdProduto = @idProdutoAtivo
			ORDER BY nivel;  

			OPEN upline_cursor
			
			FETCH NEXT FROM upline_cursor   
			INTO @nivel, @porcentagemCashback
			
			SELECT @tipoCashbackLancamento = IdTipo FROM Tipo WHERE Chave = 'DTCSH';

			WHILE @@FETCH_STATUS = 0  
			BEGIN
				DECLARE @idUsuarioUpline UNIQUEIDENTIFIER;

				SELECT @idUsuarioUpline = idusuario FROM fnc_GetUsuarioUpLine(@IdUsuario) WHERE nivel = @nivel;

				IF(@idUsuarioUpline IS NOT NULL)
				BEGIN
					SELECT @valorFinalUsuarioUpline = @ValorCashback * (@porcentagemCashback / 100);
					INSERT INTO Lancamento
					VALUES(
						@idUsuarioUpline, 
						@idTransacao, 
						@tipoCashbackLancamento, 
						@valorFinalUsuarioUpline, 
						@data, 
						1,
						'Distruibuicão cashback de: ' + @loginUsuario,
						2
					);
					SELECT @valorTotalDistribuido = @valorTotalDistribuido + @valorFinalUsuarioUpline;
				END

				FETCH NEXT FROM upline_cursor   
				INTO @nivel, @porcentagemCashback
			END
			CLOSE upline_cursor;  
			DEALLOCATE upline_cursor;

			INSERT INTO Lancamento
			VALUES(
				'CF958511-6345-4898-834D-2636BFFEF353',
				@idTransacao,
				@tipoCashbackLancamento,
				(@ValorCashback - @valorTotalDistribuido),
				@data,
				1,
				'Distruibuicão cashback de: ' + @loginUsuario,
				2
			)
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



GO
/****** Object:  StoredProcedure [dbo].[spc_obterBarraDeStatus]    Script Date: 25/03/2020 13:35:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================          
-- Author: Sérgio Junior           
-- Create date: 09-10-2019           
-- Description: Este Procedimento irá popular a barra de status do usuário logado          
-- spc_obterBarraDeStatus 'B56B2E89-2713-46FB-85E3-629723109F92'          
-- =============================================          
CREATE PROCEDURE [dbo].[spc_obterBarraDeStatus]        
(        
 @idusuario UNIQUEIDENTIFIER        
)        
AS        
BEGIN        
        
 CREATE TABLE #tmpStatus        
 (        
  idUsuario  UNIQUEIDENTIFIER,        
  idGraduacao  INT,        
  posicaoBinario INT,        
  graduacao  VARCHAR(100),        
  limiteDeGanhos DECIMAL (16,8 ),  
  valorInvestido DECIMAL (16,8 ),  
  meuLink   VARCHAR(100),        
  idPlano   INT,        
  planoAtivo  VARCHAR(100),        
  dtAtivo   DATETIME,        
  totalEsquerda INT,        
  totalDireita INT,        
  total   INT,        
  totalDireitos INT,        
  mesesAtivo      INT ,      
  urlImg VARCHAR(MAX)      
 )        
 DECLARE @urlBase VARCHAR(250);   
 DECLARE @temProduto bit  
  
 SELECT @temProduto = 1 FROM UsuarioProduto   
  INNER JOIN Produto ON Produto.IdProduto = UsuarioProduto.IdProduto  
  WHERE IdUsuario = @idUsuario   
 AND UsuarioProduto.Ativo = 1  
 AND Produto.Pontos > 0  
        
 SELECT @urlBase = Valor FROM Configuracao WHERE Chave = 'URL_BASE'        
        
 INSERT INTO #tmpStatus (idGraduacao, graduacao, idUsuario, meuLink, planoAtivo, posicaoBinario, mesesAtivo, urlImg, idPlano)        
 SELECT Graduacao.IdGraduacao, Graduacao.Nome, @idusuario, @urlBase + '/#/login/' + Login, 'Nenhum plano ativo', PosicaoBinario, ISNULL(DATEDIFF(MONTH, Usuario.DataReferencia, GETDATE()),0), URLIMG, 0         
   FROM Usuario         
  INNER JOIN Graduacao on Usuario.IdGraduacao = Graduacao.IdGraduacao        
  WHERE idusuario = @idusuario        
        
  UPDATE #tmpStatus        
  SET valorInvestido = ISNULL((SELECT SUM(P.Valor)         
       FROM UsuarioProduto UP        
      INNER JOIN Produto P ON P.IdProduto = UP.IdProduto        
      WHERE idusuario = @idusuario  
	    AND P.Pontos > 0       
        AND UP.Ativo = 1),0)        
   
 UPDATE #tmpStatus        
    SET limiteDeGanhos = CASE WHEN @temProduto =1 THEN valorInvestido * 2 ELSE 0.0 END  
            
 UPDATE #tmpStatus        
    SET planoAtivo = 'Nenhum'  
        
 UPDATE #tmpStatus         
    SET totalEsquerda = 0, -- dbo.fncTotalPessoasPorLado(@idusuario, 0, null, null), --esquerda        
        totalDireita  = 0 --direita        
        
 UPDATE #tmpStatus         
       SET total = totalEsquerda + totalDireita        
        
 UPDATE #tmpStatus         
       SET totalDireitos = (SELECT COUNT(1) FROM Usuario where IdUsuarioPai = @idusuario)        
        
 SELECT * FROM #tmpStatus        
        
 DROP TABLE #tmpStatus        
END 




GO
/****** Object:  StoredProcedure [dbo].[spc_Pedidos]    Script Date: 25/03/2020 13:36:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    
-- =============================================        
-- Author: Marcelo Drumand         
-- Create date: 18-12-2019         
-- Description: Este Procedimento irá trazer para tela todos os pedidos, para usuario e para Admin        
-- spc_Pedidos null, null, null, 2      
-- =============================================        
CREATE PROCEDURE [dbo].[spc_Pedidos]        
(        
 @IdUsuario varchar(255) = Null,         
 @DataInicio datetime = NULL,        
 @DataFim datetime = NULL,         
 @StatusTransacao int = NULL        
)        
AS        
BEGIN        
 SELECT p.IdPedido      
    ,p.Codigo AS CodidoPedido      
    ,p.IdUsuario      
    ,(SELECT TOP 1 prod.Nome FROM UsuarioProduto up  
       INNER JOIN Produto prod ON prod.idProduto = up.idProduto  
	   WHERE up.Idpedido = p.idPedido
	   ORDER BY prod.Pontos DESC) AS NomeProduto      
    ,p.ValorPedido      
    ,p.ValorTaxa      
    ,p.DataPedido      
    ,s.Nome As StatusTransacao      
    ,p.EnderecoDeposito      
    ,p.UrlPagamento      
    ,p.Cotacao      
    ,p.Quantidade      
 FROM Pedido p      
  INNER JOIN Transacao t ON p.idTransacao = t.IdTransacao      
  INNER JOIN Status s ON s.IdStatus = t.IdStatus        
 WHERE p.DataPedido BETWEEN ISNULL(@DataInicio, p.DataPedido) AND ISNULL(@DataFim, p.DataPedido)        
  AND p.Idusuario = ISNULL(CAST(@IdUsuario as uniqueidentifier), p.Idusuario)        
  AND t.IdStatus = ISNULL(@StatusTransacao, t.IdStatus)        
  AND p.Ativo = 1    
END 




GO
/****** Object:  StoredProcedure [dbo].[spc_limitesGanhos]    Script Date: 25/03/2020 13:37:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	-- =============================================  
	-- Author: Sérgio Junior  
	-- Create Date: 26/12/2019  
	-- Description: Salva log de retorno da crystalpay  
	-- [spc_limitesGanhos] 'B56B2E89-2713-46FB-85E3-629723109F92'
	-- =============================================     
	CREATE PROCEDURE [dbo].[spc_limitesGanhos]  
	(  
	   @idUsuario uniqueidentifier  
	)  
	AS   
	BEGIN  
		DECLARE @GanhosCashBack DECIMAL(18,8)  
		DECLARE @MaximoCashback DECIMAL(18,8)       
		DECLARE @PercentualCashback DECIMAL(18,8)  
		DECLARE @GanhosRede DECIMAL(18,8)       
		DECLARE @MaximoRede DECIMAL(18,8)       
		DECLARE @PercentualRede DECIMAL(18,2)   
		DECLARE @ValorAtual DECIMAL(18,8)
		DECLARE @PercentualAtual DECIMAL(18,2)
		DECLARE @temProduto bit

	  SELECT @temProduto = 1 FROM UsuarioProduto 
	   INNER JOIN Produto ON Produto.IdProduto = UsuarioProduto.IdProduto
	   WHERE IdUsuario = @idUsuario 
		 AND UsuarioProduto.Ativo = 1
		 AND Produto.Pontos > 0
    
		SELECT @ValorAtual = SUM(Produto.Valor)
		  FROM usuarioProduto    
		 INNER JOIN Produto ON Produto.idproduto = usuarioProduto.idproduto    
		 WHERE idusuario = @idUsuario    
		   AND usuarioProduto.Ativo = 1  
		   AND Produto.Pontos > 0
    
		SELECT @GanhosCashBack = SUM(Valor)         
		  FROM  Lancamento WHERE IdUsuario = @IdUsuario        
							 AND IdTipo IN (SELECT IdTipo FROM Tipo WHERE Chave IN ('CB')) 
							 AND Ativo = 1 
							 AND Valor > 0  
  
		SELECT @MaximoCashback = @ValorAtual * 2    
	
		SELECT @GanhosRede = SUM(Valor)         
		  FROM  Lancamento WHERE IdUsuario = @IdUsuario        
							 AND IdTipo NOT IN (SELECT IdTipo FROM Tipo WHERE Chave IN ('TCCASH', 'TCC', 'EST'))
							 AND Ativo = 1 
							 AND Valor > 0   
  
		SELECT @MaximoRede = CASE WHEN @temProduto = 1 THEN @ValorAtual * 2 ELSE 0.0 END   
  
		IF(@MaximoCashback > 0)         
		SELECT @PercentualCashback = (100 / ISNULL(@MaximoCashback,0)) * ISNULL(@GanhosCashBack,0)  
		IF(@MaximoRede > 0)
		SELECT @PercentualRede = (100 / ISNULL(@MaximoRede,0)) * ISNULL(@GanhosRede,0)  
		IF(@ValorAtual > 0)           
		SELECT @PercentualAtual = (100 / ISNULL(10000,0)) * ISNULL(@ValorAtual,0)   
  
		SELECT ISNULL(@GanhosCashBack,0) as GanhoAtualCashback,  
			   ISNULL(@MaximoCashback,0) as GanhoMaximoCashback,  
			   ISNULL(@PercentualCashback,0) as PercentualAtualCashback,  
			   ISNULL(@MaximoRede,0) as GanhoMaximoRede,  
			   ISNULL(@GanhosRede,0) as GanhoAtualRede,  
			   ISNULL(@PercentualRede,0) as PercentualAtualRede  ,
			   ISNULL(@ValorAtual,0) as ValorAtual,  
			   ISNULL(@PercentualAtual,0) as PercentualAtual  
	END