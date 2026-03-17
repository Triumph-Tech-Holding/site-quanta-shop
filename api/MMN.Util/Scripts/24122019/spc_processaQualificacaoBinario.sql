-- =============================================      
-- Author:      Sergio Junior      
-- Create Date: 24/12/2019      
-- Description: Processa a qualificação do usuário ao binário      
-- =============================================       
CREATE PROCEDURE [dbo].[spc_processaQualificacaoBinario]                    
(                                                
   @idusuario UNIQUEIDENTIFIER      
)                                                
AS                                
BEGIN       
 CREATE TABLE #tmpArvore      
 (      
  IdUsuario UNIQUEIDENTIFIER NOT NULL,      
  Posicao   int,      
  LadoPai   int,      
  Nivel   int      
 )      
 CREATE TABLE #tmpDiretos      
 (      
  LadoPai    int,      
  Quantidade int,      
  MenorData  datetime      
 )      
     
 DECLARE @dataQualificacao DATETIME                
 DECLARE @qtdDiretos int      
                      
   SELECT @dataQualificacao = DataQualificacao      
     FROM Usuario       
    WHERE idusuario = @idusuario                
    
  /* adicionado pra aparecer os pontyos ed binario*/  
  --set @dataQualificacao = getdate()  
          
  IF(@dataQualificacao is null )      
  BEGIN      
   SELECT @qtdDiretos = COUNT(1)       
    FROM Usuario      
   INNER JOIN UsuarioProduto UP ON UP.IdUsuario = Usuario.IdUsuario      
   INNER JOIN Pedido Ped        ON UP.IdPedido = Ped.IdPedido  
   WHERE IdUsuarioPai = @idusuario      
    AND UP.Ativo = 1  
    AND Ped.MeioPagamento NOT IN (4,5) -- Pagamentos por voucher não qualificam  
    
   IF(@qtdDiretos >= 2)    
      BEGIN      
     INSERT INTO #tmpArvore      
     SELECT * FROM dbo.[fnc_usuarioArvore](@idusuario, 0);      
       
     INSERT INTO #tmpArvore      
     SELECT * FROM dbo.[fnc_usuarioArvore](@idusuario, 1);      
    
    --Verifica os diretos estão qualificando      
    INSERT INTO #tmpDiretos      
    SELECT Ar.LadoPai, COUNT(1), MIN(P.DataPagamento) dataPagamento      
     FROM Usuario      
    INNER JOIN UsuarioProduto UP ON UP.IdUsuario =  Usuario.IdUsuario      
    INNER JOIN Pedido P ON P.IdPedido = UP.IdPedido      
    INNER JOIN #tmpArvore Ar ON Ar.IdUsuario = Usuario.IdUsuario      
    WHERE IdUsuarioPai = @idusuario      
      AND UP.Ativo = 1      
      AND P.Pago = 1      
      AND P.Ativo = 1      
   AND P.MeioPagamento NOT IN (4,5) -- Pagamentos por voucher não qualificam  
    GROUP BY Ar.LadoPai      
    ORDER BY dataPagamento      
    
    --QUALIFICADO      
    --Tem 1 direto de cada lado      
    IF ((SELECT COUNT(1) FROM #tmpDiretos where LadoPai = 0) > 0 AND      
    (SELECT COUNT(1) FROM #tmpDiretos where LadoPai = 1) > 0)      
    BEGIN      
     UPDATE Usuario       
     SET DataQualificacao = (SELECT MAX(MenorData) FROM #tmpDiretos)      
   WHERE idusuario = @idusuario      
    END      
    END      
  END    
           
  DROP TABLE #tmpDiretos;      
  DROP TABLE #tmpArvore;      
        
END 