SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Sergio Junior
-- Create Date: 24/12/2019
-- Description: Retorna a arvore do usuário conforme o lado escolhido
-- ============================================= 
CREATE FUNCTION [fnc_usuarioArvore]
(	
	@idusuario UNIQUEIDENTIFIER,
    @lado	   INT
)
RETURNS @temp TABLE 
(
    -- columns returned by the function
    IdUsuario UNIQUEIDENTIFIER NOT NULL,
    Posicao   int,
    LadoPai	  int,
	Nivel	  int
)
AS
BEGIN

	DECLARE @codusuarioum UNIQUEIDENTIFIER ,
			@total int, 
			@ajuste int                      
                           
               
   SELECT @codusuarioum = idusuario                            
     FROM matriz  WITH (NOLOCK)
    WHERE idmatrizpai = (   SELECT MIN(idmatriz)
							  FROM matriz WITH (NOLOCK)
							 WHERE IdUsuario = @idusuario                       
    )                    
                 
   and posicao = @lado;                      
                         
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
   INSERT INTO @temp                          
   SELECT recursivo.idusuario, 
          recursivo.posicao,
		  @lado as lado,
		  recursivo.lvl
     FROM recursivo                           
    INNER JOIN matrizdetalhe s WITH (NOLOCK) ON s.idmatriz = recursivo.idmatriz             
	INNER JOIN Pedido ped WITH (NOLOCK) ON ped.IdPedido = s.IdPedido         
    INNER JOIN UsuarioProduto up on up.IdPedido = ped.IdPedido
    WHERE ped.Ativo = 1
      AND ped.Pago = 1
	  AND up.Ativo = 1;

	RETURN
END
GO