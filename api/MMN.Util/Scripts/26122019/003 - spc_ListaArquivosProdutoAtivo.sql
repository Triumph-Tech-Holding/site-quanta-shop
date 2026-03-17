/*************************************************************************
Autor: Proaire / Marcelo Drumand
Data : 09/11/2019
Descrição: Lista arquivo do produto que estiver ativo do usuario

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
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spc_ListaArquivosProdutoAtivo')
   DROP PROCEDURE spc_ListaArquivosProdutoAtivo     
GO

-- =============================================
-- Author: MArcelo Drumand
-- Create Date: 26/12/2019
-- Description: Lista arquivo do produto que estiver ativo do usuario
-- spc_ListaArquivosProdutoAtivo '2509BC5C-5E8B-46BB-ACEF-FEB1C963DD57'
-- =============================================   
CREATE PROCEDURE [dbo].[spc_ListaArquivosProdutoAtivo]
(
	@IdUsuario UNIQUEIDENTIFIER
)
AS
BEGIN

   SELECT
       pa.IdProdutoArquivo
      ,pa.IdProduto
      ,t.Descricao
      ,pa.URL
	  ,pa.Ativo
   FROM Usuario u
   inner join UsuarioProduto up On u.idusuario = up.idusuario
   inner join ProdutoArquivo pa ON up.idproduto = pa.idproduto
   inner join Tipo t On t.idtipo = pa.idtipo
   Where up.ativo = 1 and up.IdUsuario = @IdUsuario   

END
GO
PRINT 'PROCEDIMENTO CRIADO'
