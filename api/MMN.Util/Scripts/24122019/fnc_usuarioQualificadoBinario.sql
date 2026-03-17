-- =============================================
-- Author:      Sergio Junior
-- Create Date: 24/12/2019
-- Description: Verifica se o usuário está qualificado
-- ============================================= 
CREATE FUNCTION [dbo].[fnc_usuarioQualificadoBinario]              
(                                          
   @idusuario UNIQUEIDENTIFIER
)                                          
                     
RETURNS INT              
              
AS                          
BEGIN 
	
    DECLARE @dataQualificacao DATETIME          

    SELECT @dataQualificacao = DataQualificacao
	  FROM Usuario 
	 WHERE idusuario = @idusuario          
	   
	IF(@dataQualificacao is null )
	BEGIN
		RETURN 0;
	END
	ELSE 
	BEGIN 
		RETURN 1;
	END
	RETURN 0;
END     