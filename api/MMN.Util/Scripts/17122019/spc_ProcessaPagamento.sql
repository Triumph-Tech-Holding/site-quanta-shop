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

  --spc_ProcessaPagamento  12    
ALTER PROCEDURE [dbo].[spc_processaPagamento]      
(      
 @idPedido bigint      
)      
AS       
BEGIN      
DECLARE @ErrorMessage varchar(500),  
  @ErrorSeverity varchar(500),  
  @ErrorState varchar(500);  
BEGIN TRY  
  
 DECLARE @idUsuario   UNIQUEIDENTIFIER,      
         @idPai       UNIQUEIDENTIFIER,      
   @idMatriz    BIGINT,      
   @dataPgto    DATETIME,      
   @valorCompra DECIMAL(18,8),      
   @idTransacao BIGINT,      
   @idProduto   INT;      
    
 IF((SELECT COUNT(1) FROM Pedido WHERE IdPedido = @idPedido AND DataPagamento is null) > 0 )      
 BEGIN      
  RAISERROR ('Pedido ainda não foi pago, não é possível processar o pagamento.', 16, 1 );        
  
 END      
   
 SELECT @idUsuario = IdUsuario,      
        @dataPgto = DataPagamento,      
        @valorCompra = ValorPedido,      
        @idTransacao = idTransacao   
   FROM Pedido       
  WHERE IdPedido = @idPedido;  
          
 --BUSCA O PAI      
 SELECT @idPai = IdUsuarioPai      
   FROM Usuario      
  WHERE IdUsuario = @idUsuario      
  
 SELECT @idProduto = IdProduto       
   FROM UsuarioProduto       
  WHERE IdPedido = @idPedido;      

--Desativa produtos anteriores    
 UPDATE UsuarioProduto       
    SET Ativo = 0       
  WHERE IdUsuario = @idUsuario      

--Ativa produto atual    
 UPDATE UsuarioProduto       
    SET Ativo = 1       
  WHERE IdPedido = @idPedido;      

--INSERE REFERÊNCIA
 UPDATE Usuario
    SET DataReferencia = @dataPgto
  WHERE IdUsuario = @idUsuario    
      
 SELECT @idMatriz = IdMatriz        
   FROM Matriz       
  WHERE IdUsuario = @idUsuario      
      
 --INSERE REFERENCIA NO BINÁRIO      
 INSERT INTO MatrizDetalhe      
 VALUES (@idMatriz, @idPedido, @idProduto, @dataPgto)      
  
 --PAGA DIRETO      
 IF(@idPai IS NOT NULL)  
 BEGIN  
  INSERT INTO Lancamento      
  SELECT @idPai, @idTransacao,(SELECT IdTipo FROM tipo WHERE Chave = 'AU'), (@valorCompra * 0.10), @dataPgto, 1, 'Pagamento direto - Usuário '+ Usuario.Nome +' ['+ Usuario.Login +']', (SELECT Idstatus FROM status WHERE chaveTraducao = 'Status_Finalizado')
    FROM Usuario      
   WHERE IdUsuario = @idUsuario      

   EXEC spc_processaQualificacaoBinario  @idPai;
 END  
 
 EXEC spc_processaQualificacaoBinario  @idUsuario;

  END TRY  
BEGIN CATCH  
    SET @ErrorMessage  = ERROR_MESSAGE()  
    SET @ErrorSeverity = ERROR_SEVERITY()  
    SET @ErrorState    = ERROR_STATE()  
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)  
END CATCH  
  
END 