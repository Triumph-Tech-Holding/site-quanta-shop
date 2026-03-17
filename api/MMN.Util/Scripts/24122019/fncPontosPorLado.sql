-- =============================================      
-- Author:      Sergio Junior      
-- Create Date: 24/12/2019      
-- Description: Retorna a quantidade de pontos de cada lado da perna      
-- OBS: Não conta os pontos de qualificação      
-- =============================================       
CREATE FUNCTION [dbo].[fncPontosPorLado]                    
(                                                
   @idusuario UNIQUEIDENTIFIER,      
   @posicao   INT,      
   @dataini   DATETIME,      
   @datafim   DATETIME,  
   @somenteExibicao bit = 0  
)                                                
                           
RETURNS INT                    
AS                                
BEGIN                                
     DECLARE @dataQualificacao DATETIME      
                      
      SELECT @dataQualificacao = ISNULL(DataQualificacao, GETDATE())      
     FROM Usuario       
    WHERE idusuario = @idusuario                
      
    DECLARE @codusuarioum UNIQUEIDENTIFIER ,      
   @total int,       
   @ajuste int                            
                                 
   set @ajuste = 0;                     
                     
   SELECT @codusuarioum = idusuario                                  
     FROM matriz  WITH (NOLOCK)      
    WHERE idmatrizpai = (   SELECT MIN(idmatriz)      
         FROM matriz WITH (NOLOCK)      
        WHERE IdUsuario = @idusuario                             
    )                          
                       
   and posicao = @posicao;                            
                               
   with recursivo (idmatrizpai,idusuario,idmatriz ,posicao,lvl)                                
   as (                          
    SELECT idmatrizpai,      
     IdUsuario,      
     idmatriz,      
           posicao,      
           0                              
      FROM matriz  WITH (NOLOCK)                               
     WHERE IdUsuario = @codusuarioum                                 
     UNION all                                
    SELECT base.idmatrizpai,      
           base.IdUsuario,      
           base.idmatriz,      
           base.posicao,      
           r.lvl +1      
      FROM matriz  base  WITH (NOLOCK)      
     INNER JOIN recursivo r ON r.idmatriz = base.idmatrizpai                                
   )                                
                                   
   SELECT @total = ISNULL(SUM(p.Pontos),0)                   
     FROM recursivo                                 
    INNER JOIN matrizdetalhe s WITH (NOLOCK) ON s.idmatriz = recursivo.idmatriz                   
    INNER JOIN Pedido ped WITH (NOLOCK) ON ped.IdPedido = s.IdPedido               
    INNER JOIN produto p on p.idproduto = s.idproduto      
    INNER JOIN UsuarioProduto Up on Up.IdPedido = ped.IdPedido      
    WHERE s.dataCompra >= isnull(@dataini,s.dataCompra)      
      AND s.dataCompra <= isnull(@datafim,s.dataCompra)  
      AND (s.DataCompra > @dataQualificacao OR @somenteExibicao = 1)  
   AND ped.MeioPagamento <> 4 -- Meio de pagamento 4 não gera pontos  
   AND ped.pago = 1  
   AND Up.Ativo = 1  
  OPTION (MAXRECURSION 0);                                 
  
      
  IF (@total < 0)                            
   SELECT @total  = 0                             
                                
  RETURN  ISNULL(@total,0)                  
END 